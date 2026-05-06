using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    private Dictionary<EquipmentSlot, ItemData> _equipped 
        = new Dictionary<EquipmentSlot, ItemData>();

    private CharacterStats _stats;
    private Inventory _inventory;

    private void Awake()
    {
        _stats = GetComponent<CharacterStats>();
        _inventory = GetComponent<Inventory>();
    }

    public void Equip(ItemData item)
    {
        // Déséquipe l'item actuel si il y en a un
        if (_equipped.TryGetValue(item.slot, out ItemData current))
            Unequip(item.slot);

        // Équipe le nouvel item
        _equipped[item.slot] = item;
        ApplyStats(item, true);

        Debug.Log($"Équipé : {item.itemName}");
    }

    public void Unequip(EquipmentSlot slot)
    {
        if (!_equipped.TryGetValue(slot, out ItemData item)) return;

        ApplyStats(item, false);
        _inventory.AddItem(item);
        _equipped.Remove(slot);

        Debug.Log($"Déséquipé : {item.itemName}");
    }

    private void ApplyStats(ItemData item, bool apply)
    {
        foreach (ItemStat itemStat in item.stats)
        {
            StatModifier modifier = new StatModifier(
                itemStat.value,
                itemStat.modifierType,
                item
            );

            if (apply)
                _stats.AddModifier(itemStat.statType, modifier);
            else
                _stats.RemoveModifier(itemStat.statType, modifier);
        }
    }

    public ItemData GetEquipped(EquipmentSlot slot)
    {
        _equipped.TryGetValue(slot, out ItemData item);
        return item;
    }
}