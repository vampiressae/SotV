
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Entity
{
    public class EntityInfo : ScriptableWithNameAndSprite
    {
        [SerializeField, HideInPlayMode] private List<StatData> _stats;

        [ShowInInspector, HideInEditorMode]
        public List<StatData> Stats { get; private set; }

        public void Init() => Stats = new(_stats);

        private bool GetStatData(StatInfo info, out StatData data)
        {
            data = Stats.Where(stat => stat.Info == info).FirstOrDefault();
            return data != null;
        }

        public int GetStatValue(StatInfo info) => GetStatData(info, out var data) ? data.Amount : 0;

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
    }
}
