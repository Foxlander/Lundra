using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }

    [Header("Références")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Transform gridParent;
    [SerializeField] private Transform equipmentSlotsParent;

    [Header("Prefabs")]
    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private GameObject itemDragHandlerPrefab;
    [SerializeField] private GameObject equipmentSlotPrefab;

    [Header("Settings")]
    [SerializeField] private int columns = 8;
    [SerializeField] private int rows = 6;

    private Inventory _inventory;
    private EquipmentManager _equipmentManager;
    private List<InventorySlotUI> _slots = new List<InventorySlotUI>();
    private bool _isOpen = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _inventory = player.GetComponent<Inventory>();
            _equipmentManager = player.GetComponent<EquipmentManager>();
        }

        GenerateGrid();
        GenerateEquipmentSlots();

        inventoryPanel.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame)
            ToggleInventory();
    }

    public void ToggleInventory()
    {
        Debug.Log("Toggle appelé !");
        
        if (_inventory == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Debug.Log($"Player trouvé : {player != null}");
            
            if (player != null)
            {
                _inventory = player.GetComponent<Inventory>();
                _equipmentManager = player.GetComponent<EquipmentManager>();
                Debug.Log($"Inventory trouvé : {_inventory != null}");
            }
        }

        if (_inventory == null)
        {
            Debug.LogWarning("Inventory null !");
            return;
        }

        _isOpen = !_isOpen;
        Debug.Log($"IsOpen : {_isOpen}");
        inventoryPanel.SetActive(_isOpen);

        if (_isOpen)
            RefreshInventory();

        Time.timeScale = _isOpen ? 0f : 1f;
    }

    private void GenerateGrid()
    {
        for (int i = 0; i < columns * rows; i++)
        {
            GameObject slotGO = Instantiate(inventorySlotPrefab, gridParent);
            InventorySlotUI slot = slotGO.GetComponent<InventorySlotUI>();
            slot.Init(_inventory, _equipmentManager);
            _slots.Add(slot);
        }
    }

    private void GenerateEquipmentSlots()
    {
        foreach (EquipmentSlot slot in System.Enum.GetValues(typeof(EquipmentSlot)))
        {
            GameObject slotGO = Instantiate(equipmentSlotPrefab, equipmentSlotsParent);
            EquipmentSlotUI slotUI = slotGO.GetComponent<EquipmentSlotUI>();
            slotUI.Init(slot, _equipmentManager, _inventory);
        }
    }

    public void RefreshInventory()
    {
        // Détruit les anciens ItemDragHandlers
        foreach (InventorySlotUI slot in _slots)
        {
            if (slot.CurrentItem != null)
            {
                Destroy(slot.CurrentItem.gameObject);
                slot.ClearSlot();
            }
        }

        // Initialise les slots
        foreach (InventorySlotUI slot in _slots)
            slot.Init(_inventory, _equipmentManager);

        // Place les items selon leur index exact
        for (int i = 0; i < _slots.Count; i++)
        {
            ItemData item = _inventory.GetItemAtIndex(i);
            if (item == null) continue;

            GameObject dragGO = Instantiate(itemDragHandlerPrefab, _slots[i].transform);
            ItemDragHandler drag = dragGO.GetComponent<ItemDragHandler>();
            drag.Init(item);

            ItemTooltip tooltip = dragGO.GetComponent<ItemTooltip>();
            if (tooltip != null) tooltip.Init(item);

            _slots[i].SetItem(drag);
        }

        RefreshEquipmentSlots();
    }

    public void RefreshEquipmentSlots()
    {
        EquipmentSlotUI[] slots = equipmentSlotsParent.GetComponentsInChildren<EquipmentSlotUI>();
        foreach (EquipmentSlotUI slot in slots)
        {
            // Vide le slot visuellement
            if (slot.CurrentItem != null)
            {
                Destroy(slot.CurrentItem.gameObject);
                slot.ClearSlot();
            }

            // Remet l'item équipé si présent
            ItemData equippedItem = _equipmentManager.GetEquipped(slot.Slot);
            if (equippedItem != null)
            {
                GameObject dragGO = Instantiate(itemDragHandlerPrefab, slot.transform);
                ItemDragHandler drag = dragGO.GetComponent<ItemDragHandler>();
                drag.Init(equippedItem);
                slot.SetItem(drag);
            }
        }
    }
}