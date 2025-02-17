using Sirenix.OdinInspector;
using UnityEngine;

namespace Items
{
   // [LabelWidth(100)]
    [System.Serializable]
    public class ItemDataWithChance
    {
        [HideLabel, HorizontalGroup("1")] public ItemInfo Info;
        [HideLabel, HorizontalGroup("1", 115), InlineProperty] public IntRange Amount;
        [HideLabel, Range(0, 1)] public float Chance = 1;
    }
}