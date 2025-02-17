using Sirenix.OdinInspector;
using System;
using UnityEngine;

public abstract class ScriptableValue : ScriptableObject 
{
    public event Action OnChangedRaw;
    protected void InvokeOnChangedRaw() => OnChangedRaw?.Invoke();
}

public abstract class ScriptableValue<T> : ScriptableValue
{
    public event Action<T> OnChanged;

    [SerializeField] private T _value;

    [ShowInInspector, HideInEditorMode, ReadOnly]
    public virtual T Value
    {
        get => _value;
        set
        {
            _value = value;
            OnChanged?.Invoke(_value);
            InvokeOnChangedRaw();
        }
    }
}
