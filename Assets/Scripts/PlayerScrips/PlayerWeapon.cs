using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField]
    private float _damageAmount; // Amount of damage the weapon deals

    // Variable to store the last direction the player was facing
    private Vector2 lastFacingDirection = Vector2.zero;

    // Reference to the PlayerController (assign in the Inspector or via code)
    private PlayerController playerController;

    private void Start()
    {
        // Find the PlayerController component attached to the player (you could also set this reference via the Inspector)
        playerController = GetComponentInParent<PlayerController>();
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Get the EnemyMovement component from the collided enemy
            EnemyMovement enemyMovement = collision.gameObject.GetComponent<EnemyMovement>();

            if (enemyMovement != null)
            {
                // Apply damage and knockback to the enemy, using lastFacingDirection
                enemyMovement.TakeDamage(
                    _damageAmount + PlayerStats.Instance.baseDamage, 
                    transform.position, 
                    6f, 
                    playerController.GetLastMoveDirection() // Use the last known facing direction
                );
            }
            else
            {
                Debug.Log("No EnemyMovement component found on: " + collision.gameObject.name);
            }
        }
        else
        {
            Debug.Log("Collision with non-enemy object: " + collision.gameObject.name);
        }
    }
}
