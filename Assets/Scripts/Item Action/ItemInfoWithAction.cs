using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Actor;
using Skills;

namespace Items
{
    public abstract class ItemInfoWithAction : ItemInfo
    {
        [Space]
        [SerializeField] private SkillInfo _useSkill;
        [SerializeField] private ItemInfoWithActionsUI _actionsUI;

        public SkillInfo UseSkill => _useSkill;
        public ItemInfoWithActionsUI ActionsUI => _actionsUI;
        public abstract ActionBase[] ItemActionsRaw { get; }

        protected override void TooltipActionsSummary(ActorHolder actor, ref List<string> descriptions)
        {
            var count = 
                _useSkill == null ? ItemActionsRaw.Length :
                actor.Info.GetSkill(_useSkill, out var skill) ? skill.Amount : 0;

            for (int i = 0; i < Mathf.Min(ItemActionsRaw.Length, count + 1); i++)
                ItemActionsRaw[i].TooltipSummary(actor, ref descriptions);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            foreach (var action in ItemActionsRaw)
                if (action == null)
                    Debug.LogError($"Null <b>ItemAction</b> found on <b>{name}</b>.");
                else
                    action.OnValidate(this);
        }
#endif
    }

    public abstract class ItemInfoWithAction<T> : ItemInfoWithAction, IHasAction where T : ActionBase
    {
        [GUIColor(1, 0.9f, 0.8f)]
        [SerializeField] private T[] _actions;

        public override ActionBase[] ItemActionsRaw => _actions;

        public ActionBase[] Actions => _actions;

        public void HandleExtendedUI(ItemUI itemUI)
            => this.HandleExtendedUI(itemUI, ActionsUI);
    }
}
