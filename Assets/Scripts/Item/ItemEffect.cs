using System.Collections.Generic;
using UnityEngine;
using Inventory;

namespace Items
{
    public abstract class ItemEffect : ScriptableObject
    {
        public virtual void AddedToInventory(InventoryHolder inventory, ItemData item) { }
        public virtual void RemovedFromInventory(InventoryHolder inventory, ItemData item) { }
        public abstract void GetTooltip(ref List<string> list);// { }
    }
}
