using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using Actor;

namespace Affliction
{
    [GUIColor("white")]
    public abstract class AfflictionInfo : ScriptableWithNameAndSpriteAndTooltip, IActorMightMissing
    {
        [SerializeField, HorizontalGroup("color", 150), ColorUsage(false), LabelWidth(69)] private Color _color;
        [SerializeField, HorizontalGroup("color"), HideLabel] private TMP_ColorGradient _gradient;

        [SerializeField, HorizontalGroup("options"), LabelText("Options"), LabelWidth(69)] public AfflictionAddMode AddMode;
        [SerializeField, HorizontalGroup("options"), LabelText("On"), LabelWidth(20)] public AfflictionMoment Moment;

        string IActorMightDictionaryKey.Name => Name;
        Sprite IActorMightDictionaryKeyPlus.Icon => Icon;
        public TMP_ColorGradient Gradient => _gradient;
        public Color Color => _color;

        public bool Afflict(AfflictionMoment moment, ActorInfo actor, AfflictionData data)
        {
            if (data.Info.Moment != moment) return false;

            if (data.ApplyChance > Random.value)
                switch (moment)
                {
                    case AfflictionMoment.Added: AfflictOnAdded(actor, data); break;
                    case AfflictionMoment.Removed: AfflictOnRemoved(actor, data); break;
                    default: Afflict(actor, data); break;
                }
            return data.Expire();
        }

        protected virtual bool AfflictOnAdded(ActorInfo actor, AfflictionData data) => data.Expire();
        protected virtual bool AfflictOnRemoved(ActorInfo actor, AfflictionData data) => data.Expire();

        protected abstract bool Afflict(ActorInfo actor, AfflictionData data);
    }
}
