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
    [SerializeField]
    private float _playerAwarenessDistance;

    // Cached reference to the player's transform
    private Transform _player;

    // Initialize references when the script starts
    private void Awake()
    {
        // Find the player GameObject by its tag and store its transform
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _player = player.transform;
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
        }
        else
        {
            AwareOfPlayer = false;
        }
    }
}