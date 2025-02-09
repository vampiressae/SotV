using System;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Tooltip Trigger")]
public class TooltipTrigger : ScriptableObject
{
    public event Action<Tooltip> OnTooltipShown;

    [SerializeField] private float _fadeDuration = 0.3f;
    [ShowInInspector, HideInEditorMode] private Tooltip _current;

    public float FadeDuration => _fadeDuration;

    private Transform _parent;
    private object _initiator;

    public object Initiator => _initiator;
    public Tooltip Current => _current;

    public void SetParent(Transform parent) => _parent = parent;

    public void UnsetCurrent()
    {
        _initiator = default;
        if (_current) _current.Hide(this);
        _current = null;
    }

    public T SetCurrent<T>(T tooltip, object initiator) where T : Tooltip
    {
        UnsetCurrent();

        if (tooltip == null) return null;

        _initiator = initiator;
        _current = Instantiate(tooltip, _parent);

        if (_current) _current.Show(this);
        return _current as T;
    }

    public void InvokeOnTooltipShown() => OnTooltipShown?.Invoke(_current);
}
