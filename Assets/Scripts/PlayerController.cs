using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the player
    private Rigidbody2D rb;
    private Vector2 input;
    Animator anim;
    private Vector2 lastMoveDirection;
    private bool facingLeft = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        ProcessInputs();
        AnimateMovement();
        if(input.x < 0 && !facingLeft || input.x > 0 && facingLeft){
            Flip();
        }
        
    }

    void FixedUpdate()
    {
        // Move the player based on input
        rb.linearVelocity = input.normalized * moveSpeed;
    }

    void ProcessInputs(){
        // Get input from player (WASD or Arrow Keys)
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        if((moveX != 0 || moveY != 0) && (input.x != 0 || input.y != 0)){
            lastMoveDirection = input;
        }

        // Set the input vector
        input.x = Input.GetAxisRaw("Horizontal"); // Left (-1) / Right (1)
        input.y = Input.GetAxisRaw("Vertical");   // Down (-1) / Up (1)

    }

    void AnimateMovement(){
        anim.SetFloat("MoveX", input.x);
        anim.SetFloat("MoveY", input.y);
        anim.SetFloat("LastMoveX", lastMoveDirection.x);
        anim.SetFloat("LastMoveY", lastMoveDirection.y);
        anim.SetFloat("MoveMagnitude", input.magnitude);
    }

    void Flip(){
       facingLeft = !facingLeft;
    Vector3 characterScale = transform.localScale;
    characterScale.x *= -1;
    transform.localScale = characterScale;
    }

}