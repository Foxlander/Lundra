using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class NovaVFX : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float maxRadius = 5f;
    [SerializeField] private float expansionSpeed = 8f;
    [SerializeField] private Color novaColor = new Color(0.5f, 0.8f, 1f, 0.8f);

    private float _currentRadius = 0f;
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        SetupLineRenderer();
    }

    private void SetupLineRenderer()
    {
        _lineRenderer.useWorldSpace = false;
        _lineRenderer.loop = true;
        _lineRenderer.positionCount = 64;
        _lineRenderer.startWidth = 0.1f;
        _lineRenderer.endWidth = 0.1f;
        _lineRenderer.sortingLayerName = "Default";
        _lineRenderer.sortingOrder = 999;
        _lineRenderer.startColor = novaColor;
        _lineRenderer.endColor = novaColor;

        DrawCircle(maxRadius);
    }

    private void Update()
    {
        if (!Application.isPlaying)
        {
            DrawCircle(maxRadius);
            return;
        }

        _currentRadius += expansionSpeed * Time.deltaTime;

        if (_currentRadius >= maxRadius)
        {
            Destroy(gameObject);
            return;
        }

        DrawCircle(_currentRadius);

        float alpha = 1f - (_currentRadius / maxRadius);
        _lineRenderer.startColor = new Color(novaColor.r, novaColor.g, novaColor.b, alpha);
        _lineRenderer.endColor = new Color(novaColor.r, novaColor.g, novaColor.b, alpha);
    }

    private void DrawCircle(float radius)
    {
        int segments = 64;
        for (int i = 0; i < segments; i++)
        {
            float angle = (float)i / segments * 2f * Mathf.PI;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            _lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }
    }
}