using UnityEngine;
using Sirenix.OdinInspector;
using Items;
using Inventory;
using Actor;

[CreateAssetMenu(menuName = "Items/Effects/Might")]
public class ItemFXEquipMight : ItemFXEquip
{
    private enum MightType { None, Reserved, Missing, Recoverable, Regen }
    private enum ModifierType { None, Multipler, Addition }

    [SerializeField, HideLabel, HorizontalGroup] private MightType _type;
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
            case ModifierType.Multipler: equipment.Actor.Info.Might.RemoveMultiplier(TrueType, item); break;
            case ModifierType.Addition: equipment.Actor.Info.Might.RemoveAddition(TrueType, item); break;
        }
    }

    private ActorMight.MightType TrueType => _type switch
    {
        MightType.Reserved => ActorMight.MightType.Reserved,
        MightType.Missing => ActorMight.MightType.Missing,
        MightType.Recoverable => ActorMight.MightType.Recoverable,
        MightType.Regen => ActorMight.MightType.Regen,
        _ => ActorMight.MightType.None,
    };
}
