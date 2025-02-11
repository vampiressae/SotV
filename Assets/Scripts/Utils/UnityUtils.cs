using System;
using UnityEngine;

public static class UnityUtils
{
    public static void Clear(this Transform transform, Func<Transform, bool> filter = null)
    {
        foreach (Transform t in transform)
            if (filter == null || filter(t))
                UnityEngine.Object.Destroy(t.gameObject);
    }

    public static T Clone<T>(this T clonable) where T : class
        => JsonUtility.FromJson<T>(JsonUtility.ToJson(clonable));

    public static GameObject GetPhysicsGameObject(this Collider collider)
        => collider.attachedRigidbody ? collider.attachedRigidbody.gameObject : collider.gameObject;

    public static string ToLabelAndValue(this string label, int value, bool sign = true)
        => label.ToLabelAndValue(sign ? value.ToStringWithSign() : value.ToString());

    public static string ToLabelAndValue(this string label, string value)
        => $"{value} {LabelColor}{label}</color>";

    public static string ToInitialValueString(this int value, bool sign = true)
        => (sign ? value.ToStringWithSign() : value.ToString()).ToInitialValueString();

    public static string ToInitialValueString(this string value)
        => $"{LabelColor}({value})</color>";

    public static string LabelColor => "<color=#ffffff55>";
}
