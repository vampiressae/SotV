using UnityEngine;
using Sirenix.OdinInspector;

public abstract class ScriptableWithNameAndSprite : SerializedScriptableObject
{
    [PreviewField(Alignment = ObjectFieldAlignment.Left,Height = 65)]
    [HorizontalGroup("main", 69, VisibleIf = "ShowMainData"), HideLabel] public Sprite Icon;

    [VerticalGroup("main/v"), HorizontalGroup("main/v/h"), HideLabel, LabelWidth(40)] public string Name;
    [VerticalGroup("main/v"), HideLabel, TextArea(2, 2)] public string Description;

    public virtual Color IconTint => Color.white;

    public bool ShowMainData { get; set; } = true;

#if UNITY_EDITOR
    protected virtual void OnValidate() { }
#endif
}
