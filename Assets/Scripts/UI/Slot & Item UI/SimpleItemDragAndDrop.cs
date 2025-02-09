using Inventory;
using UnityEngine;

namespace Skills
{
    public class SimpleItemDragAndDrop : ItemDragAndDrop
    {
        [SerializeField] private bool _canBeStacked;
        [SerializeField] private bool _interactionBlocked;

        protected override bool CanInfoBeStacked(ScriptableWithNameAndSprite _) => _canBeStacked;
        protected override bool InteractionBlocked(SlotUI _) => _interactionBlocked;
    }
}
