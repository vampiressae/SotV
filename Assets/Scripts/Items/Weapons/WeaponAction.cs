using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Items;
using Entity;
using Actor;
using Affliction;

[System.Serializable]
public class WeaponAction : ActionWithInfoValueMight<WeaponAttackInfo>
{
    [SerializeField, GUIColor(1, 0.7f, 0.7f)] private List<AfflictionData> _afflictions;

    protected override string ValueName => "Damage";

    protected override bool ActIt(Item item, ActorHolder source, EntityHolder target, bool useMight)
    {
        if (!base.ActIt(item, source, target, useMight)) return false;
        if (target is not ActorHolder actor) return false;

        actor.Info.Hurt(InfluencedValueRange(source.Info).RandomUnity);
        actor.Info.AddAfflictions(_afflictions);

        if (item.RawInfo is IHasAction has && has.UseSkill)
            has.UseSkill.OnUsedSkill(source.Info);

        return true;
    }

    public override string TooltipSummaryDetails(ActorHolder actor, string tooltip) 
        => $"{base.TooltipSummaryDetails(actor, tooltip)} - {(actor ? InfluencedValueRange(actor.Info) : 0)} : {Might}";
}
