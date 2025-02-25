using Sirenix.OdinInspector;
using UnityEngine;

namespace Actor
{
    public abstract class ActorCondition : Condition
    {
        public override bool Check(object target) => target is ActorInfo actor && Check(actor);
        protected abstract bool Check(ActorInfo actor);
    }

    public abstract class ActorConditionInt : ActorCondition
    {
        [SerializeField, HorizontalGroup(50), HideLabel] protected int _value;
    }
}
