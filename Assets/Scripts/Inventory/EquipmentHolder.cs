using System.Collections.Generic;
using Items;

namespace Inventory
{
    public class EquipmentHolder : InventoryHolder
    {
        protected override List<ItemData> GetItems() => Actor.Info.Equipment;

        protected override void InitActorHolder() => Actor.InitEquipment(this);
    }
}
