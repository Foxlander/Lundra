using UnityEngine;

public class YSorting : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float offset = 0f; // offset depuis le bas du sprite

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        if (spriteRenderer == null) return;

        // Prend le bas du sprite comme référence
        float bottomY = transform.position.y - (spriteRenderer.bounds.extents.y) + offset;
        
        // Convertit la position Y en Sorting Order
        // Multiplie par -100 pour que plus bas = valeur plus haute
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-bottomY * 100);
    }
}