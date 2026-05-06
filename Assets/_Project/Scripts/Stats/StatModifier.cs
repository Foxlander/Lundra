public enum ModifierType
{
    Flat,               // +10 dégâts
    PercentAdd,         // +10% dégâts (additif)
    PercentMultiply     // x1.10 dégâts (multiplicatif)
}

public class StatModifier
{
    public float Value { get; private set; }
    public ModifierType Type { get; private set; }
    public object Source { get; private set; } // Item, Skill, Buff...

    public StatModifier(float value, ModifierType type, object source = null)
    {
        Value = value;
        Type = type;
        Source = source;
    }
}