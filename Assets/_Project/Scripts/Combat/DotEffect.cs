public class DotEffect
{
    public DotType Type { get; private set; }
    public float DamagePerTick { get; private set; }
    public float TickInterval { get; private set; }   // Temps entre chaque tick
    public float Duration { get; private set; }
    public CharacterStats Source { get; private set; }

    private float _tickTimer;
    private float _remainingDuration;

    public bool IsExpired => _remainingDuration <= 0f;

    public DotEffect(DotType type, float damagePerTick, float tickInterval, float duration, CharacterStats source)
    {
        Type = type;
        DamagePerTick = damagePerTick;
        TickInterval = tickInterval;
        Duration = duration;
        Source = source;

        _tickTimer = tickInterval;
        _remainingDuration = duration;
    }

    // Retourne true si un tick de dégât doit être appliqué
    public bool Update(float deltaTime)
    {
        _remainingDuration -= deltaTime;
        _tickTimer -= deltaTime;

        if (_tickTimer <= 0f)
        {
            _tickTimer = TickInterval;
            return true; // Tick !
        }

        return false;
    }

    public float RemainingDuration => _remainingDuration;
    public float DurationPercent => _remainingDuration / Duration;
}