using UnityEngine;
using Actor;

namespace Modifier
{
    [CreateAssetMenu(menuName = "Modifiers/Evasion")]
    public class ModifierInfoEvasion : ModifierInfo
    {
        public override bool OnEvent(ModifierEvent e, ActorInfo actor)
        {
            base.OnEvent(e, actor);
            return e == ModifierEvent.OnTurnStarted;
        }

        public override bool OnEvent(ModifierEventInt e, ActorInfo actor, ref int value)
        {
            base.OnEvent(e, actor, ref value);
            if (e == ModifierEventInt.OnDamageReceive)
            {
                value = 0;
                if (GetFlyingText(actor, out var flying))
                    flying.Init(null, Color.clear, "Evasion", Color.green);
            }
            return false;
        }
    }
}