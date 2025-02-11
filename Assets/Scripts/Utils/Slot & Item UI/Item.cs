using System;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class Item
{
    public event Action OnChanged;

    [SerializeField, HideLabel, HorizontalGroup(50)] protected int _amount;

    public abstract bool IsEmpty { get; }
    public abstract bool IsFull { get; }
    public abstract ScriptableWithNameAndSprite RawInfo { get; set; }

    public int Amount => _amount;

    public void Copy(Item data)
    {
        _amount = data._amount;
        RawInfo = data.RawInfo;
        InvokeOnChanged();
    }

    public abstract void Swap(Item with);

    public abstract void InvokeOnItemDataChanging();
    public abstract void InvokeOnItemDataChanged();
    public void InvokeOnChanged() => OnChanged?.Invoke();
}

public abstract class Item<T> : Item where T : ScriptableWithNameAndSprite
{
    [SerializeField, HideLabel, HorizontalGroup] protected T _info;
    public T Info { get => _info; set => SetInfo(value); }

    public override ScriptableWithNameAndSprite RawInfo { get => Info; set => _info = value as T; }

    private void SetInfo(T info)
    {
        if (_info) InvokeOnItemDataChanging();

        _info = info;
        if (info) InvokeOnItemDataChanged();
    }
}
