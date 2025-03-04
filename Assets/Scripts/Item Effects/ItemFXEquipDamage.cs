using Inventory;
using Items;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Effects/Damage")]
public class ItemFXEquipDamage : ItemFXEquip
{
    public override void AddedToEquipment(EquipmentHolder equipment, ItemData item)
    {
    }

    public override void RemovedFromEquipment(EquipmentHolder equipment, ItemData item)
    {
    }
}
