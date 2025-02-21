using Items;
using Entity;
using Actor;

[System.Serializable]
public class TransferStatAction : ActionWithInfoValueMight<TransformStatInfo>
{
    protected override string ValueName => "Might";

    protected override bool ActIt(Item item, ActorHolder source, EntityHolder target, bool useMight)
    {
        if (!base.ActIt(item, source, target, useMight)) return false;
        if (target is not ActorHolder actor) return false;

        actor.Info.Heal(InfluencedValueRange(source.Info).RandomUnity);
        return true;
    }
}
