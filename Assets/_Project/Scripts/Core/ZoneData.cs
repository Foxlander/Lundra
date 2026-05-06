using UnityEngine;

[CreateAssetMenu(fileName = "ZoneData", menuName = "Game/Zone Data")]
public class ZoneData : ScriptableObject
{
    [Header("Infos")]
    public string ZoneName;
    public string SceneName;
    public Sprite ZoneIcon;
    [TextArea] public string Description;

    [Header("Niveau")]
    public int MinLevel;
    public int MaxLevel;

    [Header("Ennemis")]
    public GameObject[] EnemyPrefabs;
    public int MinEnemiesPerRoom;
    public int MaxEnemiesPerRoom;

    [Header("Loot")]
    public float LootMultiplier = 1f;
}