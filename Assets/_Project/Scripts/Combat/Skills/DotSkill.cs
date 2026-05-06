using UnityEngine;

public class DotSkill : SkillBase
{
    [Header("DoT Settings")]
    [SerializeField] private DotType dotType = DotType.Poison;
    [SerializeField] private float damagePerTick = 5f;
    [SerializeField] private float tickInterval = 0.5f;
    [SerializeField] private float dotDuration = 3f;
    [SerializeField] private float projectileSpeed = 8f;
    [SerializeField] private GameObject projectilePrefab;

    protected override void Execute(Vector2 targetPosition)
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning($"No projectile prefab assigned to {skillName}");
            return;
        }

        Vector2 origin = transform.position;
        Vector2 direction = (targetPosition - origin).normalized;

        GameObject projectileObj = Instantiate(
            projectilePrefab,
            origin,
            Quaternion.identity
        );

        DotProjectile dotProjectile = projectileObj.GetComponent<DotProjectile>();
        if (dotProjectile != null)
        {
            dotProjectile.Initialize(
                direction,
                OwnerStats,
                dotType,
                damagePerTick,
                tickInterval,
                dotDuration
            );
        }
    }
}