using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Actor;

public abstract class ScriptableWithNameAndSpriteAndTooltip : ScriptableWithNameAndSprite
{
    [SerializeField, HorizontalGroup("main/v/h", 50), HideLabel] private Color _tint = Color.white;
    public override Color IconTint => _tint;

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

    public virtual void TooltipInit(ActorHolder actor, TooltipForString tooltip, bool actionSummary)
    {
        var descriptions = new List<string>();
        TooltipDescriptions(actor, ref descriptions);
        if (actionSummary) TooltipActionsSummary(actor, ref descriptions);
        tooltip.Init(Name, string.Join("\n", descriptions), true);
    }

    protected virtual void TooltipDescriptions(ActorHolder actor, ref List<string> descriptions)
    {
        if (!string.IsNullOrEmpty(Description))
            descriptions.Insert(0, Description);
    }

    protected virtual void TooltipActionsSummary(ActorHolder actor, ref List<string> descriptions) { }
}
