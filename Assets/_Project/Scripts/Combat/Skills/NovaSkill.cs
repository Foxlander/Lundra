using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NovaSkill : SkillBase
{
    [Header("Nova Settings")]
    [SerializeField] private float novaRadius = 5f;
    [SerializeField] private float novaExpansionSpeed = 8f;
    [SerializeField] private float novaDuration = 0.3f;
    [SerializeField] private GameObject novaVFXPrefab;
    [SerializeField] private LayerMask enemyLayer;

    protected override void Execute(Vector2 targetPosition)
    {
        StartCoroutine(NovaCoroutine());
    }

    private IEnumerator NovaCoroutine()
    {
        float currentRadius = 0f;
        List<Collider2D> alreadyHit = new List<Collider2D>();

        // Spawn VFX
        if (novaVFXPrefab != null)
        {
            Vector3 spawnPos = new Vector3(
                transform.position.x,
                transform.position.y,
                0f
            );
            GameObject vfx = Instantiate(novaVFXPrefab, spawnPos, Quaternion.identity);
            Destroy(vfx, novaDuration + 0.5f);
        }

        while (currentRadius < novaRadius)
        {
            currentRadius += novaExpansionSpeed * Time.deltaTime;
            currentRadius = Mathf.Min(currentRadius, novaRadius);

            Collider2D[] hits = Physics2D.OverlapCircleAll(
                transform.position,
                currentRadius,
                enemyLayer
            );

            foreach (Collider2D hit in hits)
            {
                if (alreadyHit.Contains(hit)) continue;

                CharacterStats targetStats = hit.GetComponent<CharacterStats>();
                if (targetStats != null)
                {
                    DamageSystem.ApplyDamage(OwnerStats, targetStats, damageType);
                    alreadyHit.Add(hit);
                    ApplyKnockback(hit.transform);
                }
            }

            yield return null;
        }
    }

    private void ApplyKnockback(Transform target)
    {
        Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.AddForce(direction * 5f, ForceMode2D.Impulse);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, novaRadius);
    }
}