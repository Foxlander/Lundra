using UnityEngine;
using UnityEngine.InputSystem;

public class SkillManager : MonoBehaviour
{
    [Header("Skills")]
    [SerializeField] private SkillBase skill1;
    [SerializeField] private SkillBase skill2;
    [SerializeField] private SkillBase skill3;
    [SerializeField] private SkillBase skill4;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    // Rafraîchit la caméra à chaque scène chargée
    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        // Récupère la nouvelle caméra de la scène chargée
        _camera = Camera.main;
        Debug.Log($"SkillManager : Camera refreshed for scene {scene.name}");
    }

    private void Update()
    {
        if (GameManager.Instance != null && !GameManager.Instance.IsPlaying) return;

        // Vérifie que la caméra existe avant de l'utiliser
        if (_camera == null)
        {
            _camera = Camera.main;
            return;
        }

        Vector2 mouseWorldPos = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (Mouse.current.leftButton.wasPressedThisFrame)
            skill1?.TryUse(mouseWorldPos);

        if (Mouse.current.rightButton.wasPressedThisFrame)
            skill2?.TryUse(mouseWorldPos);

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
            skill3?.TryUse(mouseWorldPos);

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
            skill4?.TryUse(mouseWorldPos);
    }
}