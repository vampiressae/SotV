using Actor;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Affliction
{
    [System.Serializable]
    public class AfflictionData
    {
        [HideLabel] public AfflictionInfo Info;
        [SerializeField, HorizontalGroup("add"), Range(0, 1)] private float _add = 1;
        [SerializeField, HorizontalGroup("apply"), Range(0, 1)] private float _apply = 1;
        [SerializeField] private int _applies, _value;

        public float ApplyChance => _apply;
        public int Applies => _applies;
        public int Value => _value;

        public AfflictionData() { }
        public AfflictionData(AfflictionInfo info) => Info = info;

        public AfflictionAddMode GetAddMode()
        {
            if (Info.AddMode == AfflictionAddMode.None) return AfflictionAddMode.None;
            var random = Random.value;
            if (_add < random) return AfflictionAddMode.None;
            return Info.AddMode;
        }

        public bool Expire() => --_applies < 1;
        public void AddApplies(int add) => _applies += add;

        public bool Afflict(AfflictionMoment moment, ActorInfo actor)
            => Info.Afflict(moment, actor, this);
    }
}
