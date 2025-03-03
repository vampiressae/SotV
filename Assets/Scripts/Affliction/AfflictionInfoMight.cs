using Sirenix.OdinInspector;
using UnityEngine;
using Actor;

using static Actor.ActorMight;

namespace Affliction
{
    [CreateAssetMenu(menuName = "Afflictions/Might")]
    public class AfflictionInfoMight : AfflictionInfo
    {
        [SerializeField, HorizontalGroup("might"), LabelWidth(50)] private MightTypeEffect _might;
        [SerializeField, HorizontalGroup("might", 40), HideLabel] private int _value;

        protected override bool Afflict(ActorInfo actor, AfflictionData data)
        {
            actor.Might.AddMissingValue(_value, this);
            return true;
        }
    }
}
