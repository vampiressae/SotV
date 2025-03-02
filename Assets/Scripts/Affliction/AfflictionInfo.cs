using UnityEngine;
using Sirenix.OdinInspector;
using Actor;
using TMPro;

namespace Affliction
{
    [GUIColor("white")]
    public abstract class AfflictionInfo : ScriptableWithNameAndSpriteAndTooltip, IActorMightMissing
    {
        public enum AddMode { None, Unique, Stack }

        [SerializeField, HorizontalGroup("color", 150), ColorUsage(false)] private Color _color;
        [SerializeField, HorizontalGroup ("color"), HideLabel] private TMP_ColorGradient _gradient;
        [SerializeField, HorizontalGroup("add", 150), LabelWidth(50), LabelText("Add")] private AddMode _addMode;
        [SerializeField, HorizontalGroup("add"), HideLabel, Range(0, 1)] private float _addChance = 1;
        [SerializeField, HorizontalGroup("apply", 150), LabelWidth(50)] private AfflictionMoment _moment;
        [SerializeField, HorizontalGroup("apply"), HideLabel, Range(0, 1)] private float _applyChance = 1;
        [SerializeField, LabelWidth(50), HideIf("HideProperty")] private int _applies;

        string IActorMightDictionaryKey.Name => Name;
        Sprite IActorMightDictionaryKeyPlus.Icon => Icon;
        public TMP_ColorGradient Gradient => _gradient;
        public Color Color => _color;
        public int Applies => _applies;

        public AddMode GetAddMode()
        {
            if (_addMode == AddMode.None) return AddMode.None;
            var random = Random.value;
            if (_addChance < random) return AddMode.None;
            return _addMode;
        }

        public bool Afflict(AfflictionMoment moment, ActorInfo actor)
        {
            if (_applyChance < Random.value) return Expire();

            switch (moment)
            {
                case AfflictionMoment.Added: return AfflictOnAdded(actor);
                case AfflictionMoment.Removed: return AfflictOnRemoved(actor);
            }
            if (_moment != moment) return false;
            return Afflict(actor);
        }

        protected virtual bool AfflictOnAdded(ActorInfo actor) => Expire();
        protected virtual bool AfflictOnRemoved(ActorInfo actor) => Expire();
        protected abstract bool Afflict(ActorInfo actor);

        protected bool Expire()
        {
            _applies--;
            return _applies < 1;
        }

        public void AddApplies(int add) => _applies += add;

#if UNITY_EDITOR
        protected bool HideProperty => _moment == AfflictionMoment.None;

        protected override void OnValidate()
        {
            base.OnValidate();
            if (_moment == AfflictionMoment.Added || _moment == AfflictionMoment.Removed)
            {
                _moment = AfflictionMoment.None;
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }
#endif
    }
}
