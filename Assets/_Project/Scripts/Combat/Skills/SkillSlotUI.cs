using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillSlotUI : MonoBehaviour
{
    [Header("Références UI")]
    [SerializeField] private Image iconImage;
    [SerializeField] private Image cooldownOverlay;   // Image noire semi-transparente en radial
    [SerializeField] private TextMeshProUGUI cooldownText;
    [SerializeField] private TextMeshProUGUI keyLabel; // "1", "2", "3", "4"

    private SkillBase _skill;

    public void Init(SkillBase skill, string key)
    {
        _skill = skill;

        if (iconImage != null && skill.Icon != null)
            iconImage.sprite = skill.Icon;

        if (keyLabel != null)
            keyLabel.text = key;
    }

    private void Update()
    {
        if (_skill == null) return;

        float ratio = _skill.CooldownPercent; // 0 = dispo, 1 = plein cooldown

        if (cooldownOverlay != null)
            cooldownOverlay.fillAmount = ratio;

        if (cooldownText != null)
        {
            if (ratio > 0f)
                cooldownText.text = _skill.RemainingCooldown.ToString("F1");
            else
                cooldownText.text = "";
        }
    }
}