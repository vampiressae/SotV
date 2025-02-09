using Entity;
using Items;

namespace Inventory
{
    public class InventorySlotUI : SlotUI<ItemData, ItemInfo>, IHasAttachedEntity
    {
        protected InventoryHolderUI _inventoryUI;

        public EntityHolder Entity => InventoryUI.Inventory.Actor;
        public InventoryHolderUI InventoryUI => _inventoryUI;
        public InventoryItemUI ItemUI => _ui as InventoryItemUI;

        protected override ItemUI<ItemData, ItemInfo> Prefab => _inventoryUI.ItemPrefab;

        public void Init(InventoryHolderUI inventoryUI, ItemData data)
        {
            _inventoryUI = inventoryUI;
            Init(data);
        }

        public override void Uninit()
        {
            base.Uninit();
            _inventoryUI = null;
        }
    }
}
