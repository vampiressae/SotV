using Sirenix.OdinInspector;
using UnityEngine;

public abstract class ScriptableWithNameAndSprite : ScriptableObject
{
    [PreviewField(Alignment = ObjectFieldAlignment.Left,Height = 65)]
    [HorizontalGroup("main", 70), HideLabel] public Sprite Icon;
    [VerticalGroup("main/v"), HideLabel, LabelWidth(40)] public string Name;
    [VerticalGroup("main/v"), HideLabel, TextArea(2, 2)] public string Description;

#if UNITY_EDITOR
    protected virtual void OnValidate() { }
#endif
}
