using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Actor;

namespace Modifier
{
    [System.Serializable]
    public class ModifierData
    {
        [HorizontalGroup, HideLabel] public ModifierInfo Info;
        [HorizontalGroup(60), LabelWidth(10), LabelText("x")] public int Value;

        [SerializeField, SuffixLabel("$SuffixCondition")] private ConditionResult _condition;
        [HideIf(nameof(_condition), ConditionResult.None), GUIColor(0.8f, 1, 0.8f)]
        [ListDrawerSettings(CustomRemoveElementFunction = "RemoveCondition", CustomAddFunction = "AddCondition")]
        [SerializeField] private List<ActorCondition> _conditions;

        private int _timerMax;
        private int _timer;

        public bool UsesTimer => _timerMax > 0;
        public ScriptableObject ParentScriptable { get; private set; }

        public ModifierData(ModifierInfo info, int value)
        {
            Info = info;
            Value = value;
        }

        public ModifierData(ModifierInfo info, int value, int timer) : this(info, value)
            => _timer = _timerMax = timer;

        public bool OnModifierEvent(ModifierEvent e, ActorInfo actor)
            => _conditions.Check(actor) == _condition ? Info.OnEvent(this, e, actor) : false;

        public bool OnModifierEvent(ModifierEventInt e, ActorInfo actor, ref int value)
            => _conditions.Check(actor) == _condition ? Info.OnEvent(this, e, actor, ref value) : false;

        public bool Expire()
        {
            if (_timerMax > 0) _timer--;
            return _timerMax > 0 && _timer < 1;
        }

        public string ToTimerString() => $"{_timer}/{_timerMax}";

#if UNITY_EDITOR
        private void AddCondition()
        {
            if (ParentScriptable) _conditions.AddScriptableCopy(ParentScriptable);
            else Debug.LogError("Parent scriptable not found!");
        }

        private void RemoveCondition(ActorCondition item)
        {
            if (ParentScriptable) _conditions.RemoveScriptableCopy(item);
            else Debug.LogError("Parent scriptable not found!");
        }

        private string SuffixCondition => ParentScriptable ? ParentScriptable.name : "";

        [ShowInInspector, ReadOnly, HideLabel, HorizontalGroup(50)]
        private string _timerAndTimerMax => _timerMax > 0 ? this.ToTimerString() : "--";

        public void OnValidate(ScriptableObject parent) => ParentScriptable = parent;
        public void OnUnvalidate(ScriptableObject parent)
        {
            ParentScriptable = null;
            while (_conditions.Count > 0)
                _conditions.RemoveScriptableCopy(_conditions[0]);
        }

        [Title("Editor", TitleAlignment = TitleAlignments.Centered)]
        [Button] private void RemoveAllChildrenScriptables()
        {

        }
#endif
    }
}