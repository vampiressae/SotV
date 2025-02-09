using UnityEngine;

public class TooltipAgentForString : TooltipAgent<TooltipForString>
{
    [Space]
    [SerializeField] private bool _divider;
    [SerializeField] protected string _title, _text;

    protected override void Init(TooltipForString tooltip) 
        => tooltip.Init(_title, _text, _divider);

#if UNITY_EDITOR
    protected virtual bool ShowTextFields => true;
#endif
}
