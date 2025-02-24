using System;
using System.Collections.Generic;
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
        => $"{value.ToValueString()} {label}</color>";

    public static string ToInitialValueString(this int value, bool sign = true)
        => (sign ? value.ToStringWithSign() : value.ToString());

    public static string ToValueString(this IntRange value) => ValueColorOpen + value + ValueColorClose;
    public static string ToValueString(this int value) => ValueColorOpen + value + ValueColorClose;
    public static string ToValueString(this string text) => ValueColorOpen + text + ValueColorClose;
    public static string ValueColorOpen => "<color=#ffffffff><b>";
    public static string ValueColorClose => "</b></color>";

    public static void FromSprite(this PolygonCollider2D polygonCollider2D, Sprite sprite, float tolerance = 0.05f)
    {
        List<Vector2> points = new List<Vector2>();
        List<Vector2> simplifiedPoints = new List<Vector2>();

        polygonCollider2D.pathCount = sprite.GetPhysicsShapeCount();
        for (int i = 0; i < polygonCollider2D.pathCount; i++)
        {
            sprite.GetPhysicsShape(i, points);
            LineUtility.Simplify(points, tolerance, simplifiedPoints);
            polygonCollider2D.SetPath(i, simplifiedPoints);
        }
    }
}
