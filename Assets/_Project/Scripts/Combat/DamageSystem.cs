using UnityEngine;

public static class DamageSystem
{
    public static void ApplyDamage(CharacterStats attacker, CharacterStats target, DamageType damageType = DamageType.Physical)
    {
        float baseDamage = Random.Range(
            attacker.GetStatValue(StatType.MinDamage),
            attacker.GetStatValue(StatType.MaxDamage)
        );

        bool isCrit = IsCriticalHit(attacker);
        if (isCrit)
            baseDamage *= attacker.GetStatValue(StatType.CritMultiplier);

        DamageInfo damageInfo = new DamageInfo(baseDamage, isCrit, damageType, attacker);
        target.TakeDamage(damageInfo);
    }

    // Dégâts directs sans calcul (DoT, environnement, etc.)
    public static void ApplyRawDamage(CharacterStats target, float amount, DamageType damageType = DamageType.Pure)
    {
        DamageInfo damageInfo = new DamageInfo(amount, false, damageType, null);
        target.TakeDamage(damageInfo);
    }

    // Vérification critique
    public static bool IsCriticalHit(CharacterStats attacker)
    {
        float critChance = attacker.GetStatValue(StatType.CritChance);
        return Random.value <= critChance;
    }
}