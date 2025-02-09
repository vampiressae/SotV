namespace Inventory
{
    public class EquipmentHolder : InventoryHolder
    {
        public override int GetMightCost()
        {
            // will send the equipped cost
            // return base.GetMightCost();

            var cost = 0;
            foreach (var item in Items)
                if (!item.IsEmpty && item.Info is IEquipmentItem equipment)
                    cost += (equipment.EquippedMight < 0 ? item.Info.Might : equipment.EquippedMight) * item.Amount;
            return cost;
        }
    }
}
