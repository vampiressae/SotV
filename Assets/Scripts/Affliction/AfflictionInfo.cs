using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using Actor;

namespace Affliction
{
    [GUIColor("white")]
    public abstract class AfflictionInfo : ScriptableWithNameAndSpriteAndTooltip, IActorMightMissing
    {
        public enum AddMode { None, Unique, Stack }

        [SerializeField, HorizontalGroup("color", 150), ColorUsage(false)] private Color _color;
        [SerializeField, HorizontalGroup ("color"), HideLabel] private TMP_ColorGradient _gradient;

        string IActorMightDictionaryKey.Name => Name;
        Sprite IActorMightDictionaryKeyPlus.Icon => Icon;
        public TMP_ColorGradient Gradient => _gradient;
        public Color Color => _color;

        public bool Afflict(AfflictionMoment moment, ActorInfo actor, AfflictionData data)
        {
            if (!data.TryApply()) return false;

            switch (moment)
            {
                case AfflictionMoment.Added: return AfflictOnAdded(actor, data);
                case AfflictionMoment.Removed: return AfflictOnRemoved(actor, data);
            }
            if (data.Moment != moment) return false;
            return Afflict(actor, data) && data.Expire();
        }

        protected virtual bool AfflictOnAdded(ActorInfo actor, AfflictionData data) => data.Expire();
        protected virtual bool AfflictOnRemoved(ActorInfo actor, AfflictionData data) => data.Expire();

        protected abstract bool Afflict(ActorInfo actor, AfflictionData data);
    }
}
