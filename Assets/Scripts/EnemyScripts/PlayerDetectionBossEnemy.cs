using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectionBossEnemy : MonoBehaviour
{
    public bool AwareOfPlayer { get; private set; } = true;
    public Vector2 DirectionToPlayer { get; private set; }

    [SerializeField] private float _playerAwarenessDistance; // Now unused, can remove if unneeded

    private Transform _player;
    private Animator _animator;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _player = player.transform;
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector2 enemyToPlayerVector = _player.position - transform.position;
        DirectionToPlayer = enemyToPlayerVector.normalized;

        // Always tracking
        _animator.SetBool("IsMoving", true);
        _animator.SetFloat("MoveX", DirectionToPlayer.x);
        _animator.SetFloat("MoveY", DirectionToPlayer.y);
    }
}
