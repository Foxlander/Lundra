using System.Collections.Generic;
using UnityEngine;

public class Stat
{
    public float BaseValue { get; private set; }

    private List<StatModifier> _modifiers = new List<StatModifier>();
    private bool _isDirty = true;
    private float _cachedValue;

    public Stat(float baseValue)
    {
        BaseValue = baseValue;
    }

    public void AddModifier(StatModifier modifier)
    {
        _modifiers.Add(modifier);
        _isDirty = true;
    }

    public void RemoveModifier(StatModifier modifier)
    {
        _modifiers.Remove(modifier);
        _isDirty = true;
    }

    public void RemoveAllModifiersFromSource(object source)
    {
        _modifiers.RemoveAll(m => m.Source == source);
        _isDirty = true;
    }

    public float GetValue()
    {
        if (!_isDirty) return _cachedValue;

        _cachedValue = CalculateValue();
        _isDirty = false;
        return _cachedValue;
    }

    private float CalculateValue()
    {
        float flat = BaseValue;
        float percentAdd = 0f;
        float percentMultiply = 1f;

        foreach (StatModifier modifier in _modifiers)
        {
            switch (modifier.Type)
            {
                case ModifierType.Flat:
                    flat += modifier.Value;
                    break;

                case ModifierType.PercentAdd:
                    percentAdd += modifier.Value;
                    break;

                case ModifierType.PercentMultiply:
                    percentMultiply *= 1 + modifier.Value;
                    break;
            }
        }

        return (flat * (1 + percentAdd)) * percentMultiply;
    }
}