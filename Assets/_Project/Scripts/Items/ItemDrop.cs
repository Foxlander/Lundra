using UnityEngine;
using UnityEngine.InputSystem;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float pickupRadius = 1f;

    private ItemData _itemData;
    private bool _canPickup = false;

    // Couleurs par rareté
    private static readonly Color[] RarityColors = {
        new Color(1f, 1f, 1f),       // Common — blanc
        new Color(0.2f, 0.5f, 1f),   // Rare — bleu
        new Color(0.6f, 0.2f, 1f),   // Epic — violet
        new Color(1f, 0.6f, 0f),     // Legendary — orange
    };

    public void Init(ItemData data)
    {
        _itemData = data;

        if (spriteRenderer != null && data.icon != null)
        {
            spriteRenderer.sprite = data.icon;
            spriteRenderer.color = RarityColors[(int)data.rarity];
        }

        // Petit délai avant ramassage pour éviter pickup instantané
        Invoke(nameof(EnablePickup), 0.5f);
    }

    private void EnablePickup() => _canPickup = true;

    private void Update()
    {
        if (!_canPickup) return;

        // Détecte le joueur à proximité
        Collider2D player = Physics2D.OverlapCircle(
            transform.position, pickupRadius,
            LayerMask.GetMask("Player"));

        if (player != null)
        {
            // Affiche le prompt "Appuyer sur F"
            if (Keyboard.current.fKey.wasPressedThisFrame)
                TryPickup(player.GetComponent<Inventory>());
        }
    }

    private void TryPickup(Inventory inventory)
    {
        if (inventory == null) return;

        if (inventory.AddItem(_itemData))
            Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}