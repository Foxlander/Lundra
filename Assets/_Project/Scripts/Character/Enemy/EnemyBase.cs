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
    }

    protected virtual void Start()
    {
        // Trouve le joueur au démarrage
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

        // Machine à états simple
        if (distanceToPlayer <= attackRange)
            SetState(EnemyState.Attack);
        else if (distanceToPlayer <= detectionRange)
            SetState(EnemyState.Chase);
        else
            SetState(EnemyState.Idle);

        // Exécute l'état courant
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

        // Flip sprite selon direction
        if (direction.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

    protected virtual void HandleAttack()
    {
        Rb.linearVelocity = Vector2.zero;

        if (_attackTimer <= 0f)
        {
            PerformAttack();
            _attackTimer = attackCooldown;
        }
    }

    protected virtual void PerformAttack()
    {
        CharacterStats playerStats = Player.GetComponent<CharacterStats>();
        if (playerStats != null)
            DamageSystem.ApplyDamage(Stats, playerStats);
    }

    // Appelé depuis CharacterStats quand HP <= 0
    public virtual void OnDeath()
    {
        SetState(EnemyState.Dead);
        Rb.linearVelocity = Vector2.zero;

        DropLoot();

        Debug.Log($"{enemyName} died ! Rewarding {experienceReward} XP");

        // Plus tard : drop loot, spawn VFX, etc.
        Destroy(gameObject, 0.5f);
    }

    // Visualise les ranges dans l'éditeur
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

        // Choisit un template aléatoire
        ItemTemplate template = possibleTemplates[
            Random.Range(0, possibleTemplates.Length)];

        // Détermine la rareté aléatoirement
        ItemRarity rarity = RollRarity();

        // Génère l'item
        ItemData generatedItem = ItemGenerator.Generate(template, rarity, enemyLevel, 1);

        // Spawn au sol
        GameObject go = Instantiate(itemDropPrefab, transform.position, Quaternion.identity);
        go.GetComponent<ItemDrop>().Init(generatedItem);
    }
    
    private ItemRarity RollRarity()
    {
        float roll = Random.value; // nombre entre 0 et 1

        if (roll < 0.01f) return ItemRarity.Unique;      // 1%
        if (roll < 0.05f) return ItemRarity.Legendary;   // 4%
        if (roll < 0.15f) return ItemRarity.Epic;         // 10%
        if (roll < 0.35f) return ItemRarity.Rare;         // 20%
        if (roll < 0.60f) return ItemRarity.Magic;        // 25%
        return ItemRarity.Common;                          // 40%
    }

}