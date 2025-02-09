using UnityEngine;

public abstract class TooltipForComponent : TooltipForObject<Component> 
{
    public Component Component { get; protected set; }
}

public abstract class TooltipForComponent<TComponent> : TooltipForComponent
{
    public TComponent Target { get; private set; }

    public override void Init(Component target)
    {
        if (target is TComponent component)
        {
            Component = target;
            Target = component;
            Init(component);
        }
    }

    protected abstract void Init(TComponent component);
}
