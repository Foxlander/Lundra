using UnityEngine;

public class DotProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float lifetime = 3f;

    private Vector2 _direction;
    private CharacterStats _ownerStats;
    private Rigidbody2D _rb;

    // Paramètres du DoT
    private DotType _dotType;
    private float _damagePerTick;
    private float _tickInterval;
    private float _dotDuration;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = _direction * speed;
    }

    public void Initialize(Vector2 direction, CharacterStats ownerStats,
        DotType dotType, float damagePerTick, float tickInterval, float dotDuration)
    {
        _direction = direction.normalized;
        _ownerStats = ownerStats;
        _dotType = dotType;
        _damagePerTick = damagePerTick;
        _tickInterval = tickInterval;
        _dotDuration = dotDuration;

        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == _ownerStats?.gameObject) return;

        DotSystem dotSystem = other.GetComponent<DotSystem>();
        if (dotSystem != null)
        {
            DotEffect dot = new DotEffect(
                _dotType,
                _damagePerTick,
                _tickInterval,
                _dotDuration,
                _ownerStats
            );

            dotSystem.ApplyDot(dot);
            Destroy(gameObject);
        }
    }
}