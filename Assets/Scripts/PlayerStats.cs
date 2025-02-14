using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private static PlayerStats _instance;
    public static PlayerStats Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<PlayerStats>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("PlayerStats");
                    _instance = obj.AddComponent<PlayerStats>();
                    DontDestroyOnLoad(obj);
                }
            }
            return _instance;
        }
    }

    public float health = 100f;
    public float walkSpeed = 2f;
    public float runSpeed = 4f;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            health = 0;
            // Handle player death (e.g., call a method to handle death)
        }
    }

    public void Heal(float amount)
    {
        health += amount;
        if (health > 100f)
        {
            health = 100f; // Cap health at 100
        }
    }

    
}
