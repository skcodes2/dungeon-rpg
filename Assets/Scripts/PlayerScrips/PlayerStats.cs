using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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
            healthBar.SetHealth((int)health);
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
    private IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(1.5f); // play death animation for 1.5 seconds
        SceneManager.LoadScene("GameOverScene"); // game over scene
    }
    public void TakeDamage(float amount)
    {
        this.health -= amount - this.armour;
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

            StartCoroutine(HandleDeath());
        }
    }

    public void IncreaseArmour(float amount)
    {
        this.armour += amount;
    }

    public void Heal(float amount)
    {
        this.health += amount;
        this.healthBar.SetHealth((int)this.health);
        if (this.health > this.maxHealth)
        {
            this.health = this.maxHealth; // Cap health at 100
            this.healthBar.SetHealth(this.maxHealth);
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
