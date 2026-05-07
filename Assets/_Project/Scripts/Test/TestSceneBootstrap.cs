using UnityEngine;

public class TestSceneBootstrap : MonoBehaviour
{
    [Header("Prefabs à instancier si manquants")]
    [SerializeField] private GameObject gameManagerPrefab;
    [SerializeField] private GameObject cameraControllerPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerSpawnPoint;

    private void Awake()
    {
        // GameManager (contient aussi le SceneLoader)
        if (GameManager.Instance == null && gameManagerPrefab != null)
            Instantiate(gameManagerPrefab);

        // CameraController
        if (CameraController.Instance == null && cameraControllerPrefab != null)
            Instantiate(cameraControllerPrefab);
    }

    private void Start()
    {
        // Force le GameState en Playing
        if (GameManager.Instance != null)
            GameManager.Instance.SetState(GameManager.GameState.Playing);

        // Spawn joueur si absent
        if (GameObject.FindGameObjectWithTag("Player") == null && playerPrefab != null)
        {
            Vector3 spawnPos = playerSpawnPoint != null
                ? playerSpawnPoint.position
                : Vector3.zero;

            Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        }

        // Refresh HUD
        if (HUDManager.Instance != null)
            HUDManager.Instance.RefreshPlayer();
    }
}