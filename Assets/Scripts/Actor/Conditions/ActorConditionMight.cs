using UnityEngine;
using Sirenix.OdinInspector;

namespace Actor
{
    [CreateAssetMenu(menuName = "Conditions/Actor/Might")]
    public class ActorConditionMight : ActorConditionInt
    {
        [SerializeField, HorizontalGroup, PropertyOrder(-1), HideLabel] private ActorMight.MightType _type;
        [SerializeField, HorizontalGroup, PropertyOrder(-1), HideLabel] private CompareInt _compare;

        public override bool Check(ActorInfo target) 
            => _compare.Compare(target.Might.GetValueByType(_type), _value);
    }
}
