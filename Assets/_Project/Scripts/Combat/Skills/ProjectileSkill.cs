using UnityEngine;

public class ProjectileSkill : SkillBase
{
    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint; // Point de départ du projectile
    [SerializeField] private int projectileCount = 1; // Nombre de projectiles
    [SerializeField] private float spreadAngle = 0f; // Angle entre les projectiles

    protected override void Execute(Vector2 targetPosition)
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning($"No projectile prefab assigned to {skillName}");
            return;
        }

        Vector2 origin = firePoint != null ? firePoint.position : transform.position;
        Vector2 baseDirection = (targetPosition - origin).normalized;

        if (projectileCount == 1)
        {
            SpawnProjectile(origin, baseDirection);
            return;
        }

        // Plusieurs projectiles avec spread
        float totalSpread = spreadAngle * (projectileCount - 1);
        float startAngle = -totalSpread / 2f;

        for (int i = 0; i < projectileCount; i++)
        {
            float angle = startAngle + (spreadAngle * i);
            Vector2 direction = RotateVector(baseDirection, angle);
            SpawnProjectile(origin, direction);
        }
    }

    private void SpawnProjectile(Vector2 origin, Vector2 direction)
    {
        GameObject projectileObj = Instantiate(projectilePrefab, origin, Quaternion.identity);
        ProjectileBase projectile = projectileObj.GetComponent<ProjectileBase>();

        if (projectile != null)
            projectile.Initialize(direction, OwnerStats, damageType);
    }

    private Vector2 RotateVector(Vector2 vector, float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        return new Vector2(
            vector.x * Mathf.Cos(rad) - vector.y * Mathf.Sin(rad),
            vector.x * Mathf.Sin(rad) + vector.y * Mathf.Cos(rad)
        );
    }
}