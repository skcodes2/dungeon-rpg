using UnityEngine;

[System.Serializable]
public class EnemyStats
{
    public float Speed; // Speed at which the enemy moves
    public float RotationSpeed; // Speed of enemy's rotation towards target
    public float Health; // Health of the enemy
    public float Damage; // Damage the enemy can inflict
    public string myLocation;

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
        Debug.Log("Current Health: " + Health);
        if (Health <= 0)
        {
            if(myLocation == "Section1")
            {
                Room1DoorManager.Instance.EnemyDefeatedSection1();
            }
            else if(myLocation == "Section2")
            {
                Room1DoorManager.Instance.EnemyDefeatedSection2();
            }
            enemyMovement.Die();
        }
    }

    public void TakeDamage(float amount, EnemyMovementBoss enemyMovement)
    {
        Health -= amount;

        if (Health <= 0)
        {
            enemyMovement.Die();
        }
    }

    public void TakeDamage(float amount, BossMovement enemyMovement)
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

    public void StopEnemyMovement()
    {
        Speed = 0;
    }
    public void ResumeEnemyMovement()
    {
        Speed = 2;
    }
}