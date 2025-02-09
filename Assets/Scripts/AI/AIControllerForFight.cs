using Actor;
using UnityEngine;

public class AIControllerForFight : MonoBehaviour
{
    public bool SimpleAttack(ActorHolder source, ActorHolder target)
    {
        var data = source.Info.Equipment[0];
        var weapon = data.Info as WeaponInfo;

        if (weapon == null) return false;

        var attack = weapon.Actions[Random.Range(0, weapon.Actions.Length)];
        attack.Act(data, source, target, false);

        return true;
    }
}
