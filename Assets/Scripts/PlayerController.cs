using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the player
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
    }

    void Update()
    {
        // Get input from player (WASD or Arrow Keys)
        movement.x = Input.GetAxisRaw("Horizontal"); // Left (-1) / Right (1)
        movement.y = Input.GetAxisRaw("Vertical");   // Down (-1) / Up (1)
    }

    void FixedUpdate()
    {
        // Move the player based on input
        rb.linearVelocity = movement.normalized * moveSpeed;
    }
}