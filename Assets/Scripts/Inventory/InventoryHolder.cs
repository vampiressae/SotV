using System;
using System.Collections.Generic;
using UnityEngine;
using Items;
using Actor;

namespace Inventory
{
    public class InventoryHolder : MonoBehaviour
    {
        public event Action OnAnyChanged;

        public ActorHolder Actor;
        public int Slots = 10;
        public List<ItemData> Items = new();

        public void Init(List<ItemData> items, ActorHolder actor = null)
        {
            Actor = actor;
            Items = items;
            Slots = Items.Count;
            foreach (var data in Items)
            {
                data.OnChanged += AnyChanged;
                data.OnItemDataChanging += OnItemDataChanging;
                data.OnItemDataChanged += OnItemDataChanged;

                OnItemDataChanging(data);
                OnItemDataChanged(data);
            }
        }

        private void OnItemDataChanging(ItemData data) { if (data.Info) data.Info.Effects_RemovedFromInventory(this, data); }
        private void OnItemDataChanged(ItemData data) { if (data.Info) data.Info.Effects_AddedToInventory(this, data); }

        public virtual int GetMightCost()
        {
            var cost = 0;
            foreach (var item in Items)
                if (!item.IsEmpty)
                    cost += item.Info.Might * item.Amount;
            return cost;
        }

        private void AnyChanged() => OnAnyChanged?.Invoke();
    }
}
