using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Items;
using Actor;

namespace Inventory
{
    public class InventoryHolder : MonoBehaviour
    {
        public event Action OnAnyChanged;

        [ShowInInspector, ReadOnly, HideInEditorMode] public ActorHolder Actor { get; private set; }
        [ShowInInspector, ReadOnly, HideInEditorMode] public int Slots { get; private set; }
        [ShowInInspector, ReadOnly, HideInEditorMode] public List<ItemData> Items { get; private set; }

        protected virtual List<ItemData> GetItems() => Actor.Info.Inventory;

        public void Init(ActorHolder actor)
        {
            Actor = actor;
            Init(GetItems());
            InitActorHolder();
        }

        public void Init(List<ItemData> items)
        { 
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

        protected virtual void InitActorHolder() => Actor.InitInventory(this);

        private void OnItemDataChanging(ItemData data) { if (data.Info) data.Info.Effects_RemovedFromInventory(this, data); }
        private void OnItemDataChanged(ItemData data) { if (data.Info) data.Info.Effects_AddedToInventory(this, data); }

        private void AnyChanged() => OnAnyChanged?.Invoke();
    }
}
