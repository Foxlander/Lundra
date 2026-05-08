using UnityEngine;
using System.Collections.Generic;

public static class ItemGenerator
{
    // Nombre de stats total par rareté (stat de base incluse)
    private static readonly int[] StatCountByRarity = { 1, 2, 3, 4, 5, 5 };

    // Multiplicateurs de valeurs par rareté
    private static readonly float[] RarityMultiplier = { 1f, 1.2f, 1.5f, 2f, 2.5f, 3f };

    // Stats réservées à la stat de base — jamais en bonus
    private static readonly StatType[] BlacklistedBonusStats = {
        StatType.MinDamage,
        StatType.MaxDamage,
        StatType.Armor,
    };

    public static ItemData Generate(ItemTemplate template, ItemRarity rarity, int enemyLevel, int playerLevel)
    {
        ItemData item = ScriptableObject.CreateInstance<ItemData>();

        int level = (enemyLevel + playerLevel) / 2;
        float levelScale = 1f + (template.scalingPerLevel * level);
        float rarityMult = RarityMultiplier[(int)rarity];

        // ── Nom / Infos ───────────────────────────────────────
        item.itemName = $"{GetRarityPrefix(rarity)} {template.baseName}".Trim();
        item.rarity = rarity;
        item.slot = template.slot;
        item.handType = template.handType;

        // Sprite depuis la database
        if (ItemSpriteDatabase.Instance != null)
            item.icon = ItemSpriteDatabase.Instance.GetSpriteForTemplate(template);

        // ── Stat de base ──────────────────────────────────────
        if (template.IsWeapon)
        {
            // MinDamage
            float minDmg = Random.Range(template.minDamageMin, template.minDamageMax)
                           * levelScale * rarityMult;
            item.stats.Add(new ItemStat
            {
                statType = StatType.MinDamage,
                value = Mathf.Round(minDmg),
                modifierType = ModifierType.Flat
            });

            // MaxDamage
            float maxDmg = Random.Range(template.maxDamageMin, template.maxDamageMax)
                           * levelScale * rarityMult;
            item.stats.Add(new ItemStat
            {
                statType = StatType.MaxDamage,
                value = Mathf.Round(maxDmg),
                modifierType = ModifierType.Flat
            });
        }
        else if (template.IsArmor)
        {
            float armor = Random.Range(template.armorMin, template.armorMax)
                          * levelScale * rarityMult;
            item.stats.Add(new ItemStat
            {
                statType = StatType.Armor,
                value = Mathf.Round(armor),
                modifierType = ModifierType.Flat
            });
        }
        // Accessoires → pas de stat de base, que des stats bonus

        // ── Stats bonus ───────────────────────────────────────
        int baseStatCount = template.IsWeapon ? 2 : template.IsArmor ? 1 : 0;
        int bonusStatCount = StatCountByRarity[(int)rarity] - baseStatCount;

        List<TemplateStat> availableStats = new List<TemplateStat>(template.possibleStats);
        availableStats.RemoveAll(s => System.Array.IndexOf(BlacklistedBonusStats, s.statType) >= 0);

        for (int i = 0; i < bonusStatCount && availableStats.Count > 0; i++)
        {
            int index = Random.Range(0, availableStats.Count);
            TemplateStat chosenStat = availableStats[index];
            availableStats.RemoveAt(index);

            float statValue = Random.Range(chosenStat.minValue, chosenStat.maxValue) 
                            * levelScale * rarityMult;

            item.stats.Add(new ItemStat
            {
                statType = chosenStat.statType,
                value = Mathf.Round(statValue),
                modifierType = chosenStat.modifierType
            });
        }

        // ── Affix Légendaire ──────────────────────────────────
        if (rarity == ItemRarity.Legendary && template.possibleAffixes.Count > 0)
        {
            AffixData affix = template.possibleAffixes[
                Random.Range(0, template.possibleAffixes.Count)];

            float affixValue = Random.Range(affix.minValue, affix.maxValue);

            item.stats.Add(new ItemStat
            {
                statType = affix.statType,
                value = Mathf.Round(affixValue * 100f) / 100f,
                modifierType = affix.modifierType
            });

            item.affixName = affix.affixName;
            item.affixDescription = affix.description;
        }

        // ── Set Unique ────────────────────────────────────────
        if (rarity == ItemRarity.Unique && template.setData != null)
            item.setData = template.setData;

        // ── Debug ─────────────────────────────────────────────
        string statsLog = $"Item généré : {item.itemName} ({item.rarity})\n";
        foreach (ItemStat stat in item.stats)
            statsLog += $"  → {stat.statType} : {stat.value} ({stat.modifierType})\n";
        Debug.Log(statsLog);

        return item;
    }

    private static string GetRarityPrefix(ItemRarity rarity)
    {
        return rarity switch
        {
            ItemRarity.Common    => "",
            ItemRarity.Magic     => "Enchanté",
            ItemRarity.Rare      => "Précieux",
            ItemRarity.Epic      => "Épique",
            ItemRarity.Legendary => "Légendaire",
            ItemRarity.Unique    => "Unique",
            _ => ""
        };
    }
}