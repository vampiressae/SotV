#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Sirenix.Utilities;
using Sirenix.OdinInspector.Editor;

public static class EditorUtils
{
    public static T[] LoadAssets<T>(string search, bool distinct = true, string[] folders = null) where T : Object
    {
        string[] guids;
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

    public static void AddScriptableCopy<T>(this List<T> list, ScriptableObject scriptable = null) where T : ScriptableObject
    {
        var type = typeof(T);
        var prefix = $"__";
        var guids = AssetDatabase.FindAssets("t:" + type);
        var items = new List<T>();

        for (int i = 0; i < guids.Length; i++)
        {
            var path = AssetDatabase.GUIDToAssetPath(guids[i]);
            items.Add(AssetDatabase.LoadAssetAtPath<T>(path));
        }
        items = items.Where(item => !item.name.StartsWith(prefix)).ToList();

        if (items.Count == 0)
        {
            Debug.LogError($"No items found in the project with type {typeof(T)}.");
            return;
        }

        var selector = new GenericSelector<T>("Select", false, selected => selected.name, items);
        selector.EnableSingleClickToSelect();
        selector.SelectionConfirmed += selection =>
        {
            var selected = selection.FirstOrDefault();
            if (selected)
            {
                var item = Object.Instantiate(selected);
                if (scriptable)
                {
                    item.name = prefix + selected.name;
                    AssetDatabase.AddObjectToAsset(item, scriptable);
                }
                list.Add(item);
            }
        };
        selector.ShowInPopup();
    }

    public static void RemoveScriptableCopy<T>(this List<T> list, T remove, bool clear = true) where T : ScriptableObject
    {
        if (clear) ClearScriptableCopies(list);
        if (remove == null) return;
        list.Remove(remove);
        AssetDatabase.RemoveObjectFromAsset(remove);
    }

    public static void ClearScriptableCopies<T>(this List<T> list) where T : ScriptableObject
    {
        for (int i = list.Count - 1; i >= 0; i--)
            if (list[i] == null)
                list.RemoveAt(i);
    }

    public static void DrawTintedSprite(Sprite sprite, Color color)
    {
        var padding = 10;
        var rect = GUILayoutUtility.GetLastRect();
        rect.x += padding;
        rect.y += padding;
        rect.width -= padding * 2 + 4;
        rect.height -= padding * 2;

        GUI.color = color;
        DrawTextureGUI(rect, sprite);
        GUI.color = Color.white;
    }

    public static void DrawTextureGUI(Rect position, Sprite sprite)
         => DrawTextureGUI(position, sprite, new Vector2(position.width, position.height), Vector2.zero);

    public static void DrawTextureGUI(Rect position, Sprite sprite, Vector2 size, Vector2 offset)
    {
        if (sprite == null) return;

        var rect = sprite.rect;
        var tex = sprite.texture;
        var trueSize = size;

        var spriteRect = new Rect(rect.x / tex.width, rect.y / tex.height, rect.width / tex.width, rect.height / tex.height);
        trueSize.y *= (sprite.rect.height / sprite.rect.width);
        position = new Rect(position.x + offset.x, position.y + offset.y + (size.y - trueSize.y) / 2, trueSize.x, trueSize.y);

        GUI.DrawTextureWithTexCoords(position, tex, spriteRect);
    }

}
#endif