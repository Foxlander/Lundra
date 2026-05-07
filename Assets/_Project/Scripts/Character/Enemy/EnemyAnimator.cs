using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Animator _animator;
    private EnemyBase _enemy;
    private Rigidbody2D _rb;
    private bool _isDead = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<EnemyBase>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_isDead) return;
        if (_animator == null) return;

        UpdateMovementAnimation();
        UpdateFlip();
    }

    private void UpdateMovementAnimation()
    {
        bool isMoving = _rb != null && _rb.linearVelocity.magnitude > 0.1f;
        _animator.SetBool("IsWalking", isMoving);
    }

    private void UpdateFlip()
    {
        if (_rb.linearVelocity.x < -0.1f)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (_rb.linearVelocity.x > 0.1f)
            transform.localScale = new Vector3(1, 1, 1);
    }

    public void TriggerAttack()
    {
        if (_animator != null)
            _animator.SetTrigger("Attack");
    }

    public void TriggerDeath()
    {
        _isDead = true;
        if (_animator != null)
        {
            _animator.SetBool("IsWalking", false);
            _animator.SetTrigger("Death");
            
            // Désactive l'Animator après la durée du clip
            StartCoroutine(DisableAfterDeath());
        }
    }

    private System.Collections.IEnumerator DisableAfterDeath()
    {
        // Attend la fin du clip Death en cours
        yield return null; // attend 1 frame que le trigger soit pris en compte
        
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        float duration = stateInfo.length;
        
        yield return new WaitForSeconds(duration);
        
        if (_animator != null)
            _animator.enabled = false;
    }
}