using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class EquipmentSlotUI : MonoBehaviour, IDropHandler
{
    [SerializeField] private Image slotIcon;
    [SerializeField] private TextMeshProUGUI slotLabel;
    [SerializeField] private Image itemIcon;

    public EquipmentSlot Slot { get; private set; }

    private ItemDragHandler _currentItem;
    private EquipmentManager _equipmentManager;
    private Inventory _inventory;

    public void Init(EquipmentSlot slot, EquipmentManager equipmentManager, Inventory inventory)
    {
        Slot = slot;
        _equipmentManager = equipmentManager;
        _inventory = inventory;

        if (slotLabel != null)
            slotLabel.text = GetSlotLabel(slot);
    }

    public void SetItem(ItemDragHandler item)
    {
        _currentItem = item;
        item.transform.SetParent(transform);
        item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        item.SetOriginalParent(transform);
    }

    public void ClearSlot()
    {
        _currentItem = null;
    }

    public bool IsEmpty => _currentItem == null;

    public void OnDrop(PointerEventData eventData)
    {
        ItemDragHandler draggedItem = eventData.pointerDrag?.GetComponent<ItemDragHandler>();
        if (draggedItem == null) return;
        if (draggedItem.Item == null) return;

        // Vérifie que l'item correspond au slot
        if (!IsValidForSlot(draggedItem.Item)) return;

        // Déséquipe l'item actuel si présent
        if (!IsEmpty)
        {
            InventorySlotUI freeSlot = FindFreeInventorySlot();
            if (freeSlot != null)
            {
                freeSlot.SetItem(_currentItem);
                _equipmentManager.Unequip(Slot);
                ClearSlot();
            }
            else return; // inventaire plein
        }

        // Retire du slot inventaire précédent
        InventorySlotUI fromSlot = draggedItem.transform.parent
                                        .GetComponent<InventorySlotUI>();
        if (fromSlot != null)
            fromSlot.ClearSlot();

        // Équipe
        SetItem(draggedItem);
        _equipmentManager.Equip(draggedItem.Item);
    }

    private bool IsValidForSlot(ItemData item)
    {
        // Vérifie que le slot correspond
        if (item.slot != Slot)
        {
            // Ring1 et Ring2 acceptent tous les deux les anneaux
            if (Slot == EquipmentSlot.Ring2 && item.slot == EquipmentSlot.Ring1)
                return true;
            return false;
        }
        return true;
    }

    private InventorySlotUI FindFreeInventorySlot()
    {
        InventorySlotUI[] slots = FindObjectsByType<InventorySlotUI>(FindObjectsSortMode.None);
        foreach (InventorySlotUI slot in slots)
            if (slot.IsEmpty) return slot;
        return null;
    }

    private string GetSlotLabel(EquipmentSlot slot)
    {
        return slot switch
        {
            EquipmentSlot.MainHand  => "Main",
            EquipmentSlot.OffHand   => "Secondaire",
            EquipmentSlot.Head      => "Tête",
            EquipmentSlot.Shoulders => "Épaules",
            EquipmentSlot.Chest     => "Torse",
            EquipmentSlot.Hands     => "Gants",
            EquipmentSlot.Legs      => "Jambières",
            EquipmentSlot.Boots     => "Bottes",
            EquipmentSlot.Ring1     => "Anneau 1",
            EquipmentSlot.Ring2     => "Anneau 2",
            EquipmentSlot.Amulet    => "Amulette",
            _ => ""
        };
    }
}