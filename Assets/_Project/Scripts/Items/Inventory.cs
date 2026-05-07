using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int maxSlots = 48;

    // Tableau fixe — null = slot vide
    private ItemData[] _slots;
    private EquipmentManager _equipmentManager;

    // Compatibilité avec l'ancien code
    public List<ItemData> Items
    {
        get
        {
            List<ItemData> items = new List<ItemData>();
            foreach (ItemData item in _slots)
                if (item != null) items.Add(item);
            return items;
        }
    }

    public int MaxSlots => maxSlots;

    private void Awake()
    {
        _slots = new ItemData[maxSlots];
        _equipmentManager = GetComponent<EquipmentManager>();
    }

    // Ajoute dans le premier slot libre
    public bool AddItem(ItemData item)
    {
        for (int i = 0; i < maxSlots; i++)
        {
            if (_slots[i] == null)
            {
                _slots[i] = item;
                Debug.Log($"Ramassé : {item.itemName} ({item.rarity}) → slot {i}");
                return true;
            }
        }
        Debug.Log("Inventaire plein !");
        return false;
    }

    // Ajoute à un index précis
    public bool AddItemAtIndex(ItemData item, int index)
    {
        if (index < 0 || index >= maxSlots) return false;
        if (_slots[index] != null) return false; // slot occupé
        _slots[index] = item;
        return true;
    }

    // Retire un item
    public void RemoveItem(ItemData item)
    {
        for (int i = 0; i < maxSlots; i++)
        {
            if (_slots[i] == item)
            {
                _slots[i] = null;
                return;
            }
        }
    }

    // Retire à un index précis
    public void RemoveItemAtIndex(int index)
    {
        if (index >= 0 && index < maxSlots)
            _slots[index] = null;
    }

    // Récupère l'item à un index
    public ItemData GetItemAtIndex(int index)
    {
        if (index < 0 || index >= maxSlots) return null;
        return _slots[index];
    }

    // Retourne l'index d'un item
    public int GetItemIndex(ItemData item)
    {
        for (int i = 0; i < maxSlots; i++)
            if (_slots[i] == item) return i;
        return -1;
    }

    public void EquipItem(ItemData item)
    {
        if (_equipmentManager != null)
            _equipmentManager.Equip(item);
    }
}