using UnityEngine;
using Sirenix.OdinInspector;

namespace Items
{
    public abstract class ItemInfoWithAction : ItemInfo
    {
        public abstract ActionBase[] ItemActionsRaw { get; }

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
        [Space]
        [SerializeField] private ItemInfoWithActionsUI _actionsUI;

        public override ActionBase[] ItemActionsRaw => _actions;

        public ItemInfoWithActionsUI ActionsUI => _actionsUI;
        public ActionBase[] Actions => _actions;

        public void HandleExtendedUI(ItemUI itemUI)
            => this.HandleExtendedUI(itemUI, _actionsUI);
    }
}
