using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    [Header("=== HEALTH ===")]
    [SerializeField] private Image healthBarFill;
    [SerializeField] private Image healthBarDelayed;   // barre "ghost" qui suit en retard
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("=== MANA ===")]
    [SerializeField] private Image manaBarFill;
    [SerializeField] private Image manaBarDelayed;
    [SerializeField] private TextMeshProUGUI manaText;

    [Header("=== SKILL SLOTS ===")]
    [SerializeField] private SkillSlotUI[] skillSlots; // 4 slots

    [Header("=== SETTINGS ===")]
    [SerializeField] private float delayedBarSpeed = 3f; // vitesse de la barre ghost

    private CharacterStats _playerStats;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        // Trouve le joueur
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            _playerStats = player.GetComponent<CharacterStats>();
        else
            Debug.LogWarning("HUDManager : aucun GameObject taggé 'Player' trouvé.");
    }

    private void Update()
    {
        if (_playerStats == null) return;

        UpdateHealthBar();
        UpdateManaBar();
    }

    // ── Santé ────────────────────────────────────────────
    private void UpdateHealthBar()
    {
        float current = _playerStats.CurrentHealth;
        float max     = _playerStats.GetStatValue(StatType.MaxHealth);
        float ratio   = max > 0 ? current / max : 0f;

        healthBarFill.fillAmount = ratio;

        // Barre ghost : suit en retard
        if (healthBarDelayed != null)
            healthBarDelayed.fillAmount = Mathf.Lerp(
                healthBarDelayed.fillAmount, ratio, delayedBarSpeed * Time.deltaTime);

        if (healthText != null)
            healthText.text = $"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}";
    }

    // ── Mana ─────────────────────────────────────────────
    private void UpdateManaBar()
    {
        float current = _playerStats.CurrentMana;
        float max     = _playerStats.GetStatValue(StatType.MaxMana);
        float ratio   = max > 0 ? current / max : 0f;

        manaBarFill.fillAmount = ratio;

        if (manaBarDelayed != null)
            manaBarDelayed.fillAmount = Mathf.Lerp(
                manaBarDelayed.fillAmount, ratio, delayedBarSpeed * Time.deltaTime);

        if (manaText != null)
            manaText.text = $"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}";
    }

    // ── API publique ──────────────────────────────────────
    // Appelé par SkillManager quand le joueur change de scène
    public void RefreshPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            _playerStats = player.GetComponent<CharacterStats>();
    }
}