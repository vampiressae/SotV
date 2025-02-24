using Actor;
using System.Collections.Generic;

namespace Modifier
{
    public abstract class ModifierInfo : ScriptableWithNameAndSprite
    {
        public virtual bool OnEvent(ModifierData data, ModifierEvent e, ActorInfo actor)
        {
            UnityEngine.Debug.Log($"OnEvent : {GetType().Name} : <b>{e}</b>");
            return false;
        }

        public virtual bool OnEvent(ModifierData data, ModifierEventInt e, ActorInfo actor, ref int value)
        {
            UnityEngine.Debug.Log($"OnEvent : {GetType().Name} : <b>{e}</b>");
            return false;
        }

        protected bool GetFlyingText(ActorInfo actor, out FlyingText flying)
        {
            flying = actor.InvokeOnFlyingText();
            return flying;
        }

        public virtual void GetDescriptions(ModifierData data, ref List<string> descriptions) { }

        protected string AppendTimerValue(ModifierData data) 
            => data.UsesTimer ? " for " + data.ToTimerString().ToValueString() + " turns" : string.Empty;
    }

    public abstract class ModifierInfoAutoExpire : ModifierInfo
    {
        protected enum ExpiringEvent { OnTurnStarted, OnTurnEnded }
        protected abstract ExpiringEvent _event { get; }

        public override bool OnEvent(ModifierData data, ModifierEvent e, ActorInfo actor)
        {
            switch(e)
            {
                case ModifierEvent.OnTurnStarted:
                    if (_event == ExpiringEvent.OnTurnStarted)
                        if (data.Expire())
                            return true;
                    break;

                case ModifierEvent.OnTurnEnded:
                    if (_event == ExpiringEvent.OnTurnEnded)
                        if (data.Expire())
                            return true;
                    break;
            }
            return base.OnEvent(data, e, actor);
        }
    }

    public abstract class ModifierInfoAutoExpireOnTurnStart : ModifierInfoAutoExpire
    { protected override ExpiringEvent _event =>  ExpiringEvent.OnTurnStarted; }

    public abstract class ModifierInfoAutoExpireOnTurnEnd : ModifierInfoAutoExpire
    { protected override ExpiringEvent _event => ExpiringEvent.OnTurnEnded; }
}