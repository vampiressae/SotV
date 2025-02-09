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
}
