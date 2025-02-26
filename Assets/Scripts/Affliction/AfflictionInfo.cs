using UnityEngine;
using Sirenix.OdinInspector;
using Actor;

namespace Affliction
{
    public abstract class AfflictionInfo : ScriptableWithNameAndSpriteAndTooltip
    {
        [SerializeField, LabelWidth(70), HorizontalGroup("1")] private AfflictionMoment _moment;
        [HideIf(nameof(_moment), AfflictionMoment.None), HideLabel]
        [SerializeField, LabelWidth(70), HorizontalGroup("1", 50)] private int _applications;
        [SerializeField, LabelWidth(70)] private int _might;

        public bool Afflict(AfflictionMoment time, ActorInfo actor)
        {
            actor.Might.AddMissingValue(_might);
            actor.InvokeOnFlyingText().Init(null, Color.clear, _might, Color.green);

            _applications--;
           return _applications < 1;
        }
    }
}
