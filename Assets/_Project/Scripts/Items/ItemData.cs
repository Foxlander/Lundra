using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/Equipment")]
public class ItemData : ScriptableObject
{
    [Header("Infos")]
    public string itemName = "Item";
    public Sprite icon;
    public ItemRarity rarity = ItemRarity.Common;
    public EquipmentSlot slot = EquipmentSlot.MainHand;

    [Header("Stats")]
    public List<ItemStat> stats = new List<ItemStat>();

    [Header("Affix (Légendaire)")]
    public string affixName = "";
    public string affixDescription = "";

    [Header("Set (Unique)")]
    public SetData setData;

    [Header("Type de main")]
    public HandType handType = HandType.OneHanded;
}

[System.Serializable]
public class ItemStat
{
    public StatType statType;
    public float value;
    public ModifierType modifierType = ModifierType.Flat;
}
