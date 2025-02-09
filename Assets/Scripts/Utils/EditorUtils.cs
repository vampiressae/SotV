#if UNITY_EDITOR
using System.Linq;
using UnityEngine;
using UnityEditor;
using Sirenix.Utilities;

public static class EditorUtils
{
    public static T[] LoadAssets<T>(string search, bool distinct = true, string[] folders = null) where T : Object
    {
        string[] guids = null;

        if (folders.IsNullOrEmpty()) guids = AssetDatabase.FindAssets(search);
        else guids = AssetDatabase.FindAssets(search, folders);

        int length = guids.Length;
        var array = new T[length];
        for (int i = 0; i < length; i++)
        {
            var path = AssetDatabase.GUIDToAssetPath(guids[i]);
            array[i] = AssetDatabase.LoadAssetAtPath<T>(path);
        }

        if (distinct) array = array.Distinct().ToArray();
        return array;
    }

    public static T[] LoadAssetsSafe<T>(string search = "", bool distinct = true, string[] folders = null) where T : UnityEngine.Object
        => LoadAssets<T>($"{search} t: {typeof(T)}", distinct, folders);

    public static T LoadAssetsSafeFirst<T>(string search = "", bool precise = false, string[] folders = null) where T : UnityEngine.Object
    {
        var type = $"{typeof(T)}";
        type = type.Substring(type.LastIndexOf('.') + 1);

        var assets = LoadAssets<T>($"{search} t: {type}", folders: folders);

        if (assets.IsNullOrEmpty()) return null;
        if (!precise) return assets[0];

        search = search.ToLower();
        for (int i = 0; i < assets.Length; i++)
            if (assets[i].name.ToLower() == search)
                return assets[i];
        return null;
    }

    public static string GetAssetPath(this ScriptableObject scriptable)
    {
        var path = AssetDatabase.GetAssetPath(scriptable);
        var length = path.Length - (scriptable.name + ".asset").Length;
        return path.Substring(0, length);
    }
}
#endif