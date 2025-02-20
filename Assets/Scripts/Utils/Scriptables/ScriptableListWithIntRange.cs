using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class ScriptableListWithIntRange<T> : ScriptableList<T>
{
    [InlineProperty, PropertyOrder(-1)]
    [SerializeField, LabelWidth(50)] protected IntRange _range;

    public virtual IntRange Range => _range;
    public int RangeRandom => Range.RandomUnity;

    public override List<T> Pick(bool shuffle, Func<T, bool> selector = null)
    {
        var picked = base.Pick(shuffle, selector);
        return picked.GetRange(0, Mathf.Min(RangeRandom, picked.Count));
    }
}
