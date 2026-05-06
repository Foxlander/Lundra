using UnityEngine;
using System.Collections.Generic;

public class DotSystem : MonoBehaviour
{
    // Liste des DoT actifs sur ce personnage
    private List<DotEffect> _activeDots = new List<DotEffect>();

    private CharacterStats _stats;

    private void Awake()
    {
        _stats = GetComponent<CharacterStats>();
    }

    private void Update()
    {
        if (_activeDots.Count == 0) return;

        ProcessDots();
    }

    private void ProcessDots()
    {
        for (int i = _activeDots.Count - 1; i >= 0; i--)
        {
            DotEffect dot = _activeDots[i];

            bool shouldTick = dot.Update(Time.deltaTime);

            if (shouldTick)
            {
                ApplyDotDamage(dot);
            }

            if (dot.IsExpired)
            {
                _activeDots.RemoveAt(i);
                Debug.Log($"{gameObject.name} : {dot.Type} expired");
            }
        }
    }

    private void ApplyDotDamage(DotEffect dot)
    {
        // Convertit DotType en DamageType
        DamageType damageType = GetDamageType(dot.Type);

        DamageInfo damageInfo = new DamageInfo(
            dot.DamagePerTick,
            false, // Les DoT ne critent pas par défaut
            damageType,
            dot.Source
        );

        _stats.TakeDamage(damageInfo);

        Debug.Log($"{gameObject.name} took {dot.DamagePerTick} {dot.Type} damage. " +
                  $"({dot.RemainingDuration:F1}s remaining)");
    }

    // Applique un nouveau DoT
    public void ApplyDot(DotEffect dot)
    {
        // Vérifie si un DoT du même type existe déjà
        DotEffect existing = _activeDots.Find(d => d.Type == dot.Type);

        if (existing != null)
        {
            // Refresh → on remplace par le nouveau
            _activeDots.Remove(existing);
            Debug.Log($"{gameObject.name} : {dot.Type} refreshed !");
        }

        _activeDots.Add(dot);
        Debug.Log($"{gameObject.name} : {dot.Type} applied !");
    }

    // Retire un DoT spécifique
    public void RemoveDot(DotType type)
    {
        _activeDots.RemoveAll(d => d.Type == type);
    }

    // Retire tous les DoT
    public void RemoveAllDots()
    {
        _activeDots.Clear();
        Debug.Log($"{gameObject.name} : all DoTs cleared !");
    }

    // Vérifie si un DoT est actif
    public bool HasDot(DotType type)
    {
        return _activeDots.Exists(d => d.Type == type);
    }

    // Convertit DotType en DamageType
    private DamageType GetDamageType(DotType dotType)
    {
        switch (dotType)
        {
            case DotType.Poison:    return DamageType.Poison;
            case DotType.Burn:      return DamageType.Fire;
            case DotType.Bleed:     return DamageType.Physical;
            default:                return DamageType.Pure;
        }
    }

    // Accesseurs utiles pour le HUD
    public List<DotEffect> ActiveDots => _activeDots;
    public int DotCount => _activeDots.Count;
}