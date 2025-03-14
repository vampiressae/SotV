using UnityEngine;
using Sirenix.OdinInspector;

namespace Actor
{
    [CreateAssetMenu(menuName = "Conditions/Actor/Might")]
    public class ActorConditionMight : ActorConditionInt
    {
        private enum Mode { Value, Percent }

        [SerializeField, HorizontalGroup, PropertyOrder(-1), HideLabel] private ActorMight.MightType _type;
        [SerializeField, HorizontalGroup, PropertyOrder(-1), HideLabel] private CompareInt _compare;
        [SerializeField, HorizontalGroup, HideLabel] private Mode _mode;

        private bool _percent => _mode == Mode.Percent;

        protected override bool Check(ActorInfo actor)
        {
            var value = actor.Might.GetValueByType(_type);
            var mode = _percent ? actor.Might.Max * _value / 100 : _value;
            var ok = _compare.Compare(value, mode);
            Debug.Log($"Condition Check for Actor <b>{actor.Name}</b>: {_compare.ToString(value, mode)} ({this})");
            return ok;
        }

        public override string ToString()
        {
            if (_compare == CompareInt.None) return "Skipped!";
            var compare = _compare == CompareInt.Equal ? "with" : "than";
            return $"Might.{_type} is {_compare} {compare} {_value}{(_percent ? "%" : "")}";
        }
    }
}
