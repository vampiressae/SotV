using Items;
using Entity;
using Actor;

[System.Serializable]
public class WeaponAction : ActionWithInfoValueMight<WeaponAttackInfo>
{
    protected override string ValueName => "Damage";

    protected override bool ActIt(Item item, ActorHolder source, EntityHolder target, bool useMight)
    {
        if (!base.ActIt(item, source, target, useMight)) return false;
        if (target is not ActorHolder actor) return false;

        actor.Info.Might.AddMissingValue(InfluencedValueRange(source.Info).RandomUnity);
        return true;
    }

    public override string TooltipSummaryDetails(ActorHolder actor, string tooltip) 
        => $"{base.TooltipSummaryDetails(actor, tooltip)} - {(actor ? InfluencedValueRange(actor.Info) : 0)} : {Might}";
}
