using System.Collections.Generic;
using UnityEngine;
using Items;
using Actor;

namespace Skills
{
    [CreateAssetMenu(menuName = "Skill/Info")]
    public class SkillInfo : ScriptableWithNameAndSpriteAndTooltip, IHasAction
    {
        [SerializeField] private SkillAction[] _actions;
        [Space]
        [SerializeField] private ItemInfoWithActionsUI _actionsUI;

        public ItemInfoWithActionsUI ActionsUI => _actionsUI;
        ActionBase[] IHasAction.Actions => _actions;

        public void HandleExtendedUI(ItemUI itemUI) 
            => this.HandleExtendedUI(itemUI, _actionsUI);

        protected override void TooltipActionsSummary(ActorHolder actor, ref List<string> descriptions)
        {
            var count = _actions.Length;

            for (int i = 0; i < Mathf.Min(_actions.Length, count + 1); i++)
                _actions[i].TooltipSummary(actor, ref descriptions);
        }
    }
}