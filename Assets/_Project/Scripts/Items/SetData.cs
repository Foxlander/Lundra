using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewSet", menuName = "Items/Set")]
public class SetData : ScriptableObject
{
    [Header("Infos")]
    public string setName = "Set";
    [TextArea] public string description = "";

    [Header("Bonus de set")]
    public List<SetBonus> bonuses = new List<SetBonus>();
}

[System.Serializable]
public class SetBonus
{
    public int requiredPieces;
    [TextArea] public string description = "";

    [Header("Stat bonus (optionnel)")]
    public bool hasStat = false;
    public StatType statType;
    public float value;
    public ModifierType modifierType = ModifierType.Flat;

    [Header("Affixe spécial (optionnel)")]
    public bool hasAffix = false;
    public AffixData affix;
}