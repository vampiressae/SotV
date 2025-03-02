using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtils
{
    public static bool IsInProximity(this Vector3 point, GameObject go, float distance)
    {
        var colliders = Physics.OverlapSphere(point, distance);
        if (colliders.Length == 0) return false;

        return colliders.Where(collider => collider.GetPhysicsGameObject() == go).FirstOrDefault();
    }

    public static T GetClosest<T>(this IEnumerable<T> list, Vector3 origin, out float closest, Func<T, bool> filter = null) where T : Component
    {
        closest = float.MaxValue;
        T result = null;

        foreach (var component in list)
        {
            if (filter != null && !filter(component)) continue;
            var distance = Vector3.Distance(component.transform.position, origin);
            if (distance < closest)
            {
                closest = distance;
                result = component;
            }
        }
        return result;
    }

    public static string ShortenedValue(this int value)
    {
        var text = value.ToString();
        var length = text.Count();

        if (length > 6) return text.Substring(0, length - 6) + "M";
        else if (length > 3) return text.Substring(0, length - 3) + "K";

        return text;
    }

    public static string ToStringWithSign(this int value, bool zeroIsNegative = false)
        => (value == 0 && zeroIsNegative ? "-" : value < 0 ? string.Empty : "+") + value;
}
