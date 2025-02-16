using UnityEngine;

[System.Serializable]
public class EnemyStats
{
    public float Speed; // Speed at which the enemy moves
    public float RotationSpeed; // Speed of enemy's rotation towards target
    public float Health; // Health of the enemy
    public float Damage; // Damage the enemy can inflict

    public void Heal(float amount)
    {
        Health += amount;
        if (Health > 100) // Assuming 100 is the max health
        {
            Health = 100;
        }
    }

    public void TakeDamage(float amount, EnemyMovement enemyMovement)
    {
        Health -= amount;
        
        if (Health <= 0)
        {
            enemyMovement.Die();
        }
    }

    public float DealDamage()
    {
        return Damage;
    }

    private void Die()
    {
        // Handle enemy death (e.g., play animation, destroy game object, etc.)
        // This method can be expanded or overridden in a derived class if needed
        Debug.Log("Enemy died");
    }
}