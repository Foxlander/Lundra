using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private float damage = 0f; // 0 = utilise les stats de l'attaquant

    private Vector2 _direction;
    private CharacterStats _ownerStats;
    private Rigidbody2D _rb;
    private DamageType _damageType = DamageType.Physical;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Se détruit après lifetime secondes
        Destroy(gameObject, lifetime);
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = _direction * speed;
    }

    // Initialise le projectile
    public void Initialize(Vector2 direction, CharacterStats ownerStats, DamageType damageType = DamageType.Physical)
    {
        _direction = direction.normalized;
        _ownerStats = ownerStats;
        _damageType = damageType;

        // Rotation du sprite selon la direction
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ignore l'owner du projectile
        if (other.gameObject == _ownerStats?.gameObject) return;

        CharacterStats targetStats = other.GetComponent<CharacterStats>();

        if (targetStats != null)
        {
            if (damage > 0)
                DamageSystem.ApplyRawDamage(targetStats, damage, _damageType);
            else
                DamageSystem.ApplyDamage(_ownerStats, targetStats, _damageType);

            Destroy(gameObject);
        }
    }
}