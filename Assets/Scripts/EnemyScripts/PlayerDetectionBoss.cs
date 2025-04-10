using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectionBoss : MonoBehaviour
{
    public bool AwareOfPlayer { get; private set; } = true; // Always aware
    public Vector2 DirectionToPlayer { get; private set; }

    private Transform _player;
    private Animator _animator;

    public bool IsPhase3 { get; set; } = false;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _player = player.transform;
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!IsPhase3)
        {
            _animator.SetBool("IsMoving", false);
            _animator.SetFloat("MoveX", 0f);
            _animator.SetFloat("MoveY", 0f);
            return;
        }

        Vector2 enemyToPlayerVector = _player.position - transform.position;
        DirectionToPlayer = enemyToPlayerVector.normalized;

        _animator.SetBool("IsMoving", true);
        _animator.SetFloat("MoveX", DirectionToPlayer.x);
        _animator.SetFloat("MoveY", DirectionToPlayer.y);
    }
}
