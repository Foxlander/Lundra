using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Spawner de test — raccourcis clavier pour tester rapidement
/// </summary>
public class TestSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject itemDropPrefab;

    [Header("Templates items")]
    [SerializeField] private ItemTemplate[] itemTemplates;

    [Header("Settings")]
    [SerializeField] private float spawnRadius = 3f;
    [SerializeField] private int enemyLevel = 1;

    private void Update()
    {
        // E → Spawn ennemi autour du joueur
        if (Keyboard.current.eKey.wasPressedThisFrame)
            SpawnEnemy();

        // I → Spawn item aléatoire
        if (Keyboard.current.iKey.wasPressedThisFrame)
            SpawnRandomItem();

        // K → Tue tous les ennemis
        if (Keyboard.current.kKey.wasPressedThisFrame)
            KillAllEnemies();

        // H → Heal joueur full
        if (Keyboard.current.hKey.wasPressedThisFrame)
            HealPlayer();
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null) return;

        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        Debug.Log("TestSpawner : Ennemi spawné !");
    }

    private void SpawnRandomItem()
    {
        if (itemTemplates.Length == 0 || itemDropPrefab == null) return;

        // Template aléatoire
        ItemTemplate template = itemTemplates[Random.Range(0, itemTemplates.Length)];

        // Rareté aléatoire
        ItemRarity rarity = (ItemRarity)Random.Range(0, System.Enum.GetValues(typeof(ItemRarity)).Length);

        // Génère l'item
        ItemData item = ItemGenerator.Generate(template, rarity, enemyLevel, 1);

        // Spawn au sol devant le joueur
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 spawnPos = player != null ? player.transform.position + Vector3.right * 1.5f : Vector3.zero;

        GameObject go = Instantiate(itemDropPrefab, spawnPos, Quaternion.identity);
        go.GetComponent<ItemDrop>().Init(item);

        Debug.Log($"TestSpawner : Item spawné — {item.itemName} ({item.rarity})");
    }

    private void KillAllEnemies()
    {
        EnemyBase[] enemies = FindObjectsByType<EnemyBase>(FindObjectsSortMode.None);
        foreach (EnemyBase enemy in enemies)
            enemy.OnDeath();

        Debug.Log($"TestSpawner : {enemies.Length} ennemis tués !");
    }

    private void HealPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        CharacterStats stats = player.GetComponent<CharacterStats>();
        if (stats != null)
        {
            stats.FullHeal();
            stats.FullMana();
            Debug.Log("TestSpawner : Joueur full HP/Mana !");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}