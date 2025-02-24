using UnityEngine;
using Actor;

namespace Modifier
{
    [CreateAssetMenu(menuName = "Modifiers/Evasion")]
    public class ModifierInfoEvasion : ModifierInfo
    {
        public override void OnEvent(ModifierEvent e, ActorInfo actor)
        {
            Debug.Log("OnEvent : " + GetType().Name + " : " + e);
        }
    }
}