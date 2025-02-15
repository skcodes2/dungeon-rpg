using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    PlayerStats playerStats;
    public Transform AttactHitBox;
    public GameObject Kick; 
    public GameObject Slash;
    Animator anim;

    private Vector2 input;
    private Vector2 lastMoveDirection;
    private bool facingLeft = true;

    bool isKicking = false;
    bool isSlashing = false;
    private bool isWalking = false;
    private bool isRunning = false;
    private bool isMoving = false;

    private float slideDuration = 1.14f;
    private float kickDuration = 0.3f;
    private float kickTimer =  0f;
    private float slashDuration = 0.3f;
    private float slashTimer = 0f;

    IEnumerator Slide()
    {
        playerStats.IncreaseSpeed(1f); // Increase speed during slide
        yield return new WaitForSeconds(slideDuration); // Wait for the slide duration
        playerStats.DecreaseSpeed(1f); // Decrease speed back to normal
    }

    void Awake(){
        playerStats = PlayerStats.Instance;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        ProcessInputs();
        WalkAnimation();

        CheckTimer(ref isKicking, Kick, kickDuration, ref kickTimer);
        CheckTimer(ref isSlashing, Slash, slashDuration, ref slashTimer);

        if(Input.GetKeyDown(KeyCode.Space)){
            anim.SetTrigger("Kick");
            onAttack(ref isKicking, Kick);
        }

        if(Input.GetKeyDown(KeyCode.N)){
            anim.SetTrigger("Slash");
            onAttack(ref isSlashing, Slash);
        }

        if (Input.GetKeyDown(KeyCode.M) && isRunning)
        {
            anim.SetTrigger("slide");
            StartCoroutine(Slide());
        }
    }

    void FixedUpdate()
    {
        // Move the player based on input
        float speed = isRunning ? playerStats.runSpeed : playerStats.walkSpeed;
        rb.linearVelocity = input.normalized * speed;
        if(isMoving){
            Vector3 vec3 = Vector3.left * input.x + Vector3.down * input.y;
            AttactHitBox.rotation = Quaternion.LookRotation(Vector3.forward, vec3);
        }
    }

    void ProcessInputs(){
        // Get input from player (WASD or Arrow Keys)
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        if((moveX != 0 || moveY != 0) && (input.x != 0 || input.y != 0)){
            // Check for 180-degree turn
            // if (Vector2.Dot(lastMoveDirection, input) < -0.9f) // Adjust the threshold as needed
            // {
            //     anim.SetTrigger("Turn180");
            // }

            lastMoveDirection = input;
            isMoving = true;
            isWalking = !Input.GetKey(KeyCode.LeftShift);
            isRunning = Input.GetKey(KeyCode.LeftShift);
        }
        else{
            isWalking = false;
            isRunning = false;
            isMoving = false;
            Vector3 vec3 = Vector3.left * lastMoveDirection.x + Vector3.down * lastMoveDirection.y;
            AttactHitBox.rotation = Quaternion.LookRotation(Vector3.forward, vec3);
        }

        // Set the input vector
        input.x = Input.GetAxisRaw("Horizontal"); // Left (-1) / Right (1)
        input.y = Input.GetAxisRaw("Vertical");   // Down (-1) / Up (1)
    }

    void WalkAnimation(){
        anim.SetFloat("MoveX", input.x);
        anim.SetFloat("MoveY", input.y);
        anim.SetFloat("LastMoveX", lastMoveDirection.x);
        anim.SetFloat("LastMoveY", lastMoveDirection.y);
        anim.SetFloat("MoveMagnitude", input.magnitude);
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isRunning", isRunning);
        if(input.x < 0 && !facingLeft || input.x > 0 && facingLeft){
            Flip();
        }
    }

    void CheckTimer(ref bool isAttacking, GameObject weapon, float attackDuration, ref float attackTimer){
        if(isAttacking){
            attackTimer += Time.deltaTime;
            
            if(attackTimer >= attackDuration){
                isAttacking = false;
                attackTimer = 0;
                weapon.SetActive(false);
            }
        }
    }

    void onAttack(ref bool isAttacking, GameObject weapon){
        if(!isAttacking){
            isAttacking = true;
            weapon.SetActive(true);
        }
    }

    void Flip(){
        facingLeft = !facingLeft;
        Vector3 characterScale = transform.localScale;
        characterScale.x *= -1;
        transform.localScale = characterScale;
    }
}