using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CharacterStats))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Vector2 _moveInput;
    private Camera _camera;
    private CharacterStats _stats;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
        _stats = GetComponent<CharacterStats>();
    }

    private void Update()
    {
        if (GameManager.Instance != null && !GameManager.Instance.IsPlaying) return;

        HandleFlip();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    private void HandleMovement()
    {
        // MoveSpeed vient maintenant du StatSystem !
        float speed = _stats.GetStatValue(StatType.MoveSpeed);
        _rb.linearVelocity = _moveInput * speed;
    }

    private void HandleFlip()
    {
        Vector3 mousePos = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (mousePos.x < transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

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
        _camera = Camera.main;
    }

    public Vector2 MoveDirection => _moveInput;
    public bool IsMoving => _moveInput != Vector2.zero;
    public float MoveSpeed => _stats.GetStatValue(StatType.MoveSpeed);

    
}