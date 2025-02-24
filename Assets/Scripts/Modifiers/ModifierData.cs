using Sirenix.OdinInspector;
using Actor;

namespace Modifier
{
    [System.Serializable]
    public class ModifierData
    {
        [HorizontalGroup, HideLabel] public ModifierInfo Info;
        [HorizontalGroup(60), LabelWidth(10), LabelText("x")] public int Value;

        private int _timerMax;
        private int _timer;

        public ModifierData(ModifierInfo info, int value)
        {
            Info = info;
            Value = value;
        }

        public ModifierData(ModifierInfo info, int value, int timer) : this(info, value)
            => _timer = _timerMax = info.HasTimer ? timer : 0;

        public bool OnModifierEvent(ModifierEvent e, ActorInfo actor)
        { if (Info.OnEvent(e, actor)) return true; return TryExpire(); }

        public bool OnModifierEvent(ModifierEventInt e, ActorInfo actor, ref int value)
        { if (Info.OnEvent(e, actor, ref value)) return true; return TryExpire(); }

        private bool TryExpire()
        {
            if (!Info.HasTimer) return false;
            if (_timerMax > 0) _timer--;
            return _timerMax > 0 && _timer < 1;
        }

#if UNITY_EDITOR
        [ShowInInspector, ReadOnly, HideLabel, HorizontalGroup(50)]
        private string _timerAndTimerMax => _timerMax > 0 ? $"{_timer}/{_timerMax}" : "--";
#endif
    }
}