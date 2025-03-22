using UnityEngine;
using UnityEngine.AI;
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

    private NavMeshAgent agent;
    private Animator animator;
    private bool isAttacking = false; // Track attack state

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerDetection = GetComponent<PlayerDetection>();  

        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = speed; 
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
        if (!isAttacking) 
        {
            isAttacking = true;
            animator.SetBool("IsAttacking", true);
            print("Attack");
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
    }


    public void TakeDamage(float amount, Vector2 attackOrigin, float knockbackForce, Vector2 playerFacingDirection)
    {
        _enemyStats.TakeDamage(amount, this);
        print($"Enemy took {amount} damage. Health left: {_enemyStats.Health}");

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Directly use the player's facing direction for knockback
            Vector2 knockbackVector = playerFacingDirection.normalized * knockbackForce;

            print($"Player's facing direction: {playerFacingDirection}");
            print($"Applying knockback force: {knockbackVector}");

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
        animator.SetTrigger("Die");  
        Destroy(gameObject, 2f);  
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
        agent.speed = newSpeed;  
    }
}
