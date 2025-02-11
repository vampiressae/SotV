using Items;
using Inventory;

public abstract class ItemFXEquip : ItemEffect
{
    public override void AddedToInventory(InventoryHolder inventory, ItemData item)
    {
        if (inventory is EquipmentHolder equipment)
            AddedToEquipment(equipment, item);
    }

    public override void RemovedFromInventory(InventoryHolder inventory, ItemData item)
    {
        if (inventory is EquipmentHolder equipment)
            RemovedFromEquipment(equipment, item);
    }

    public abstract void AddedToEquipment(EquipmentHolder equipment, ItemData item);
    public abstract void RemovedFromEquipment(EquipmentHolder equipment, ItemData item);
}
