using Actor;

namespace Modifier
{
    public abstract class ModifierInfo : ScriptableWithNameAndSpriteAndTooltip
    {
        public bool HasTimer => false;

        public virtual bool OnEvent(ModifierEvent e, ActorInfo actor)
        {
            UnityEngine.Debug.Log($"OnEvent : {GetType().Name} : <b>{e}</b>");
            return false;
        }

        public virtual bool OnEvent(ModifierEventInt e, ActorInfo actor, ref int value)
        {
            UnityEngine.Debug.Log($"OnEvent : {GetType().Name} : <b>{e}</b>");
            return false;
        }

        protected bool GetFlyingText(ActorInfo actor, out FlyingText flying)
        {
            flying = actor.InvokeOnFlyingText();
            return flying;
        }
    }
}