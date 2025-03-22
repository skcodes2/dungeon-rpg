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
    public GameObject kickGameObject;
    public GameObject slashGameObject;
    public GameObject runAttackGameObject;
    public GameObject swordSlamGameObject;
    public GameObject pummelGameObject;
    public GameObject swipeGameObject;

    // Movement variables
    private Vector2 input;
    private Vector2 lastMoveDirection;
    private bool facingLeft = true;
    private bool isWalking = false;
    private bool isRunning = false;

    // Attack variables
    public WeaponController kickController;
    public WeaponController slashController;
    public WeaponController runAttackController;
    public WeaponController swordSlamController;
    public WeaponController pummelController;
    public WeaponController swipeController;

    // // Movement ability controllers
    // public MovementAbilityController slideController;
    // public MovementAbilityController rollController;

    void Awake()
    {
        playerStats = PlayerStats.Instance;
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Initialize weapon controllers  
        pummelController = new WeaponController(pummelGameObject, anim, KeyCode.M, 0.5f, 1.5f);
        kickController = new WeaponController(kickGameObject, anim, KeyCode.N, 1f, 1.5f);


        runAttackController = new WeaponController(runAttackGameObject, anim, KeyCode.B, 0.3f, 1.5f);
        swordSlamController = new WeaponController(swordSlamGameObject, anim, KeyCode.V, 1f, 1.5f);

        slashController = new WeaponController(slashGameObject, anim, KeyCode.C, 0.5f, 1.5f);
        swipeController = new WeaponController(swipeGameObject, anim, KeyCode.X, 0.4f, 1.5f);

        // Initialize movement ability controllers
        // slideController = new MovementAbilityController(this, anim, KeyCode.Space, 1f, 1.5f, 1f);

        // rollController = new MovementAbilityController(this, anim, KeyCode.Space, 0.4f, 1.5f, 1f);
    }

    void Update()
    {
        ProcessInputs();
        UpdateAnimations();
        UpdateAbilities();
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
            lastMoveDirection = input;
            isWalking = !Input.GetKey(KeyCode.LeftShift);
            isRunning = Input.GetKey(KeyCode.LeftShift);
            UpdateAbilityRunning();
        }
        else
        {
            isWalking = false;
            isRunning = false;
        }

        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
    }

    private void UpdateAbilities()
    {
        kickController.Update();
        slashController.Update();
        runAttackController.Update();
        swordSlamController.Update();
        pummelController.Update();
        swipeController.Update();
        // slideController.Update();
        // rollController.Update();
    }

    private void UpdateAbilityRunning()
    {
        kickController.SetRunning(isRunning);
        slashController.SetRunning(isRunning);
        runAttackController.SetRunning(isRunning);
        swordSlamController.SetRunning(isRunning);
        pummelController.SetRunning(isRunning);
        swipeController.SetRunning(isRunning);
        // slideController.SetRunning(isRunning);
        // rollController.SetRunning(isRunning);
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

    private void MovePlayer()
    {
        float speed = isRunning ? playerStats.runSpeed : playerStats.walkSpeed;
        rb.linearVelocity = input.normalized * speed;
    }

    private void Flip()
    {
        facingLeft = !facingLeft;
        Vector3 characterScale = transform.localScale;
        characterScale.x *= -1;
        transform.localScale = characterScale;
    }
}