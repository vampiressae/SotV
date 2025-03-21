﻿using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Items;
using Inventory;
using Actor;

using static Actor.ActorMight;

[CreateAssetMenu(menuName = "Items/Effects/Might")]
public class ItemFXEquipMight : ItemFXEquip
{
    private enum ModifierType { None, Multipler, Addition }

    [SerializeField, HideLabel, HorizontalGroup] private MightTypeEffect _type;
    [SerializeField, HideLabel, HorizontalGroup] private ModifierType _modifier;
    [SerializeField, HideLabel, HorizontalGroup(50)] private int _value;

    public override void AddedToEquipment(EquipmentHolder equipment, ItemData item)
    {
        if (equipment.Actor == null) return;
        switch (_modifier)
        {
            case ModifierType.Multipler: equipment.Actor.Info.Might.AddMultiplier(TrueType, item, _value); break;
            case ModifierType.Addition: equipment.Actor.Info.Might.AddAddition(TrueType, item, _value); break;
        }
    }

    public override void RemovedFromEquipment(EquipmentHolder equipment, ItemData item)
    {
        if (equipment.Actor == null) return;
        switch (_modifier)
        {
            case ModifierType.Multipler: equipment.Actor.Info.Might.RemoveMultiplier(TrueType, item, _value); break;
            case ModifierType.Addition: equipment.Actor.Info.Might.RemoveAddition(TrueType, item, _value); break;
        }
    }

    private MightType TrueType => _type switch
    {
        MightTypeEffect.Reserved => MightType.Reserved,
        MightTypeEffect.Missing => MightType.Missing,
        MightTypeEffect.Recoverable => MightType.Consumed,
        MightTypeEffect.Regen => MightType.Regen,
        MightTypeEffect.Max => MightType.Max,
        _ => MightType.None,
    };

    public override void GetTooltip(ref List<string> list)
    {
        var modifier = _modifier == ModifierType.Multipler ? "%" : "";
        list.Add(TooltipName.ToLabelAndValue(_value.ToStringWithSign() + modifier));
    }

    private string TooltipName => _type switch
    {
        MightTypeEffect.Reserved => "Reserved Might",
        MightTypeEffect.Missing => "Might Damage",
        MightTypeEffect.Recoverable => "Might Cost",
        MightTypeEffect.Regen => "Might Regeneration",
        MightTypeEffect.Max => "Maximum Might",
        _ => "[NO TOOLTIP NAME DEFINED]",
    };
}
