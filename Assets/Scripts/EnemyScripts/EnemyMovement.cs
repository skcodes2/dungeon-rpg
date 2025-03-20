using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] 
    private Transform target;  // Reference to the target (player)
    
    [SerializeField] 
    private float speed = 3.5f;  // Control speed of the enemy
    
    [SerializeField] 
    private EnemyStats _enemyStats;  // Reference to the EnemyStats class
    
    private NavMeshAgent agent;
    private Animator animator;  // Reference to the Animator component
    
    [SerializeField] 
    private float detectionRange = 5f;  // The range at which the enemy starts following the player

    private bool isPlayerInRange = false;  // Track if the player is within detection range

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>(); 
        animator = GetComponent<Animator>();  // Get the Animator component
        
        agent.updateRotation = false; 
        agent.updateUpAxis = false;

        // Set the initial speed of the agent
        agent.speed = speed;
    }

    private void Update()
    {
        // Check if the player is within detection range
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);
        if (distanceToPlayer <= detectionRange)
        {
            // Player is within range, start following
            isPlayerInRange = true;
        }
        else
        {
            // Player is out of range, stop following
            isPlayerInRange = false;
        }

        // If the player is in range, update the destination and handle movement
        if (isPlayerInRange)
        {
            agent.SetDestination(target.position);
            HandleMovementAnimations();
        }
        else
        {
            // If the player is out of range, stop the enemy
            StopEnemy();
        }
    }

    private void HandleMovementAnimations()
    {
        // Handle movement animation
        if (agent.velocity.sqrMagnitude > 0.1f)  // If the enemy is moving
        {
            animator.SetBool("IsMoving", true);  // Set walking animation
        }
        else
        {
            animator.SetBool("IsMoving", false);  // Set idle animation
        }
    }

    // Method to update the speed dynamically
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
        agent.speed = newSpeed;  // Apply the speed change to the NavMeshAgent
    }

    // Method to handle taking damage
    public void TakeDamage(float amount)
    {
        _enemyStats.TakeDamage(amount, this);
        print("Enemy health: " + _enemyStats.Health);
    }

    // Handle enemy death
    public void Die()
    {
        animator.SetTrigger("Die");  // Trigger death animation (make sure to have a death animation in the Animator)
        Destroy(gameObject, 2f);  // Destroy after animation completes (you can adjust the delay)
    }

    private void StopEnemy()
    {
        // Stop the enemy's movement
        agent.SetDestination(transform.position);  // Stay in place (or implement other logic for idle)
        animator.SetBool("IsMoving", false);  // Set idle animation
    }
}
