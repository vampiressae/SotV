using UnityEngine;

public abstract class TooltipAgentForObject<TTarget, TTooltip> : TooltipAgent<TTooltip> where TTarget : Object where TTooltip : Tooltip<TTarget>
{
    [Space]
    [SerializeField] private TTarget _component;

    protected override void Init(TTooltip tooltip) => tooltip.Init(_component);

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_component == null) _component = GetComponent<TTarget>();
    }
#endif
}

