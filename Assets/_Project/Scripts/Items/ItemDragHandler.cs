using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragHandler : MonoBehaviour, 
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Image iconImage;

    private Canvas _canvas;
    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;
    private Transform _originalParent;
    private Vector2 _originalPosition;

    public ItemData Item { get; private set; }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvas = GetComponentInParent<Canvas>();
        iconImage = GetComponent<Image>();
    }

    public void Init(ItemData item)
    {
        Item = item;

        if (iconImage != null && item.icon != null)
            iconImage.sprite = item.icon;

        // Couleur selon rareté
        iconImage.color = GetRarityColor(item.rarity);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag appelé !");
        
        // Cherche le canvas si pas encore trouvé
        if (_canvas == null)
            _canvas = GetComponentInParent<Canvas>();
        
        if (_canvas == null)
        {
            _canvas = FindFirstObjectByType<Canvas>();
        }

        _originalParent = transform.parent;
        _originalPosition = _rectTransform.anchoredPosition;

        transform.SetParent(_canvas.transform);
        transform.SetAsLastSibling();

        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = 0.8f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1f;

        // Si pas déposé sur un slot valide → retour position originale
        transform.SetParent(_originalParent);
        _rectTransform.anchoredPosition = _originalPosition;
    }

    public void SetOriginalParent(Transform parent)
    {
        _originalParent = parent;
    }

    private Color GetRarityColor(ItemRarity rarity)
    {
        return rarity switch
        {
            ItemRarity.Common    => Color.white,
            ItemRarity.Magic     => new Color(0.4f, 0.7f, 1f),
            ItemRarity.Rare      => new Color(1f, 0.8f, 0.2f),
            ItemRarity.Epic      => new Color(0.7f, 0.3f, 1f),
            ItemRarity.Legendary => new Color(1f, 0.5f, 0f),
            ItemRarity.Unique    => new Color(0.8f, 0.1f, 0.1f),
            _ => Color.white
        };
    }
}