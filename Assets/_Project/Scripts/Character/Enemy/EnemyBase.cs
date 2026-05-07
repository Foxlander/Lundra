using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class EnemyBase : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string enemyName = "Enemy";
    [SerializeField] private int experienceReward = 10;
    [SerializeField] private float detectionRange = 8f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1f;

    [Header("Loot")]
    [SerializeField] private ItemTemplate[] possibleTemplates;
    [SerializeField] private float dropChance = 0.3f;
    [SerializeField] private GameObject itemDropPrefab;
    [SerializeField] private int enemyLevel = 1;

    // Composants
    protected CharacterStats Stats;
    protected Transform Player;
    protected Rigidbody2D Rb;
    protected EnemyAnimator EnemyAnimator;

    // État
    private float _attackTimer;
    private EnemyState _currentState;

    private enum EnemyState
    {
        Idle,
        Chase,
        Attack,
        Dead
    }

    protected virtual void Awake()
    {
        Stats = GetComponent<CharacterStats>();
        Rb = GetComponent<Rigidbody2D>();
        EnemyAnimator = GetComponent<EnemyAnimator>();
    }

    protected virtual void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            Player = playerObj.transform;

        SetState(EnemyState.Idle);
    }

    protected virtual void Update()
    {
        if (_currentState == EnemyState.Dead) return;
        if (Player == null) return;

        _attackTimer -= Time.deltaTime;

        float distanceToPlayer = Vector2.Distance(transform.position, Player.position);

        if (distanceToPlayer <= attackRange)
            SetState(EnemyState.Attack);
        else if (distanceToPlayer <= detectionRange)
            SetState(EnemyState.Chase);
        else
            SetState(EnemyState.Idle);

        switch (_currentState)
        {
            case EnemyState.Idle:
                HandleIdle();
                break;
            case EnemyState.Chase:
                HandleChase();
                break;
            case EnemyState.Attack:
                HandleAttack();
                break;
        }
    }

    private void SetState(EnemyState newState)
    {
        if (_currentState == newState) return;
        _currentState = newState;
    }

    protected virtual void HandleIdle()
    {
        Rb.linearVelocity = Vector2.zero;
    }

    protected virtual void HandleChase()
    {
        if (Player == null) return;

        Vector2 direction = (Player.position - transform.position).normalized;
        float speed = Stats.GetStatValue(StatType.MoveSpeed);
        Rb.linearVelocity = direction * speed;
    }

    protected virtual void HandleAttack()
    {
        Rb.linearVelocity = Vector2.zero;

        if (_attackTimer <= 0f)
        {
            PerformAttack();
            _attackTimer = attackCooldown;

            // Déclenche l'animation d'attaque
            if (EnemyAnimator != null)
                EnemyAnimator.TriggerAttack();
        }
    }

    protected virtual void PerformAttack()
    {
        CharacterStats playerStats = Player.GetComponent<CharacterStats>();
        if (playerStats != null)
            DamageSystem.ApplyDamage(Stats, playerStats);
    }

    public virtual void OnDeath()
    {
        SetState(EnemyState.Dead);
        Rb.linearVelocity = Vector2.zero;

        // Déclenche l'animation de mort
        if (EnemyAnimator != null)
            EnemyAnimator.TriggerDeath();

        DropLoot();

        Debug.Log($"{enemyName} died ! Rewarding {experienceReward} XP");

        Destroy(gameObject, 1.5f); // ← augmenté pour laisser l'animation jouer
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void DropLoot()
    {
        if (possibleTemplates.Length == 0) return;
        if (Random.value > dropChance) return;

        ItemTemplate template = possibleTemplates[Random.Range(0, possibleTemplates.Length)];
        ItemRarity rarity = RollRarity();
        ItemData generatedItem = ItemGenerator.Generate(template, rarity, enemyLevel, 1);

        GameObject go = Instantiate(itemDropPrefab, transform.position, Quaternion.identity);
        go.GetComponent<ItemDrop>().Init(generatedItem);
    }

    private ItemRarity RollRarity()
    {
        float roll = Random.value;

        if (roll < 0.01f) return ItemRarity.Unique;
        if (roll < 0.05f) return ItemRarity.Legendary;
        if (roll < 0.15f) return ItemRarity.Epic;
        if (roll < 0.35f) return ItemRarity.Rare;
        if (roll < 0.60f) return ItemRarity.Magic;
        return ItemRarity.Common;
    }
}