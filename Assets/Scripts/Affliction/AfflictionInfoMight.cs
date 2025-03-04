using Sirenix.OdinInspector;
using UnityEngine;
using Actor;

using static Actor.ActorMight;
using Sirenix.Serialization;

namespace Affliction
{
    [CreateAssetMenu(menuName = "Afflictions/Might")]
    public class AfflictionInfoMight : AfflictionInfo
    {
        private enum DamageKey { Default, Myself, Custom }

        [SerializeField, HorizontalGroup("might"), LabelWidth(69)] private MightTypeEffect _might;
        [SerializeField, HorizontalGroup("key"), LabelWidth(69), LabelText("Key")] private DamageKey _damageKey = DamageKey.Myself;
        [ShowIf(nameof(_damageKey), DamageKey.Custom)]
        [SerializeField, HorizontalGroup("key"), HideLabel] private IActorMightMissing _key;

        protected override bool Afflict(ActorInfo actor, AfflictionData data)
        {
            var key = _damageKey == default ? null : _damageKey == DamageKey.Myself ? this : _key;
            actor.Might.AddMissingValue(data.Value, this, key);
            return true;
        }
    }
}
