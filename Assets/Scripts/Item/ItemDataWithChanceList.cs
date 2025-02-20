using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(menuName = "Scriptable Values/Item Data With Chance List")]
    public class ItemDataWithChanceList : ScriptableListWithIntRangeChance<ItemDataWithChances>
    {
        public override IntRange Range => _rangeOverride;

        private IntRange _rangeOverride;

        public void PickWithChance(List<ItemData> inventory, bool fillEmpty = false, bool shuffle = true)
        {
            _rangeOverride = _range;

            var max = _range.Max - inventory.Count;
            if (max < 0) return;

            if (_range.Min > max) _rangeOverride.Min = max;
            if (_range.Max > max) _rangeOverride.Max = max;

            PickWithChance(shuffle).ForEach(item => inventory.Add(new(item.Item, item.RangeRandom)));
            if (!fillEmpty) return;

            for (int i = 0; i < _rangeOverride.Max - inventory.Count; i++)
                inventory.Add(new());
        }
    }
}
