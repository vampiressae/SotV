using Actor;
using Sirenix.OdinInspector;
using UnityEngine;

using static Affliction.AfflictionInfo;

namespace Affliction
{
    [System.Serializable]
    public class AfflictionData
    {
        [HideLabel] public AfflictionInfo Info;
        [SerializeField, HorizontalGroup("add", 150), LabelWidth(50), LabelText("Add")] private AddMode _addMode;
        [SerializeField, HorizontalGroup("add"), HideLabel, Range(0, 1)] private float _addChance = 1;
        [SerializeField, HorizontalGroup("apply", 150), LabelWidth(50)] private AfflictionMoment _moment;
        [SerializeField, HorizontalGroup("apply"), HideLabel, Range(0, 1)] private float _applyChance = 1;
        [SerializeField, LabelWidth(50)] private int _applies;

        public AfflictionMoment Moment => _moment;
        public int Applies => _applies;

        public AfflictionData() { }
        public AfflictionData(AfflictionInfo info) => Info = info;

        public AddMode GetAddMode()
        {
            if (_addMode == AddMode.None) return AddMode.None;
            var random = Random.value;
            if (_addChance < random) return AddMode.None;
            return _addMode;
        }

        public bool TryApply()
        {
            Expire();
            return _applyChance > Random.value;
        }

        public bool Expire()
        {
            _applies--;
            return _applies < 1;
        }

        public void AddApplies(int add) => _applies += add;

        public bool Afflict(AfflictionMoment moment, ActorInfo actor)
            => Info.Afflict(moment, actor, this);
    }
}
