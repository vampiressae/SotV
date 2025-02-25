using System.Collections.Generic;
using UnityEngine;

public enum ConditionResult { None, One, All }

public abstract class Condition : ScriptableObject
{
}

public abstract class Condition<T> : Condition
{
    public abstract bool Check(T target);
}

public static class ConditionUtils
{
    public static ConditionResult Check<T>(this IEnumerable<Condition<T>> list, T target)
    {
        var ok = 0;

        foreach (var condition in list)
            if (condition.Check(target)) ok++;

        return
            ok == 0 ? ConditionResult.None :
            ok == 1 ? ConditionResult.One :
            ConditionResult.All;
    }
}
