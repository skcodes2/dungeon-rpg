using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    // Flag to determine if the player is within detection range
    public bool AwareOfPlayer { get; private set; }

    // Direction vector pointing from the enemy to the player
    public Vector2 DirectionToPlayer { get; private set; }

    // The detection radius within which the player is noticed
    [SerializeField] private float _playerAwarenessDistance;

    // Cached reference to the player's transform
    private Transform _player;

    // Reference to the enemy's Animator component
    private Animator _animator;

    // Initialize references when the script starts
    private void Awake()
    {
        // Find the player GameObject by its tag and store its transform
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _player = player.transform;

        // Get the Animator component from the enemy
        _animator = GetComponent<Animator>();
    }

    // Update the awareness status and player direction each frame
    void Update()
    {
        // Calculate the vector pointing from the enemy to the player
        Vector2 enemyToPlayerVector = _player.position - transform.position;

        // Normalize the vector to get the direction
        DirectionToPlayer = enemyToPlayerVector.normalized;

        // Check if the player is within the awareness distance
        if (enemyToPlayerVector.magnitude <= _playerAwarenessDistance)
        {
            AwareOfPlayer = true;

            // Set the IsMoving parameter to true in the animator
            _animator.SetBool("IsMoving", true);

            // Set MoveX and MoveY based on the player's relative position
            _animator.SetFloat("MoveX", DirectionToPlayer.x);
            _animator.SetFloat("MoveY", DirectionToPlayer.y);
        }
        else
        {
            AwareOfPlayer = false;

            // Set the IsMoving parameter to false when the player is not in range
            _animator.SetBool("IsMoving", false);

            // Reset MoveX and MoveY if the player is not detected
            _animator.SetFloat("MoveX", 0f);
            _animator.SetFloat("MoveY", 0f);
        }
    }
}
