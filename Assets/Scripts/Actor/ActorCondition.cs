using Sirenix.OdinInspector;
using UnityEngine;

namespace Actor
{
    public abstract class ActorCondition : Condition<ActorInfo> { }
    public abstract class ActorConditionInt : ActorCondition
    {
        [SerializeField, HorizontalGroup(50), HideLabel] protected int _value;
    }
}
