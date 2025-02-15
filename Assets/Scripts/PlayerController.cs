using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    // Components
    private Rigidbody2D rb;
    private Animator anim;
    private PlayerStats playerStats;

    // Public fields
    public Transform AttactHitBox;
    public GameObject Kick; 
    public GameObject Slash;
    public GameObject RunAttack;
    public GameObject SwordSlam;
    public GameObject Pummel;
    public GameObject Swipe;


    // Movement variables
    private Vector2 input;
    private Vector2 lastMoveDirection;
    private bool facingLeft = true;
    private bool isWalking = false;
    private bool isRunning = false;
    private bool isMoving = false;

    // Attack variables
    private bool isKicking = false;
    private float kickTimer = 0f;
    public float kickCooldown = 1.5f;
    private float kickDuration = 0.3f;
    private float kickCooldownTimer = 0f;

    private bool isSlashing = false;
    private float slashDuration = 0.3f;
    private float slashTimer = 0f;
    public float slashCooldown = 1.5f;
    private float slashCooldownTimer = 0f;

    private bool isRunAttacking = false;
    private float runAttackDuration = 0.3f;
    private float runAttackTimer = 0f;
    public float runAttackCooldown = 1.5f;
    private float runAttackCooldownTimer = 0f;

    private bool isSwordSlamming = false;
    private float swordSlamDuration = 0.3f;
    private float swordSlamTimer = 0f;
    public float swordSlamCooldown = 1.5f;
    private float swordSlamCooldownTimer = 0f;

    private bool isPummeling = false;
    private float pummelDuration = 0.4f;
    private float pummelTimer = 0f;
    public float pummelCooldown = 1.5f;
    private float pummelCooldownTimer = 0f;

    private bool isSwiping = false;
    private float swipeDuration = 0.4f;
    private float swipeTimer = 0f;
    public float swipeCooldown = 1.5f;
    private float swipeCooldownTimer = 0f;


    // Slide variables
    private bool isSliding = false;
    private float slideDuration = 1.14f;
    public float slideCooldown = 1.5f;
    private float slideCooldownTimer = 0f;

    private bool isRolling = false;
    private float rollDuration = 1.14f;
    public float rollCooldown = 1.5f;
    private float rollCooldownTimer = 0f;

    void Awake()
    {
        playerStats = PlayerStats.Instance;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        ProcessInputs();
        UpdateAnimations();
        UpdateTimers();
        HandleAttacks();
        HandleSlide();
        HandleRoll();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void ProcessInputs()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        if ((moveX != 0 || moveY != 0) && (input.x != 0 || input.y != 0))
        {
            // Check for 180-degree turn
            // if (Vector2.Dot(lastMoveDirection, input) < -0.9f)
            // {
            //     anim.SetTrigger("Turn180");
            // }

            lastMoveDirection = input;
            isMoving = true;
            isWalking = !Input.GetKey(KeyCode.LeftShift);
            isRunning = Input.GetKey(KeyCode.LeftShift);
        }
        else
        {
            isWalking = false;
            isRunning = false;
            isMoving = false;
        }

        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
    }

    private void UpdateAnimations()
    {
        anim.SetFloat("MoveX", input.x);
        anim.SetFloat("MoveY", input.y);
        anim.SetFloat("LastMoveX", lastMoveDirection.x);
        anim.SetFloat("LastMoveY", lastMoveDirection.y);
        anim.SetFloat("MoveMagnitude", input.magnitude);
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isRunning", isRunning);

        if (input.x < 0 && !facingLeft || input.x > 0 && facingLeft)
        {
            Flip();
        }
        if (input != Vector2.zero)
            {
                float angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg + 90f;
                AttactHitBox.rotation = Quaternion.Euler(0, 0, angle);
            }
    }

    private void UpdateTimers()
    {
        if (kickCooldownTimer > 0)
        {
            kickCooldownTimer -= Time.deltaTime;
        }

        if (slashCooldownTimer > 0)
        {
            slashCooldownTimer -= Time.deltaTime;
        }

        if (slideCooldownTimer > 0)
        {
            slideCooldownTimer -= Time.deltaTime;
        }

        if (runAttackCooldownTimer > 0)
        {
            runAttackCooldownTimer -= Time.deltaTime;
        }

        if (swordSlamCooldownTimer > 0)
        {
            swordSlamCooldownTimer -= Time.deltaTime;
        }

        if(pummelCooldownTimer > 0)
        {
            pummelCooldownTimer -= Time.deltaTime;
        }

        if(swipeCooldownTimer > 0)
        {
            swipeCooldownTimer -= Time.deltaTime;
        }

        CheckTimer(ref isKicking, Kick, kickDuration, ref kickTimer);
        CheckTimer(ref isSlashing, Slash, slashDuration, ref slashTimer);
        CheckTimer(ref isRunAttacking, RunAttack, runAttackDuration, ref runAttackTimer);
        CheckTimer(ref isSwordSlamming, SwordSlam, swordSlamDuration, ref swordSlamTimer);
        CheckTimer(ref isPummeling, Pummel, pummelDuration, ref pummelTimer);
        CheckTimer(ref isSwiping, Swipe, swipeDuration, ref swipeTimer);
    }

    private void HandleAttacks()
    {
        if (Input.GetKeyDown(KeyCode.Space) && kickCooldownTimer <= 0 && !isRunning)
        {
            anim.SetTrigger("Kick");
            onAttack(ref isKicking, Kick);
            kickCooldownTimer = kickCooldown;
        }

        if (Input.GetKeyDown(KeyCode.N) && slashCooldownTimer <= 0 && !isRunning)
        {
            anim.SetTrigger("Slash");
            onAttack(ref isSlashing, Slash);
            slashCooldownTimer = slashCooldown;
        }

        if (Input.GetKeyDown(KeyCode.B) && runAttackCooldownTimer <= 0 && isRunning)
        {
            anim.SetTrigger("SpinAttack");
            onAttack(ref isRunAttacking, RunAttack);
            runAttackCooldownTimer = runAttackCooldown;
        }

        if (Input.GetKeyDown(KeyCode.V) && swordSlamCooldownTimer <= 0 && !isRunning)
        {
            anim.SetTrigger("SwordSlam");
            onAttack(ref isSwordSlamming, SwordSlam);
            swordSlamCooldownTimer = swordSlamCooldown;
        }

        if (Input.GetKeyDown(KeyCode.X) && pummelCooldownTimer <= 0 && !isRunning)
        {
            anim.SetTrigger("Pummel");
            onAttack(ref isPummeling, Pummel);
            pummelCooldownTimer = pummelCooldown;
        }

        if (Input.GetKeyDown(KeyCode.Z) && swipeCooldownTimer <= 0 && !isRunning)
        {
            anim.SetTrigger("Swipe");
            onAttack(ref isSwiping, Swipe);
            swipeCooldownTimer = swipeCooldown;
        }
    }

    private void HandleSlide()
    {
        if (Input.GetKeyDown(KeyCode.M) && isRunning && slideCooldownTimer <= 0)
        {
            anim.SetTrigger("Slide");
            StartCoroutine(Slide());
            slideCooldownTimer = slideCooldown;
        }
    }

    private void HandleRoll()
    {
        if (Input.GetKeyDown(KeyCode.C) && isRunning && rollCooldownTimer <= 0)
        {
            anim.SetTrigger("Roll");
            StartCoroutine(Roll());
            rollCooldownTimer = rollCooldown;
        }
    }

    private void MovePlayer()
    {
        float speed = isRunning ? playerStats.runSpeed : playerStats.walkSpeed;
        if(isKicking){
            speed = playerStats.walkSpeed;
        }
        rb.linearVelocity = input.normalized * speed;

        
           
        
    }

    private void CheckTimer(ref bool isAttacking, GameObject weapon, float attackDuration, ref float attackTimer)
    {
        if (isAttacking)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackDuration * (3/4f))
            {
                weapon.SetActive(true);
            }

            if (attackTimer >= attackDuration)
            {
                isAttacking = false;
                attackTimer = 0;
                weapon.SetActive(false);
            }
        }
    }

    private void onAttack(ref bool isAttacking, GameObject weapon)
    {
        if (!isAttacking)
        {
            isAttacking = true;
        }
    }

    private void Flip()
    {
        facingLeft = !facingLeft;
        Vector3 characterScale = transform.localScale;
        characterScale.x *= -1;
        transform.localScale = characterScale;
    }

    private IEnumerator Slide()
    {
        isSliding = true;
        playerStats.IncreaseSpeed(1f);
        yield return new WaitForSeconds(slideDuration);
        playerStats.DecreaseSpeed(1f);
        isSliding = false;
    }

    private IEnumerator Roll()
    {
        isRolling = true;
        playerStats.IncreaseSpeed(1f);
        yield return new WaitForSeconds(rollDuration);
        playerStats.DecreaseSpeed(1f);
        isRolling = false;
    }
}