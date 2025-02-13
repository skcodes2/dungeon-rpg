using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed; // Speed at which the enemy moves

    [SerializeField]
    private float _rotationSpeed; // Speed of enemy's rotation towards target

    [SerializeField]
    private float _screenBorder; // Distance from screen borders to change direction

    [SerializeField]
    private float _obstacleCheckCircleRadius; // Radius for obstacle detection

    [SerializeField]
    private float _obstacleCheckDistance; // Distance ahead for obstacle checks

    [SerializeField]
    private LayerMask _obstacleLayerMask; // Layer mask for detecting obstacles

    private Rigidbody2D _rigidbody; // Reference to Rigidbody2D for movement
    private PlayerDetection _playerAwarenessController; // Reference to player detection script
    private Vector2 _targetDirection; // Current movement direction
    private float _changeDirectionCooldown; // Cooldown timer for random direction changes
    private Camera _camera; // Reference to the camera
    private RaycastHit2D[] _obstacleCollisions; // Array to store obstacle collisions
    private float _obstacleAvoidanceCooldown; // Cooldown timer for avoiding obstacles
    private Vector2 _obstacleAvoidanceTargetDirection; // Direction to avoid obstacles

    private void Awake()
    {
        // Initialize references
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponent<PlayerDetection>();
        _targetDirection = transform.up; // Initially facing up
        _camera = Camera.main;
        _obstacleCollisions = new RaycastHit2D[10]; // Array to store obstacle collisions
    }

    private void FixedUpdate()
    {
        // If not aware of the player, stay idle
        if (!_playerAwarenessController.AwareOfPlayer)
        {
            _rigidbody.linearVelocity = Vector2.zero; // Stop movement
            return;
        }

        // If aware of the player, update movement logic
        UpdateTargetDirection();
        RotateTowardsTarget();
        SetVelocity();
    }

    private void UpdateTargetDirection()
    {
        // Determine the target direction based on different factors
        if (_playerAwarenessController.AwareOfPlayer)
        {
            // Track and move towards the player if detected
            _targetDirection = _playerAwarenessController.DirectionToPlayer;
        }
        else
        {
            // Only change direction randomly if not aware of the player
            HandleRandomDirectionChange();
        }

        HandleObstacles();  // Handle obstacles by changing direction
        HandleEnemyOffScreen();  // Ensure enemy stays within screen bounds
    }

    private void HandleRandomDirectionChange()
    {
        // Cooldown timer for random direction change
        _changeDirectionCooldown -= Time.deltaTime;

        if (_changeDirectionCooldown <= 0)
        {
            // Apply a random angle to change the movement direction
            float angleChange = Random.Range(-90f, 90f);
            Quaternion rotation = Quaternion.AngleAxis(angleChange, transform.forward);
            _targetDirection = rotation * _targetDirection;

            // Reset the cooldown timer for next random change
            _changeDirectionCooldown = Random.Range(1f, 5f);
        }
    }

    private void HandleObstacles()
    {
        // Cooldown for obstacle avoidance
        _obstacleAvoidanceCooldown -= Time.deltaTime;

        // Set up the contact filter to detect obstacles in the specified layer
        var contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(_obstacleLayerMask);

        // Cast a circle to detect obstacles ahead of the enemy
        int numberOfCollisions = Physics2D.CircleCast(
            transform.position,
            _obstacleCheckCircleRadius,
            transform.up,
            contactFilter,
            _obstacleCollisions,
            _obstacleCheckDistance);

        // Loop through all detected obstacles
        for (int index = 0; index < numberOfCollisions; index++)
        {
            var obstacleCollision = _obstacleCollisions[index];

            // Skip self-collisions
            if (obstacleCollision.collider.gameObject == gameObject)
            {
                continue;
            }

            // If the cooldown is over, avoid the obstacle
            if (_obstacleAvoidanceCooldown <= 0)
            {
                _obstacleAvoidanceTargetDirection = obstacleCollision.normal; // Get the direction to avoid the obstacle
                _obstacleAvoidanceCooldown = 0.5f;  // Reset cooldown for next avoidance
            }

            // Rotate the enemy to avoid the obstacle
            var targetRotation = Quaternion.LookRotation(transform.forward, _obstacleAvoidanceTargetDirection);
            var rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

            _targetDirection = rotation * Vector2.up; // Update target direction
            break; // Only avoid one obstacle at a time
        }
    }

    private void HandleEnemyOffScreen()
    {
        // Get the screen position of the enemy
        Vector2 screenPosition = _camera.WorldToScreenPoint(transform.position);

        // Change direction if the enemy is near screen borders
        if ((screenPosition.x < _screenBorder && _targetDirection.x < 0) ||
            (screenPosition.x > _camera.pixelWidth - _screenBorder && _targetDirection.x > 0))
        {
            _targetDirection = new Vector2(-_targetDirection.x, _targetDirection.y);  // Reverse X direction
        }

        if ((screenPosition.y < _screenBorder && _targetDirection.y < 0) ||
            (screenPosition.y > _camera.pixelHeight - _screenBorder && _targetDirection.y > 0))
        {
            _targetDirection = new Vector2(_targetDirection.x, -_targetDirection.y);  // Reverse Y direction
        }
    }

    private void RotateTowardsTarget()
    {
        // Smoothly rotate the enemy towards the target direction
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _targetDirection);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        _rigidbody.SetRotation(rotation); // Apply the rotation to the Rigidbody2D
    }

    private void SetVelocity()
    {
        // Move the enemy forward based on its current direction
        _rigidbody.linearVelocity = transform.up * _speed;
    }
}
