using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    private Dictionary<EquipmentSlot, ItemData> _equipped
        = new Dictionary<EquipmentSlot, ItemData>();

    private CharacterStats _stats;
    private Inventory _inventory;

    // Suivi des bonus de set actifs
    private Dictionary<SetData, int> _activeSets
        = new Dictionary<SetData, int>();

    private void Awake()
    {
        _stats = GetComponent<CharacterStats>();
        _inventory = GetComponent<Inventory>();
    }

    public void Equip(ItemData item)
    {
        // Si 2 mains → déséquipe l'OffHand
        if (item.handType == HandType.TwoHanded)
        {
            if (_equipped.ContainsKey(EquipmentSlot.OffHand))
                Unequip(EquipmentSlot.OffHand);
        }

        // Si on équipe OffHand → vérifie que MainHand n'est pas 2 mains
        if (item.slot == EquipmentSlot.OffHand)
        {
            if (_equipped.TryGetValue(EquipmentSlot.MainHand, out ItemData mainHand))
                if (mainHand.handType == HandType.TwoHanded)
                    Unequip(EquipmentSlot.MainHand);
        }

        // Déséquipe l'item actuel du slot
        if (_equipped.ContainsKey(item.slot))
            Unequip(item.slot);

        // Équipe
        _equipped[item.slot] = item;
        ApplyStats(item, true);

        // Mise à jour des sets
        if (item.setData != null)
            UpdateSetBonus(item.setData, true);

        Debug.Log($"Équipé : {item.itemName}");
    }

    public void Unequip(EquipmentSlot slot)
    {
        if (!_equipped.TryGetValue(slot, out ItemData item)) return;

        ApplyStats(item, false);

        // Mise à jour des sets
        if (item.setData != null)
            UpdateSetBonus(item.setData, false);

        _inventory.AddItem(item);
        _equipped.Remove(slot);

        Debug.Log($"Déséquipé : {item.itemName}");
    }

    // ── Stats items ───────────────────────────────────────
    private void ApplyStats(ItemData item, bool apply)
    {
        foreach (ItemStat itemStat in item.stats)
        {
            StatModifier modifier = new StatModifier(
                itemStat.value,
                itemStat.modifierType,
                item
            );

            if (apply)
                _stats.AddModifier(itemStat.statType, modifier);
            else
                _stats.RemoveModifier(itemStat.statType, modifier);
        }
    }

    // ── Gestion des sets ──────────────────────────────────
    private void UpdateSetBonus(SetData setData, bool equipping)
    {
        // Compte les pièces avant modification
        int piecesOld = _activeSets.TryGetValue(setData, out int count) ? count : 0;
        int piecesNew = equipping ? piecesOld + 1 : piecesOld - 1;

        // Met à jour le compteur
        if (piecesNew <= 0)
            _activeSets.Remove(setData);
        else
            _activeSets[setData] = piecesNew;

        // Retire les anciens bonus puis applique les nouveaux
        RemoveSetBonuses(setData, piecesOld);
        ApplySetBonuses(setData, piecesNew);

        Debug.Log($"Set {setData.setName} : {piecesNew} pièces actives");
    }

    private void ApplySetBonuses(SetData setData, int pieceCount)
    {
        foreach (SetBonus bonus in setData.bonuses)
        {
            if (pieceCount < bonus.requiredPieces) continue;

            // Stat bonus
            if (bonus.hasStat)
            {
                StatModifier modifier = new StatModifier(
                    bonus.value,
                    bonus.modifierType,
                    setData
                );
                _stats.AddModifier(bonus.statType, modifier);
            }

            // Affixe spécial
            if (bonus.hasAffix && bonus.affix != null)
            {
                StatModifier affixModifier = new StatModifier(
                    Random.Range(bonus.affix.minValue, bonus.affix.maxValue),
                    bonus.affix.modifierType,
                    setData
                );
                _stats.AddModifier(bonus.affix.statType, affixModifier);

                Debug.Log($"Affixe de set activé : {bonus.affix.affixName}");
            }

            Debug.Log($"Bonus set activé : {bonus.requiredPieces} pièces — {bonus.description}");
        }
    }

    private void RemoveSetBonuses(SetData setData, int pieceCount)
    {
        foreach (SetBonus bonus in setData.bonuses)
        {
            if (pieceCount < bonus.requiredPieces) continue;

            // Retire tous les modificateurs liés à ce set
            _stats.RemoveAllModifiersFromSource(setData);
        }
    }

    public ItemData GetEquipped(EquipmentSlot slot)
    {
        _equipped.TryGetValue(slot, out ItemData item);
        return item;
    }

    // Retourne le nombre de pièces actives d'un set
    public int GetSetPieceCount(SetData setData)
    {
        return _activeSets.TryGetValue(setData, out int count) ? count : 0;
    }

    // Retourne tous les sets actifs
    public Dictionary<SetData, int> GetActiveSets() => _activeSets;
}