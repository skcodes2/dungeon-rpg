using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 3.5f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private EnemyStats _enemyStats;
    [SerializeField] private PlayerDetection playerDetection;

    [SerializeField] private float verticalRangeAbove = 1.5f;
    [SerializeField] private float verticalRangeBelow = 1.0f;

    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int coinDropCount = 3;

    [Header("Health Bar UI")]
    [SerializeField] private GameObject healthBarPrefab;      // assign EnemyHealthBar.prefab
    [SerializeField] private Vector3 healthBarOffset = new Vector3(0, 1.2f, 0);

    private GameObject healthBarInstance;
    private Slider healthSlider;
    private float maxHealth;

    private NavMeshAgent agent;
    private Animator animator;
    private bool isAttacking = false; // Track attack state

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerDetection = GetComponent<PlayerDetection>();
        target = GameObject.FindWithTag("Player").transform;

        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = speed;

        maxHealth = _enemyStats.Health;
        healthBarInstance = Instantiate(
            healthBarPrefab,
            transform.position + healthBarOffset,
            Quaternion.identity,
            transform
        );
        healthSlider = healthBarInstance.GetComponentInChildren<Slider>();
        healthSlider.value = 1f;
    }

    private void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Reset attack state if animation is done
        if (stateInfo.IsName("Attack") && stateInfo.normalizedTime >= 1f)
        {
            isAttacking = false;
        }

        if (isAttacking)
            return; // Skip movement logic while attacking

        animator.SetBool("IsAttacking", false);

        if (playerDetection.AwareOfPlayer)
        {
            float distanceToPlayer = Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z),
                                                    new Vector3(target.position.x, 0f, target.position.z));
            float verticalDistanceToPlayer = Mathf.Abs(transform.position.y - target.position.y);

            if (distanceToPlayer <= attackRange)
            {
                if (target.position.y > transform.position.y && verticalDistanceToPlayer <= verticalRangeAbove ||
                    target.position.y < transform.position.y && verticalDistanceToPlayer <= verticalRangeBelow)
                {
                    if (!stateInfo.IsName("Attack")) // Prevent re-triggering attack mid-animation
                    {
                        AttackPlayer();
                    }
                }
                else
                {
                    FollowPlayer();
                }
            }
            else
            {
                FollowPlayer();
            }
        }
        else
        {
            StopEnemy();
        }
    }

    private void FollowPlayer()
    {
        if (PlayerStats.Instance.getHealth() <= 0)
        {
            StopEnemy();
            return;
        }
        agent.SetDestination(target.position);
        HandleMovementAnimations();
    }

    private void HandleMovementAnimations()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (!stateInfo.IsName("Attack"))
        {
            animator.SetBool("IsMoving", agent.velocity.sqrMagnitude > 0.1f);
        }
    }

    private void AttackPlayer()
    {
        if (PlayerStats.Instance.getHealth() <= 0)
        {
            StopEnemy();
            return;
        }
        if (!isAttacking)
        {
            isAttacking = true;
            animator.SetBool("IsAttacking", true);

            agent.SetDestination(transform.position);

            // Cancel current animation and directly play attack animation
            animator.Play("Attack", 0, 0f); // Play attack immediately, canceling other animations

            // Apply damage to the player when attack animation is triggered
            if (target != null)
            {
                PlayerStats.Instance.TakeDamage(damage); // Call TakeDamage on PlayerStats
            }
        }
    }

    private void StopEnemy()
    {

        agent.SetDestination(transform.position);
        animator.SetBool("IsMoving", false);
        _enemyStats.StopEnemyMovement();
    }


    public void TakeDamage(float amount, Vector2 attackOrigin, float knockbackForce, Vector2 playerFacingDirection)
    {
        _enemyStats.TakeDamage(amount, this);
        AudioManager.Instance.Play("hit");
        print($"Enemy took {amount} damage. Health left: {_enemyStats.Health}");

        if (healthSlider != null)
            healthSlider.value = _enemyStats.Health / maxHealth;

        // Flicker effect (change color to red)
        StartCoroutine(FlickerRed());

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Apply knockback based on player's facing direction
            Vector2 knockbackVector = playerFacingDirection.normalized * knockbackForce;


            // Apply the knockback velocity to the Rigidbody2D
            rb.linearVelocity = knockbackVector;

            // Gradually reduce the knockback over time
            StartCoroutine(ReduceKnockback(rb)); // Gradually stop movement
        }
        else
        {
            print("Rigidbody2D not found on enemy!");
        }
    }

    private IEnumerator FlickerRed()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            // Change color to red
            spriteRenderer.color = Color.red;

            // Wait for a short duration (flicker effect)
            yield return new WaitForSeconds(0.1f);

            // Change back to original color
            spriteRenderer.color = Color.white;
        }
    }


    private IEnumerator ReduceKnockback(Rigidbody2D rb)
    {
        while (rb.linearVelocity.magnitude > 0.1f) // Stop when almost still
        {
            rb.linearVelocity *= 0.9f; // Reduce velocity over time
            yield return new WaitForSeconds(0.05f); // Adjust timing if needed
        }
        rb.linearVelocity = Vector2.zero; // Fully stop movement
    }


    public void Die()
    {
        if (healthBarInstance != null)
            Destroy(healthBarInstance);

        // Stop the NavMeshAgent from moving and tracking the player
        if (agent != null)
        {
            agent.isStopped = true;   // Stop the agent from moving
            agent.velocity = Vector3.zero; // Reset the velocity
        }

        DropCoins();

        // Trigger the death animation
        animator.SetTrigger("Die");

        // Destroy the enemy after the death animation has finished (10 seconds here)
        Destroy(gameObject, 1.6f);
    }

    private void DropCoins()
    {
        if (coinPrefab == null)
        {
            Debug.LogWarning("Coin prefab not set in EnemyMovement.");
            return;
        }

        for (int i = 0; i < coinDropCount; i++)
        {
            // Random slight offset so coins don't stack on each other
            Vector3 offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.2f, 0.2f), 0);
            Instantiate(coinPrefab, transform.position + offset, Quaternion.identity);
        }
    }


    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
        agent.speed = newSpeed;
    }

    private void LateUpdate()
    {
        if (healthBarInstance != null)
            healthBarInstance.transform.rotation = Camera.main.transform.rotation;
    }
}
