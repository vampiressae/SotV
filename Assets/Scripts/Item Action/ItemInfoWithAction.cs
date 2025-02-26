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
            var expertise = (int)SkillExpertise.Master;
            if (_useSkill != null) expertise = (int)actor.Info.GetActionAmount(_useSkill);
            for (int i = 0; i < ItemActionsRaw.Length; i++)
                if (expertise >= (int)ItemActionsRaw[i].Expertise)
                    ItemActionsRaw[i].TooltipSummary(actor, ref descriptions);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            foreach (var action in ItemActionsRaw)
                if (action == null) Debug.LogError($"Null <b>ItemAction</b> found on <b>{name}</b>.");
                else action.OnValidate(this);
        }
#endif
    }

    public abstract class ItemInfoWithAction<T> : ItemInfoWithAction, IHasAction where T : ActionBase
    {
        [GUIColor(1, 0.9f, 0.8f)]
        [SerializeField] protected T[] _actions;

        public ActionBase[] Actions => _actions;
        public override ActionBase[] ItemActionsRaw => _actions;
        public void HandleExtendedUI(ItemUI itemUI) => this.HandleExtendedUI(itemUI, ActionsUI);
    }
}
