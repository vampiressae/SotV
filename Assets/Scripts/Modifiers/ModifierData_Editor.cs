#if UNITY_EDITOR
using UnityEngine;
using Sirenix.OdinInspector;
using Actor;

namespace Modifier
{
    public partial class ModifierData
    {
        private void AddCondition()
        {
            if (ParentScriptable) _conditions.AddScriptableCopy(ParentScriptable);
            else Debug.LogError("Parent scriptable not found!");
        }

        private void RemoveCondition(Condition item)
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
    }
}
#endif
