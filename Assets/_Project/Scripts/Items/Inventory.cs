using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int maxSlots = 20;

    private List<ItemData> _items = new List<ItemData>();
    private EquipmentManager _equipmentManager;

    public List<ItemData> Items => _items;

    private void Awake()
    {
        _equipmentManager = GetComponent<EquipmentManager>();
    }

    public bool AddItem(ItemData item)
    {
        if (_items.Count >= maxSlots)
        {
            Debug.Log("Inventaire plein !");
            return false;
        }

        _items.Add(item);
        Debug.Log($"Ramassé : {item.itemName} ({item.rarity})");
        return true;
    }

    public void RemoveItem(ItemData item)
    {
        _items.Remove(item);
    }

    public void EquipItem(ItemData item)
    {
        if (_equipmentManager != null)
            _equipmentManager.Equip(item);
    }
}