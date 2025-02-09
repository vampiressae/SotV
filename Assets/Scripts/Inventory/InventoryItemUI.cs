using Items;

namespace Inventory
{
    public class InventoryItemUI : ItemUI<ItemData, ItemInfo>
    {
        public InventorySlotUI SlotUI => _slotUI as InventorySlotUI;

        protected override string AmountText => "" + (Data.Info.Stack == 1 ? "" : Data.Amount);

        protected override void Refresh()
        {
            if (Data.Info.Rank) _frame.material = Data.Info.Rank.Material;
            base.Refresh();
        }
    }
}
