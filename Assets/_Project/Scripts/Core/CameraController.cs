using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10f);

    private Transform _target;

    private void Awake()
    {
        // Singleton + persistance
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
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
        // Retrouve le joueur dans la nouvelle scène
        FindPlayer();

        // Refresh le HUD aussi
        if (HUDManager.Instance != null)
            HUDManager.Instance.RefreshPlayer();
    }

    private void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _target = player.transform;
            Debug.Log("CameraController : Player found !");
        }
        else
        {
            Debug.LogWarning("CameraController : No Player found in scene !");
        }
    }

    private void LateUpdate()
    {
        if (_target == null)
        {
            FindPlayer();
            return;
        }

        Vector3 desiredPosition = _target.position + offset;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}