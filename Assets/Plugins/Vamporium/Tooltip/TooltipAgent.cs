using UnityEngine;
using UnityEngine.EventSystems;

public abstract class TooltipAgent<TTooltip> : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler where TTooltip : Tooltip
{
    protected enum ActivatingMode { None, Hover, Hold, Down, Up }

    [SerializeField] protected ActivatingMode _activate = ActivatingMode.Hover;
    [SerializeField] protected TooltipTrigger _trigger;
    [SerializeField] protected TTooltip _tooltipPrefab;

    protected bool _hovered;

    private void OnDisable() { if (_trigger) _trigger.UnsetCurrent(); }

    private void OnMouseEnter() => Enter();
    private void OnMouseExit() => Exit();
    private void OnMouseDown() => Down();
    private void OnMouseUp() => Up();

    public virtual void OnPointerEnter(PointerEventData e) => Enter();
    public virtual void OnPointerExit(PointerEventData e) => Exit();
    public virtual void OnPointerDown(PointerEventData e) => Down();
    public virtual void OnPointerUp(PointerEventData e) => Up();
    public virtual bool IsEmpty => false;

    protected abstract void Init(TTooltip tooltip);

    protected virtual void Enter()
    {
        if (!enabled || IsEmpty) return;
        if (_activate != ActivatingMode.Hover) return;

        Init(_trigger.SetCurrent(_tooltipPrefab, this));
        _trigger.InvokeOnTooltipShown();
        _hovered = true;
    }

    protected virtual void Exit()
    {
        if (!enabled) return;
        if (_activate != ActivatingMode.Hover) return;

        _trigger.UnsetCurrent();
        _trigger.InvokeOnTooltipShown();
        _hovered = false;
    }

    protected virtual void Down()
    {
        if (!enabled || IsEmpty) return;
        if (_activate != ActivatingMode.Hover) Exit();
        switch (_activate)
        {
            case ActivatingMode.Down:
                if (_trigger.Current && _trigger.Initiator == (object)this) _trigger.UnsetCurrent();
                else Init(_trigger.SetCurrent(_tooltipPrefab, this));
                _trigger.InvokeOnTooltipShown();
                break;

            case ActivatingMode.Hold:
                Init(_trigger.SetCurrent(_tooltipPrefab, this));
                _trigger.InvokeOnTooltipShown();
                break;
        }
    }

    protected virtual void Up()
    {
        if (!enabled) return;
        switch (_activate)
        {
            case ActivatingMode.Up:
                if (_trigger.Current && _trigger.Initiator == (object)this) _trigger.UnsetCurrent();
                else Init(_trigger.SetCurrent(_tooltipPrefab, this));
                _trigger.InvokeOnTooltipShown();
                break;

            case ActivatingMode.Hold:
                _trigger.UnsetCurrent();
                _trigger.InvokeOnTooltipShown();
                break;
        }
    }
}
