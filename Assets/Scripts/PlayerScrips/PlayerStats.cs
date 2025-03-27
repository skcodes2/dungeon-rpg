using UnityEngine;
using System.Collections;

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

    public float health;

    public float maxHealth = 100f;
    public float walkSpeed = 2f;
    public float runSpeed;

    public float tmpWalkSpeed = 0f;
    public float tmpRunSpeed = 0f;

    public float baseDamage = 5f;

    public float armour = 0f;

    public HealthBar healthBar;

    private Animator anim;

    void Start()
    {
        if (healthBar != null)
        {
            healthBar.SetMaxHealth((int)maxHealth);
        }
        else
        {
            Debug.LogWarning("HealthBar is not assigned in PlayerStats.");
        }
    }

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
        health = maxHealth;
        runSpeed = walkSpeed * 1.5f;
        anim = GetComponent<Animator>();
    }

    public void SetMaxHealth(float amount)
    {
        this.maxHealth = amount;
        if (healthBar != null)
        {
            healthBar.SetMaxHealth((int)maxHealth);
        }
        else
        {
            Debug.LogWarning("HealthBar is not assigned in PlayerStats.");
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
    public void TakeDamage(float amount)
    {
        this.health -= amount;
        AudioManager.Instance.Play("hit");
        // Flicker effect (change color to red)
        StartCoroutine(FlickerRed());

        // Ensure healthBar is assigned before using it
        if (this.healthBar != null)
        {
            this.healthBar.SetHealth((int)health);
        }
        else
        {
            Debug.LogWarning("HealthBar is not assigned in PlayerStats.");
        }

        if (this.health <= 0)
        {
            this.health = 0;
            this.anim.SetTrigger("Die");
            this.walkSpeed = 0;
            this.runSpeed = 0;


            // Handle player death (e.g., call a method to handle death)
        }
    }

    public void IncreaseArmour(float amount)
    {
        this.armour += amount;
    }

    public void Heal(float amount)
    {
        this.health += amount;
        if (health > 100f)
        {
            this.health = 100f; // Cap health at 100
        }
    }

    public void IncreaseSpeed(float amount)
    {
        this.walkSpeed += amount;
    }

    public void DecreaseSpeed(float amount)
    {
        this.walkSpeed -= amount;
    }

    public void StopPlayer()
    {
        this.tmpRunSpeed = this.runSpeed;
        this.tmpWalkSpeed = this.walkSpeed;
        this.walkSpeed = 0f;
        this.runSpeed = 0f;
    }

    public void ResumePlayer()
    {
        this.walkSpeed = this.tmpWalkSpeed;
        this.runSpeed = this.tmpRunSpeed;
        this.tmpWalkSpeed = 0f;
        this.tmpRunSpeed = 0f;
    }

    public float getHealth()
    {
        return this.health;
    }
}
