using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class ScriptableWithNameAndSprite : ScriptableObject
{
    [PreviewField(Alignment = ObjectFieldAlignment.Left,Height = 65)]
    [HorizontalGroup("main", 69), HideLabel] public Sprite Icon;
    [VerticalGroup("main/v"), HideLabel, LabelWidth(40)] public string Name;
    [VerticalGroup("main/v"), HideLabel, TextArea(2, 2)] public string Description;

    public string GetTooltip()
    {
        var list = new List<string>();
        GetTooltip(ref list);
        return string.Join("\n", list);
    }

    protected virtual void GetTooltip(ref List<string> list)
    {
        if (!string.IsNullOrEmpty(Description)) list.Add(Description);
    }

#if UNITY_EDITOR
    protected virtual void OnValidate() { }
#endif
}
