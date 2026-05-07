using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Tooltip UI")]
    [SerializeField] private GameObject tooltipPanel;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI rarityText;
    [SerializeField] private TextMeshProUGUI statsText;
    [SerializeField] private TextMeshProUGUI affixText;

    private ItemData _item;

    // Couleurs rareté
    private static readonly Color[] RarityColors = {
        Color.white,
        new Color(0.4f, 0.7f, 1f),
        new Color(1f, 0.8f, 0.2f),
        new Color(0.7f, 0.3f, 1f),
        new Color(1f, 0.5f, 0f),
        new Color(0.8f, 0.1f, 0.1f),
    };

    public void Init(ItemData item)
    {
        _item = item;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_item == null || tooltipPanel == null) return;

        tooltipPanel.SetActive(true);

        // Nom
        if (itemNameText != null)
        {
            itemNameText.text = _item.itemName;
            itemNameText.color = RarityColors[(int)_item.rarity];
        }

        // Rareté
        if (rarityText != null)
            rarityText.text = _item.rarity.ToString();

        // Stats
        if (statsText != null)
        {
            string stats = "";
            foreach (ItemStat stat in _item.stats)
                stats += $"+{stat.value} {stat.statType}\n";
            statsText.text = stats;
        }

        // Affixe
        if (affixText != null)
        {
            if (!string.IsNullOrEmpty(_item.affixName))
                affixText.text = $"{_item.affixName}\n{_item.affixDescription}";
            else
                affixText.text = "";
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltipPanel != null)
            tooltipPanel.SetActive(false);
    }
}