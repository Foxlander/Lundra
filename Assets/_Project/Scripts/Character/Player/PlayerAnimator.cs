using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;
    private PlayerController _controller;
    private bool _isDead = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (_isDead) return;
        if (_animator == null) return;

        UpdateMovementAnimation();
    }

    private void UpdateMovementAnimation()
    {
        bool isMoving = _controller != null && _controller.IsMoving;
        _animator.SetBool("IsWalking", isMoving);
    }

    public void TriggerDeath()
    {
        _isDead = true;

        if (_animator != null)
        {
            _animator.SetBool("IsWalking", false);
            _animator.SetTrigger("Death");
        }
    }
}