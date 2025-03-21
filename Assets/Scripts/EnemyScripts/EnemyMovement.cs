using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform target;  // Reference to the player
    [SerializeField] private float speed = 3.5f;  // Speed of the enemy
    [SerializeField] private float attackRange = 1.5f;  // Range for attacking the player
    [SerializeField] private EnemyStats _enemyStats;  // Reference to the EnemyStats
    [SerializeField] private PlayerDetection playerDetection;  // Reference to the PlayerDetection
    
    [SerializeField] private float verticalRangeAbove = 1.6f;  // Tolerance for when the player is above
    [SerializeField] private float verticalRangeBelow = 0.9f;  // Tolerance for when the player is below

    private NavMeshAgent agent;
    private Animator animator;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerDetection = GetComponent<PlayerDetection>();  // Get the PlayerDetection script

        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = speed; // Set the initial speed of the agent
    }

    private void Update()
    {
        // Check if the attack animation is still playing
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // If the attack animation is still playing, don't do anything else
        if (stateInfo.IsName("Attack") && stateInfo.normalizedTime < 1f)
        {
            return;  // Exit if the attack animation is still running
        }
        else
        {
            animator.SetBool("IsAttacking", false);
            
            if (playerDetection.AwareOfPlayer)
            {
                // Player is in range to follow or attack
                float distanceToPlayer = Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z), new Vector3(target.position.x, 0f, target.position.z));
                float verticalDistanceToPlayer = Mathf.Abs(transform.position.y - target.position.y); // Check vertical distance

                // Check if the player is within horizontal attack range and the appropriate vertical range
                if (distanceToPlayer <= attackRange)
                {
                    if (target.position.y > transform.position.y && verticalDistanceToPlayer <= verticalRangeAbove)  // Player is above
                    {
                        AttackPlayer();
                    }
                    else if (target.position.y < transform.position.y && verticalDistanceToPlayer <= verticalRangeBelow)  // Player is below
                    {
                        AttackPlayer();
                    }
                    else
                    {
                        FollowPlayer();
                    }
                }
                else
                {
                    // Player is within detection range but not in attack range
                    FollowPlayer();
                }
            }
            else
            {
                // Player is out of detection range, stop movement
                StopEnemy();
            }
        }
    }

    private void FollowPlayer()
    {
        agent.SetDestination(target.position);
        HandleMovementAnimations();
    }

    private void HandleMovementAnimations()
    {
        // Only handle movement animations if the attack animation is NOT playing
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (!stateInfo.IsName("Attack"))  // Check if the attack animation is NOT playing
        {
            if (agent.velocity.sqrMagnitude > 0.1f)
            {
                animator.SetBool("IsMoving", true);  // Set walking animation
            }
            else
            {
                animator.SetBool("IsMoving", false);  // Set idle animation
            }
        }
    }

    private void AttackPlayer()
    {
        animator.SetBool("IsAttacking", true);
        print("Attack");
        // Stop movement and trigger attack animation
        agent.SetDestination(transform.position);  // Stop movement

        // Stop any other animation instantly by crossfading to the attack animation
        animator.CrossFade("Attack", 0f, 0); // Crossfade with 0 time to instantly switch to Attack animation
    }

    private void StopEnemy()
    {
        agent.SetDestination(transform.position);  // Stay in place
        animator.SetBool("IsMoving", false);  // Set idle animation
    }

    public void TakeDamage(float amount)
    {
        _enemyStats.TakeDamage(amount, this);  // Take damage and pass EnemyMovement as a parameter
        print("Enemy health: " + _enemyStats.Health);
    }

    public void Die()
    {
        animator.SetTrigger("Die");  // Trigger death animation
        Destroy(gameObject, 2f);  // Destroy after animation completes (adjust delay as needed)
    }

    // Optional: Method to dynamically change enemy speed
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
        agent.speed = newSpeed;  // Apply the speed change to the NavMeshAgent
    }
}
