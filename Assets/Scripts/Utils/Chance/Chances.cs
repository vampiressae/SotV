using Sirenix.OdinInspector;
using UnityEngine;

public abstract class Chances
{
    [HideLabel, PropertyOrder(1), Range(0, 1)] public float Chance = 1;
}

public abstract class Chances<T> : Chances
{
    [HideLabel, HorizontalGroup("main")] public T Item;
}

public abstract class ChancesWithIntRange<T> : Chances<T>
{
    [InlineProperty]
    [HideLabel, HorizontalGroup("main", 115)] public IntRange Range = IntRange.One;

    public int RangeRandom => Range.RandomUnity;
}

