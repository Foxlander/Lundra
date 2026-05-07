using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IDropHandler
{
    [SerializeField] private Image background;

    private ItemDragHandler _currentItem;
    private Inventory _inventory;
    private EquipmentManager _equipmentManager;

    public void Init(Inventory inventory, EquipmentManager equipmentManager)
    {
        _inventory = inventory;
        _equipmentManager = equipmentManager;
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
    public ItemDragHandler CurrentItem => _currentItem;

    public void OnDrop(PointerEventData eventData)
    {
        ItemDragHandler draggedItem = eventData.pointerDrag?.GetComponent<ItemDragHandler>();
        if (draggedItem == null) return;

        // Slot vide → dépose l'item
        if (IsEmpty)
        {
            // Retire de l'équipement si venait d'un slot équipement
        EquipmentSlotUI fromEquipSlot = draggedItem.transform.parent
                                            .GetComponent<EquipmentSlotUI>();
            if (fromEquipSlot != null)
            {
                _equipmentManager.Unequip(fromEquipSlot.Slot);
                fromEquipSlot.ClearSlot();
            }

            SetItem(draggedItem);
        }
    }
}