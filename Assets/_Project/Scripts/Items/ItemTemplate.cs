using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewItemTemplate", menuName = "Items/Template")]
public class ItemTemplate : ScriptableObject
{
    [Header("Infos de base")]
    public string baseName = "Item";
    public EquipmentSlot slot = EquipmentSlot.MainHand;
    public HandType handType = HandType.OneHanded;
    public Sprite[] icons;

    [Header("Type d'arme (MainHand seulement)")]
    public WeaponType weaponType = WeaponType.None;

    [Header("Stat de base — Armes (MainHand/OffHand/TwoHanded)")]
    public float minDamageMin = 5f;
    public float minDamageMax = 15f;
    public float maxDamageMin = 10f;
    public float maxDamageMax = 25f;

    [Header("Stat de base — Armures (Head/Shoulders/Chest/Hands/Legs/Boots)")]
    public float armorMin = 5f;
    public float armorMax = 20f;

    [Header("Stats bonus disponibles")]
    public List<TemplateStat> possibleStats = new List<TemplateStat>();

    [Header("Affixes disponibles (Légendaire)")]
    public List<AffixData> possibleAffixes = new List<AffixData>();

    [Header("Set (Unique seulement)")]
    public SetData setData;

    [Header("Scaling par niveau")]
    public float scalingPerLevel = 0.15f;

    // Vérifie si c'est une arme
    public bool IsWeapon => slot == EquipmentSlot.MainHand ||
                            slot == EquipmentSlot.OffHand;

    // Vérifie si c'est une armure
    public bool IsArmor => slot == EquipmentSlot.Head ||
                           slot == EquipmentSlot.Shoulders ||
                           slot == EquipmentSlot.Chest ||
                           slot == EquipmentSlot.Hands ||
                           slot == EquipmentSlot.Legs ||
                           slot == EquipmentSlot.Boots;

    // Vérifie si c'est un accessoire
    public bool IsAccessory => slot == EquipmentSlot.Ring1 ||
                               slot == EquipmentSlot.Ring2 ||
                               slot == EquipmentSlot.Amulet;
}

[System.Serializable]
public class TemplateStat
{
    public StatType statType;
    public float minValue = 3f;
    public float maxValue = 10f;
    public ModifierType modifierType = ModifierType.Flat;
}