using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField]
    private float _damageAmount; // Amount of damage the weapon deals

    private void OnTriggerEnter2D(Collider2D collision)
    {
       

        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Get the EnemyMovement component from the collided enemy
            EnemyMovement enemyMovement = collision.gameObject.GetComponent<EnemyMovement>();
            
            if (enemyMovement != null)
            {
                // Apply damage to the enemy
                enemyMovement.TakeDamage(_damageAmount);
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
