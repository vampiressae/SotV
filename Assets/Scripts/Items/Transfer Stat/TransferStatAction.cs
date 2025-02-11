using UnityEngine;
using Items;
using Actor;
using Entity;

[System.Serializable]
public class TransferStatAction : ActionWithInfoValueMight<TransformStatInfo>
{
    protected override string ValueName => "Might";

    protected override bool ActIt(Item item, ActorHolder source, EntityHolder target, bool useMight)
    {
        if (!base.ActIt(item, source, target, useMight)) return false;
        if (target is not ActorHolder actor) return false;

        Debug.Log("Transferring " + Value + " for " + _might);

        actor.Info.Might.RemoveMissingValue(InfluencedValueRange(source.Info).RandomUnity);
        return true;
    }
}
