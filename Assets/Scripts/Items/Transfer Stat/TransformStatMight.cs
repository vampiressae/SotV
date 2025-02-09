using UnityEngine;
using Actor;
using Entity;

[CreateAssetMenu(menuName = "Items/Actions/Transfer Stat/Might")]
public class TransformStatMight : TransformStatInfo
{
    public override bool Transfer(EntityHolder target, int value)
    {
        if (target is not ActorHolder actor) return false;
        actor.Info.Might.AddMissingValue(value);
        return true;
    }
}
