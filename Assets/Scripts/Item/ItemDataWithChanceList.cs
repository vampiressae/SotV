using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(menuName = "Scriptable Values/Item Data With Chance List")]
    public class ItemDataWithChanceList : ScriptableObject
    {
        [InlineProperty, LabelWidth(30)] public IntRange Max;
        public List<ItemDataWithChance> Items;
    }
}
