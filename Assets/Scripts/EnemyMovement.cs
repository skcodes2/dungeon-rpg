using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Movement speed of the enemy
    [SerializeField]
    private float _speed;

    // Rotation speed of the enemy (degrees per second)
    [SerializeField]
    private float _rotationSpeed;

    // Rigidbody2D reference to handle physics-based movement
    private Rigidbody2D _rigidbody;

    // Reference to the PlayerAwarenessController to get information about the player's position
    private PlayerDetection _playerAwarenessController;

    // Target direction towards the player or zero when no target is detected
    private Vector2 _targetDirection;

    // Initialize references when the script starts
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponent<PlayerDetection>();
    }

    // FixedUpdate is called at fixed time intervals and is used for physics calculations
    private void FixedUpdate()
    {
        // Update the direction towards the player based on awareness status
        UpdateTargetDirection();

        // Rotate the enemy smoothly towards the target direction
        RotateTowardsTarget();

        // Set the velocity of the enemy based on the target direction
        SetVelocity();
    }

    // Update the target direction based on whether the player is detected
    private void UpdateTargetDirection()
    {
        if (_playerAwarenessController.AwareOfPlayer)
        {
            // If the player is detected, set the target direction to the player's direction
            _targetDirection = _playerAwarenessController.DirectionToPlayer;
        }
        else
        {
            // If the player is not detected, stop moving
            _targetDirection = Vector2.zero;
        }
    }

    // Rotate the enemy towards the target direction smoothly
    private void RotateTowardsTarget()
    {
        if (_targetDirection == Vector2.zero)
        {
            // If there's no target direction, don't rotate
            return;
        }

        // Calculate the angle to the target direction using Atan2
        float targetAngle = Mathf.Atan2(_targetDirection.y, _targetDirection.x) * Mathf.Rad2Deg - 90f;

        // Smoothly rotate towards the target angle
        float currentAngle = _rigidbody.rotation;
        float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, _rotationSpeed * Time.deltaTime);

        // Apply the new rotation to the Rigidbody2D
        _rigidbody.rotation = newAngle;
    }

    // Set the movement velocity based on the direction towards the player
    private void SetVelocity()
    {
        if (_targetDirection == Vector2.zero)
        {
            // If no direction, stop the enemy's movement
            _rigidbody.linearVelocity = Vector2.zero;
        }
        else
        {
            // Move in the direction the enemy is facing (transform.up points forward)
            _rigidbody.linearVelocity = transform.up * _speed;
        }
    }
}