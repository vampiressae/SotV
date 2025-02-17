using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using Items;

namespace Entity
{
    public class EntityInfo : ScriptableWithNameAndSprite
    {
        [SerializeField, HideInPlayMode] private List<StatData> _stats;

        [SerializeField, HideLabel] private ItemDataWithChanceList _inventorList;
        public List<ItemData> Inventory;

        [ShowInInspector, HideInEditorMode]
        public List<StatData> Stats { get; private set; }

        public virtual void Init()
        {
            Stats = new(_stats);
            InitRandomInventory();
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

        public void InitRandomInventory()
        {
            if (_inventorList == null) return;

            var random = Random.value;
            var list = new List<ItemData>();

            foreach (var item in _inventorList.Items)
            {
                if (item.Chance > random) continue;
                list.Add(new(item.Info, item.Amount.RandomUnity));
            }

            var rnd = new System.Random();
            list.Sort((x, y) => rnd.Next(-1, 1));

            var array = list.ToArray();
            array = array[0.._inventorList.Max.RandomUnity];
            Inventory.Add(array);
        }
    }
}
