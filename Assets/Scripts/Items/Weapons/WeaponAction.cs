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

        actor.Info.Might.AddMissingValue(Value.RandomUnity);
        return true;
    }
}
