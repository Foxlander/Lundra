using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    [Header("Skill Settings")]
    [SerializeField] protected string skillName = "Skill";
    [SerializeField] protected float cooldown = 1f;
    [SerializeField] protected float manaCost = 10f;
    [SerializeField] protected DamageType damageType = DamageType.Physical;
    [SerializeField] private Sprite icon;

    protected float CooldownTimer;
    protected CharacterStats OwnerStats;
    protected Camera MainCamera;
    public float RemainingCooldown => CooldownTimer;
    

    protected virtual void Awake()
    {
        OwnerStats = GetComponentInParent<CharacterStats>();
        MainCamera = Camera.main;
    }

    protected virtual void Update()
    {
        if (CooldownTimer > 0)
            CooldownTimer -= Time.deltaTime;
    }

    public bool CanUse()
    {
        return CooldownTimer <= 0 &&
               OwnerStats.HasEnoughMana(manaCost);
    }

    public void TryUse(Vector2 targetPosition)
    {
        if (!CanUse()) return;

        OwnerStats.UseMana(manaCost);
        CooldownTimer = cooldown;
        Execute(targetPosition);
    }

    protected abstract void Execute(Vector2 targetPosition);

    public float CooldownPercent => CooldownTimer / cooldown;
    public string SkillName => skillName;
    public Sprite Icon => icon;
}