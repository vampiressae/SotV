using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Items;
using Inventory;
using Entity;

[CreateAssetMenu(menuName = "Items/Effects/Stat")]
public class ItemFXEquipStat : ItemFXEquip
{
    [SerializeField, HideLabel, HorizontalGroup(50)] private int _amount;
    [SerializeField, HideLabel, HorizontalGroup] private StatInfo _info;

    public override void AddedToEquipment(EquipmentHolder equipment, ItemData item)
    {
        if (equipment.Actor == null) return;
        equipment.Actor.Info.AddStatValue(item, _info, _amount);
    }

    public override void RemovedFromEquipment(EquipmentHolder equipment, ItemData item)
    {
        if (equipment.Actor == null) return;
        equipment.Actor.Info.RemoveStatValue(item, _info);
    }

    public override void GetTooltip(ref List<string> list)
    {
        base.GetTooltip(ref list);
        list.Add(_info.Name.ToLabelAndValue(_amount));
    }
}
