using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 input;
    Animator anim;
    private Vector2 lastMoveDirection;
    private bool facingLeft = true;
    private bool isMoving = false;
    PlayerStats playerStats;
    public Transform AttactHitBox;
    public GameObject Kick; 
    private float attackDuration = 0.3f;
    private float attackTimer =  0f;
    bool isAttacking = false;
    

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
        CheckKickTimer();
        if(Input.GetKeyDown(KeyCode.Space)){
            anim.SetTrigger("Kick");
            onAttack();
        }
     
    }

    void FixedUpdate()
    {
        // Move the player based on input
        rb.linearVelocity = input.normalized * playerStats.moveSpeed;
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
            lastMoveDirection = input;
            isMoving = true;
        }
        else{
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
        if(input.x < 0 && !facingLeft || input.x > 0 && facingLeft){
            Flip();
        }
        
    }

    void CheckKickTimer(){
        if(isAttacking){
            attackTimer += Time.deltaTime;
            if(attackTimer >= attackDuration){
                isAttacking = false;
                attackTimer = 0;
                Kick.SetActive(false);
            }
        }
    }

    void onAttack(){
        if(!isAttacking){
            isAttacking = true;
            Kick.SetActive(true);
            
        }
    }

    void Flip(){
        facingLeft = !facingLeft;
        Vector3 characterScale = transform.localScale;
        characterScale.x *= -1;
        transform.localScale = characterScale;
    }

}