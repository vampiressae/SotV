using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Items;

namespace Entity
{
    public class EntityInfo : ScriptableWithNameAndSprite
    {
        public event Func<FlyingText> OnFlyingText;

        [SerializeField, HideInPlayMode] private List<StatData> _stats;

        [BoxGroup("Inventories"), SerializeField, HideLabel] private ItemDataWithChanceList _inventorList;
        [BoxGroup("Inventories"), SerializeField, LabelText("Inventory")] public List<ItemData> _inventoryStart;
        [ShowInInspector, HideInEditorMode] public List<StatData> Stats { get; private set; }

        protected List<ItemData> _inventory;
        public List<ItemData> Inventory => _inventory ??= InitInventory();

        public virtual void Init()
        {
            Stats = new(_stats);
        }

        private bool GetStatData(StatInfo info, out StatData data)
        {
            data = Stats.Where(stat => stat.Info == info).FirstOrDefault();
            return data != null;
        }

        public int GetStatValue(StatInfo info) => GetStatData(info, out var data) ? data.Amount : 0;
        public float GetInfluencedStatValue(StatInfo info) => GetStatData(info, out var data) ? data.Amount * info.Influence : 0;

        public void AddStatValue(StatInfo info, int value)
        {
            if (!GetStatData(info, out var data))
            {
                data = new StatData(info);
                Stats.Add(data);
            }
            data.AddAmount(value);
        }

        public void AddStatValue(object source, StatInfo info, int value)
        {
            var old = Stats.Where(stat => stat.Source == source && stat.Info == info).FirstOrDefault();
            if (old == null)
            {
                old = new StatData(source, info, value);
                Stats.Add(old);
            }
            else old.SetAmount(value);
        }

        public void RemoveStatValue(object source, StatInfo info)
        {
            var old = Stats.Where(stat => stat.Source == source && stat.Info == info).FirstOrDefault();
            if (old == null) return;
            Stats.Remove(old);
        }

        protected virtual List<ItemData> InitInventory()
        {
            _inventory = new() { _inventoryStart };
            if (_inventorList) _inventorList.PickWithChance(_inventory);
            return _inventory;
        }

        public virtual List<ItemData> GetLoot() => Inventory;

        public FlyingText InvokeOnFlyingText() => OnFlyingText?.Invoke();
    }
}
