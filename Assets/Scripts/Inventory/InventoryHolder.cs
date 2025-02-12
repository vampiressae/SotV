using System;
using System.Collections.Generic;
using UnityEngine;
using Items;
using Actor;
using Sirenix.OdinInspector;

namespace Inventory
{
    public class InventoryHolder : MonoBehaviour
    {
        public event Action OnAnyChanged;

        [ShowInInspector, ReadOnly] public ActorHolder Actor { get; private set; }
        [ShowInInspector, ReadOnly] public int Slots { get; private set; }
        [ShowInInspector, ReadOnly] public List<ItemData> Items { get; private set; }

        protected virtual List<ItemData> GetItems() => Actor.Info.Inventory;

        public void Init(ActorHolder actor)
        {
            Actor = actor;
            Items = GetItems();
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
