using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ScriptableList<T> : ScriptableObject
{
    [SerializeField] private List<T> _list;

    public IReadOnlyList<T> List => _list;

    public virtual List<T> Pick(bool shuffle, Func<T, bool> selector = null)
    {
        var result = new List<T>();
        var list = List;

        foreach (var item in list)
            if (selector == null || selector(item))
                result.Add(item);

        if (shuffle)
        {
            var rnd = new System.Random();
            result.Sort((x, y) => rnd.Next(-1, 1));
        }

        return result;
    }
}
