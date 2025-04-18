using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Components
    private Rigidbody2D rb;
    private Animator anim;
    private PlayerStats playerStats;

    // Public fields
    public Transform AttactHitBox;
    public GameObject abilityBarUI;
    public GameObject kickGameObject;
    public GameObject slashGameObject;
    public GameObject runAttackGameObject;
    public GameObject swordSlamGameObject;
    public GameObject pummelGameObject;
    public GameObject swipeGameObject;

    public GameObject skillTreeMenu;



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

    // Movement ability variables
    public MovementAbilityController slideController;
    public MovementAbilityController rollController;

    void Awake()
    {
        playerStats = PlayerStats.Instance;
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Initialize weapon controllers
        pummelController = new WeaponController(pummelGameObject, anim, KeyCode.I, 0.5f, 3f, abilityBarUI);
        kickController = new WeaponController(kickGameObject, anim, KeyCode.I, 1f, 3f, abilityBarUI);
        runAttackController = new WeaponController(runAttackGameObject, anim, KeyCode.P, 0.3f, 7f, abilityBarUI);
        swordSlamController = new WeaponController(swordSlamGameObject, anim, KeyCode.P, 1f, 7f, abilityBarUI);
        slashController = new WeaponController(slashGameObject, anim, KeyCode.O, 0.5f, 5f, abilityBarUI);
        swipeController = new WeaponController(swipeGameObject, anim, KeyCode.O, 0.4f, 5f, abilityBarUI);

        // Initialize movement ability controllers
        slideController = new MovementAbilityController(this, anim, KeyCode.Space, 1f, 4f, 1f, abilityBarUI);

        rollController = new MovementAbilityController(this, anim, KeyCode.Space, 0.4f, 4f, 0.5f, abilityBarUI);
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
        if (playerStats.getHealth() <= 0)
        {
            return;
        }
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        //if esc pressed, toggle skill tree menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            skillTreeMenu.SetActive(!skillTreeMenu.activeSelf);
        }


        if ((moveX != 0 || moveY != 0) && (input.x != 0 || input.y != 0))
        {
            lastMoveDirection = input;  // Update the last movement direction
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
        slideController.Update();
        rollController.Update();
    }

    private void UpdateAbilityRunning()
    {
        kickController.SetRunning(isRunning);
        slashController.SetRunning(isRunning);
        runAttackController.SetRunning(isRunning);
        swordSlamController.SetRunning(isRunning);
        pummelController.SetRunning(isRunning);
        swipeController.SetRunning(isRunning);
        slideController.SetRunning(isRunning);
        rollController.SetRunning(isRunning);
    }

    private void UpdateAnimations()
    {
        anim.SetFloat("MoveX", input.x);
        anim.SetFloat("MoveY", input.y);
        anim.SetFloat("LastMoveX", lastMoveDirection.x);  // Set LastMoveX for weapon facing
        anim.SetFloat("LastMoveY", lastMoveDirection.y);  // Set LastMoveY for weapon facing
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

    // Public method to get the last movement direction
    public Vector2 GetLastMoveDirection()
    {
        return lastMoveDirection;
    }
}
