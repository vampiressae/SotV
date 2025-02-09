using Items;

namespace Inventory
{
    public class EquipmentSlotUI : InventorySlotUI
    {
        private EquipmentHolderUI EquipmentUI => InventoryUI as EquipmentHolderUI;

        public override bool CanAcceptItem(Item item)
        {
            if (item is IEquipmentItem onlyOne)
                if (onlyOne.CanEquipOnlyOne)
                    foreach (var data in InventoryUI.Inventory.Items)
                        if (data.Info == item.RawInfo)
                            return false;

            return base.CanAcceptItem(item);
        }

        protected override void OnItemUIRefresh()
        {
            base.OnItemUIRefresh();
            if (EquipmentUI.Blocked && Data.Info is IHasAction hasAction)
                hasAction.HandleExtendedUI(RawItemUI);
        }
    }
}
