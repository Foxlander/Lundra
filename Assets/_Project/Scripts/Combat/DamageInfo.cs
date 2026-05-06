// Structure pour transporter les infos de dégâts
public struct DamageInfo
{
    public float Amount;
    public bool IsCrit;
    public DamageType Type;
    public CharacterStats Source;

    public DamageInfo(float amount, bool isCrit, DamageType type, CharacterStats source)
    {
        Amount = amount;
        IsCrit = isCrit;
        Type = type;
        Source = source;
    }
}

public enum DamageType
{
    Physical,
    Fire,
    Ice,
    Lightning,
    Poison,
    Pure // Ignore l'armure
}