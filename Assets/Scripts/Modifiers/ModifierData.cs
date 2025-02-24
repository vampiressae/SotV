using UnityEngine;
using Actor;
using Sirenix.OdinInspector;

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
            => _timer = _timerMax = timer;

        public bool OnModifierEvent(ModifierEvent e, ActorInfo actor)
        {
            Info.OnEvent(e, actor);
            return Expire();
        }

        private bool Expire()
        {
            if (_timerMax > 0) _timer--;
            return _timerMax > 0 && _timer < 1;
        }

#if UNITY_EDITOR
        [ShowInInspector, ReadOnly, HideLabel,  HorizontalGroup(50)]
        private string _timerAndTimerMax => _timerMax > 0 ? $"{_timer}/{_timerMax}" : "--";
#endif
    }
}