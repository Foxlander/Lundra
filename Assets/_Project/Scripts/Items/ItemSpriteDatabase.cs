using UnityEngine;

[CreateAssetMenu(fileName = "ItemSpriteDatabase", menuName = "Items/Sprite Database")]
public class ItemSpriteDatabase : ScriptableObject
{
    public static ItemSpriteDatabase Instance { get; set; }

    [Header("Armes")]
    public Sprite sword;
    public Sprite bow;
    public Sprite staff;
    public Sprite shield;

    [Header("Armures")]
    public Sprite helmet;
    public Sprite shoulders;
    public Sprite chest;
    public Sprite gloves;
    public Sprite legs;
    public Sprite boots;

    [Header("Accessoires")]
    public Sprite ring;
    public Sprite amulet;

    private void OnEnable()
    {
        Instance = this;
    }

    public Sprite GetSpriteForSlot(EquipmentSlot slot, HandType handType = HandType.OneHanded)
    {
        return slot switch
        {
            EquipmentSlot.MainHand => sword,
            EquipmentSlot.OffHand  => shield,
            EquipmentSlot.Head     => helmet,
            EquipmentSlot.Shoulders => shoulders,
            EquipmentSlot.Chest    => chest,
            EquipmentSlot.Hands    => gloves,
            EquipmentSlot.Legs     => legs,
            EquipmentSlot.Boots    => boots,
            EquipmentSlot.Ring1    => ring,
            EquipmentSlot.Ring2    => ring,
            EquipmentSlot.Amulet   => amulet,
            _ => null
        };
    }

    public Sprite GetSpriteForTemplate(ItemTemplate template)
    {
        if (template.IsWeapon && template.slot == EquipmentSlot.MainHand)
        {
            return template.weaponType switch
            {
                WeaponType.Sword => sword,
                WeaponType.Bow   => bow,
                WeaponType.Staff => staff,
                _                => sword
            };
        }

        return GetSpriteForSlot(template.slot);
    }
}