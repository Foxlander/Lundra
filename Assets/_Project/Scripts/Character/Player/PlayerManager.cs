using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // ← Le joueur persiste !
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Repositionne le joueur selon la scène chargée
        if (scene.name == SceneLoader.SCENE_HUB)
            MoveToHubSpawn();
        else
            MoveToZoneSpawn();
    }

    private void MoveToHubSpawn()
    {
        // Cherche le point de spawn du Hub
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("HubSpawn");
        if (spawnPoint != null)
            transform.position = spawnPoint.transform.position;
        else
            transform.position = Vector3.zero;

        Debug.Log("Player moved to Hub spawn !");
    }

    private void MoveToZoneSpawn()
    {
        // Cherche le point de spawn de la zone
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("ZoneSpawn");
        if (spawnPoint != null)
            transform.position = spawnPoint.transform.position;
        else
            transform.position = Vector3.zero;

        Debug.Log("Player moved to Zone spawn !");
    }
}