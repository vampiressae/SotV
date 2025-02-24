using UnityEngine;
using Entity;
using Actor;
using System.Collections.Generic;

namespace Modifier
{
    [CreateAssetMenu(menuName = "Modifiers/Add Stat")]
    public class ModifierInfoAddStat : ModifierInfoAutoExpire
    {
        [SerializeField] private StatInfo _info;
        [SerializeField] private ExpiringEvent _expire;

        protected override ExpiringEvent _event => _expire;

        public override bool OnEvent(ModifierData data, ModifierEvent e, ActorInfo actor)
        {
            switch (e)
            {
                case ModifierEvent.OnAdded: actor.AddStatValue(this, _info, data.Value); break;
                case ModifierEvent.OnRemoved: actor.RemoveStatValue(this, _info); break;
            }
            return base.OnEvent(data, e, actor);
        }

        public override void GetDescriptions(ModifierData data, ref List<string> descriptions)
        {
            base.GetDescriptions(data, ref descriptions);
            descriptions.Add($"{data.Value.ToStringWithSign().ToValueString()} {_info.Name}" + AppendTimerValue(data));
        }
    }
}
