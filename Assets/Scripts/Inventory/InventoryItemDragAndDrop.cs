using Items;

namespace Inventory
{
    public class InventoryItemDragAndDrop : ItemDragAndDrop
    {
        protected override bool CanInfoBeStacked(ScriptableWithNameAndSprite info) => (info as ItemInfo).Stack != 1;
        protected override bool InteractionBlocked(SlotUI ui) => (ui as InventorySlotUI).InventoryUI.Blocked;
    }
}