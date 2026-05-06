using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] private float baseMaxHealth = 100f;
    [SerializeField] private float baseHealthRegen = 1f;
    [SerializeField] private float baseMinDamage = 5f;
    [SerializeField] private float baseMaxDamage = 10f;
    [SerializeField] private float baseCritChance = 0.05f;
    [SerializeField] private float baseCritMultiplier = 1.5f;
    [SerializeField] private float baseAttackSpeed = 1f;
    [SerializeField] private float baseMoveSpeed = 5f;
    [SerializeField] private float baseArmor = 0f;
    [SerializeField] private float baseMaxMana = 100f;
    [SerializeField] private float baseManaRegen = 2f;

    // Dictionnaire de stats
    private Dictionary<StatType, Stat> _stats;

    // Valeurs courantes
    public float CurrentHealth { get; private set; }
    public float CurrentMana { get; private set; }

    private void Awake()
    {
        InitializeStats();
        CurrentHealth = GetStatValue(StatType.MaxHealth);
        CurrentMana = GetStatValue(StatType.MaxMana);
    }

    private void InitializeStats()
    {
        _stats = new Dictionary<StatType, Stat>
        {
            { StatType.MaxHealth,       new Stat(baseMaxHealth) },
            { StatType.HealthRegen,     new Stat(baseHealthRegen) },
            { StatType.MinDamage,       new Stat(baseMinDamage) },
            { StatType.MaxDamage,       new Stat(baseMaxDamage) },
            { StatType.CritChance,      new Stat(baseCritChance) },
            { StatType.CritMultiplier,  new Stat(baseCritMultiplier) },
            { StatType.AttackSpeed,     new Stat(baseAttackSpeed) },
            { StatType.MoveSpeed,       new Stat(baseMoveSpeed) },
            { StatType.Armor,           new Stat(baseArmor) },
            { StatType.MaxMana,         new Stat(baseMaxMana) },
            { StatType.ManaRegen,       new Stat(baseManaRegen) },
        };
    }

    private void Update()
    {
        // Régénération HP
        float hpRegen = GetStatValue(StatType.HealthRegen);
        if (hpRegen > 0 && CurrentHealth < GetStatValue(StatType.MaxHealth))
            Heal(hpRegen * Time.deltaTime);

        // Régénération Mana
        float manaRegen = GetStatValue(StatType.ManaRegen);
        if (manaRegen > 0 && CurrentMana < GetStatValue(StatType.MaxMana))
            RestoreMana(manaRegen * Time.deltaTime);
    }

    // Récupérer la valeur d'une stat
    public float GetStatValue(StatType type)
    {
        if (_stats.TryGetValue(type, out Stat stat))
            return stat.GetValue();

        Debug.LogWarning($"Stat {type} not found on {gameObject.name}");
        return 0f;
    }

    // Ajouter un modificateur
    public void AddModifier(StatType type, StatModifier modifier)
    {
        if (_stats.TryGetValue(type, out Stat stat))
            stat.AddModifier(modifier);
    }

    // Retirer un modificateur
    public void RemoveModifier(StatType type, StatModifier modifier)
    {
        if (_stats.TryGetValue(type, out Stat stat))
            stat.RemoveModifier(modifier);
    }

    // Retirer tous les modificateurs d'une source
    public void RemoveAllModifiersFromSource(object source)
    {
        foreach (var stat in _stats.Values)
            stat.RemoveAllModifiersFromSource(source);
    }

    // Gestion de la vie
    public void TakeDamage(DamageInfo damageInfo)
    {
        float finalDamage = damageInfo.Amount;

        // Le dégât Pure ignore l'armure
        if (damageInfo.Type != DamageType.Pure)
        {
            float armor = GetStatValue(StatType.Armor);
            finalDamage = Mathf.Max(1, finalDamage - armor);
        }

        CurrentHealth = Mathf.Clamp(
            CurrentHealth - finalDamage,
            0,
            GetStatValue(StatType.MaxHealth)
        );

        Debug.Log($"{gameObject.name} took {finalDamage:F1} " +
                $"{damageInfo.Type} damage " +
                $"{(damageInfo.IsCrit ? "<CRIT>" : "")}. " +
                $"HP: {CurrentHealth:F1}");

        if (CurrentHealth <= 0)
            OnDeath();
    }

    // Garde l'ancienne pour compatibilité
    public void TakeDamage(float damage)
    {
        TakeDamage(new DamageInfo(damage, false, DamageType.Physical, null));
    }

    public void Heal(float amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, GetStatValue(StatType.MaxHealth));
    }

    public void UseMana(float amount)
    {
        CurrentMana = Mathf.Clamp(CurrentMana - amount, 0, GetStatValue(StatType.MaxMana));
    }

    public void RestoreMana(float amount)
    {
        CurrentMana = Mathf.Clamp(CurrentMana + amount, 0, GetStatValue(StatType.MaxMana));
    }

    public bool HasEnoughMana(float amount) => CurrentMana >= amount;

    protected virtual void OnDeath()
    {
        // Vérifie si c'est un ennemi
        EnemyBase enemy = GetComponent<EnemyBase>();
        if (enemy != null)
        {
            enemy.OnDeath();
            return;
        }

        // Vérifie si c'est le joueur
        PlayerDeath playerDeath = GetComponent<PlayerDeath>();
        if (playerDeath != null)
        {
            playerDeath.OnPlayerDeath();
            return;
        }

        Debug.Log($"{gameObject.name} is dead !");
    }

    // Sync MoveSpeed avec PlayerController
    public void SyncMoveSpeed(PlayerController controller)
    {
        // On le fera plus tard quand on liera les stats au controller
    }

    public void FullHeal()
    {
        CurrentHealth = GetStatValue(StatType.MaxHealth);
    }

    public void FullMana()
    {
        CurrentMana = GetStatValue(StatType.MaxMana);
    }
}