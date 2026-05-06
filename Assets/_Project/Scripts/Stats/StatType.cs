public enum StatType
{
    // ── OFFENSIF ────────────────────────────────
    MinDamage,
    MaxDamage,
    CritChance,
    CritMultiplier,
    AttackSpeed,

    // Élémentaire
    FireDamage,
    LightningDamage,
    IceDamage,
    PoisonDamage,
    PhysicalDamage,

    // DoT
    BleedDamage,
    BurnDamage,
    PoisonDuration,
    BleedDuration,
    BurnDuration,
    DoTChance,

    // ── DÉFENSIF ────────────────────────────────
    MaxHealth,
    HealthRegen,
    Armor,
    DodgeChance,
    BlockChance,
    Thorns,
    LifeSteal,
    DamageReduction,

    // ── MANA / SKILLS ───────────────────────────
    MaxMana,
    ManaRegen,
    ManaCostReduction,
    CooldownReduction,
    SkillDamage,
    ProjectileSpeed,
    ProjectileCount,
    AreaOfEffect,

    // ── MOBILITÉ ────────────────────────────────
    MoveSpeed,
    DashDistance,
    DashCooldown,

    // ── UTILITAIRE ──────────────────────────────
    LuckChance,
    GoldFind,
    ExperienceGain,
    ItemFind,
}