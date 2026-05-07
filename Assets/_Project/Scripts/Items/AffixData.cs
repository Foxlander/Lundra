using UnityEngine;

[CreateAssetMenu(fileName = "NewAffix", menuName = "Items/Affix")]
public class AffixData : ScriptableObject
{
    [Header("Infos")]
    public string affixName = "Affix";
    [TextArea] public string description = "";

    [Header("Effet")]
    public StatType statType;
    public float minValue;
    public float maxValue;
    public ModifierType modifierType = ModifierType.PercentAdd;
}