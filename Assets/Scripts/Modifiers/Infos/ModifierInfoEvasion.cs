using System.Collections.Generic;
using UnityEngine;
using Actor;

namespace Modifier
{
    [CreateAssetMenu(menuName = "Modifiers/Evasion")]
    public class ModifierInfoEvasion : ModifierInfoAutoExpireOnTurnStart
    {
        public override bool OnEvent(ModifierData data, ModifierEventInt e, ActorInfo actor, ref int value)
        {
            if (e == ModifierEventInt.OnDamageReceive && Random.Range(0, 101) <= data.Value)
            {
                value = 0;
                if (GetFlyingText(actor, out var flying))
                    flying.Init(null, Color.clear, "Evasion", Color.green);
            }
            return base.OnEvent(data, e, actor, ref value);
        }

        public override void GetDescriptions(ModifierData data, ref List<string> descriptions)
        {
            base.GetDescriptions(data, ref descriptions);
            descriptions.Add(data.Value + "% chance for " + "Evasion".ToValueString() + AppendTimerValue(data));
        }
    }
}