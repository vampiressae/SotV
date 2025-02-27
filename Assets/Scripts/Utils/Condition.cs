using System.Collections.Generic;
using UnityEngine;

public enum ConditionResult { None, One, All }

public abstract class Condition : ScriptableObject
{
    public abstract bool Check(object target);
}

public static class ConditionUtils
{
    public static ConditionResult Check(this IEnumerable<Condition> list, object target)
    {
        var ok = 0;

        if (list != null)
            foreach (var condition in list)
                if (condition.Check(target)) ok++;

        return
            ok == 0 ? ConditionResult.None :
            ok == 1 ? ConditionResult.One :
            ConditionResult.All;
    }
}
