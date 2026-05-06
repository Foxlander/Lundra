using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    [SerializeField] private float transitionDuration = 0.5f;

    // Scènes du jeu
    public const string SCENE_BOOT = "Boot";
    public const string SCENE_HUB = "Hub";
    public const string SCENE_ZONE_FORET = "Zone_Foret";
    public const string SCENE_ZONE_DONJON = "Zone_Donjon";
    public const string SCENE_ZONE_DESERT = "Zone_Desert";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadHub() => LoadScene(SCENE_HUB);
    public void LoadZone(string zoneName) => LoadScene(zoneName);
    public void ReloadCurrentScene() => LoadScene(SceneManager.GetActiveScene().name);

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        GameManager.Instance.SetState(GameManager.GameState.Paused);

        yield return new WaitForSecondsRealtime(transitionDuration);

        SceneManager.LoadScene(sceneName);

        // Si on charge le Hub on passe en mode Hub
        if (sceneName == SCENE_HUB)
            GameManager.Instance.SetState(GameManager.GameState.Hub);
        else
            GameManager.Instance.SetState(GameManager.GameState.Playing);
    }
}