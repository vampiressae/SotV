using System;
using UnityEngine;
using Sirenix.OdinInspector;
using Actor;

public abstract class Item
{
    public event Action OnChanged;

    [SerializeField, HideLabel, HorizontalGroup(50)] protected int _amount;

    public abstract bool IsEmpty { get; }
    public abstract bool IsFull { get; }
    public abstract ScriptableWithNameAndSpriteAndTooltip RawInfo { get; set; }

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

    public virtual void TooltipInit(ActorHolder actor, TooltipForString tooltip, bool actionSummary) 
        => RawInfo.TooltipInit(actor, tooltip, actionSummary);
}

public abstract class Item<T> : Item where T : ScriptableWithNameAndSpriteAndTooltip
{
    [SerializeField, HideLabel, HorizontalGroup] protected T _info;
    public T Info { get => _info; set => SetInfo(value); }

    public override ScriptableWithNameAndSpriteAndTooltip RawInfo { get => Info; set => _info = value as T; }

    private void SetInfo(T info)
    {
        if (_info) InvokeOnItemDataChanging();

        _info = info;
        if (info) InvokeOnItemDataChanged();
    }
}
