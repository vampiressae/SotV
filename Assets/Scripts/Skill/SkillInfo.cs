using Items;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(menuName = "Skill/Info")]
    public class SkillInfo : ScriptableWithNameAndSprite, IHasAction
    {
        [SerializeField] private SkillAction[] _actions;
        [Space]
        [SerializeField] private ItemInfoWithActionsUI _actionsUI;

        public ItemInfoWithActionsUI ActionsUI => _actionsUI;
        ActionBase[] IHasAction.Actions => _actions;

        public void HandleExtendedUI(ItemUI itemUI) 
            => this.HandleExtendedUI(itemUI, _actionsUI);
    }
}