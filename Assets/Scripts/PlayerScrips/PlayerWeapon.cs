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
            // Try EnemyMovement
            var enemyMovement = collision.gameObject.GetComponent<EnemyMovement>();
            if (enemyMovement != null)
            {
                enemyMovement.TakeDamage(
                    _damageAmount + PlayerStats.Instance.baseDamage,
                    transform.position,
                    6f,
                    playerController.GetLastMoveDirection()
                );
                return;
            }

            // Try EnemyMovementBoss
            var enemyBoss = collision.gameObject.GetComponent<EnemyMovementBoss>();
            if (enemyBoss != null)
            {
                enemyBoss.TakeDamage(
                    _damageAmount + PlayerStats.Instance.baseDamage,
                    transform.position,
                    6f,
                    playerController.GetLastMoveDirection()
                );
                return;
            }

            // Try BossMovement
            var bossMovement = collision.gameObject.GetComponent<BossMovement>();
            if (bossMovement != null)
            {
                bossMovement.TakeDamage(
                    _damageAmount + PlayerStats.Instance.baseDamage,
                    transform.position,
                    6f,
                    playerController.GetLastMoveDirection()
                );
                return;
            }

            Debug.Log("Enemy does not have a compatible movement script: " + collision.gameObject.name);
        }
        else
        {
            Debug.Log("Collision with non-enemy object: " + collision.gameObject.name);
        }
    }

}
