using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class ItemUI : MonoBehaviour
{
    public event Action OnRefresh;

    [SerializeField] protected Image _icon;
    [SerializeField] protected TMP_Text _amount;
    [SerializeField] protected Image _frame;

    public abstract Item RawData { get;  }
    public abstract SlotUI RawSlotUI { get; }
    protected virtual string AmountText => string.Empty;

    protected virtual void Refresh()
    {
        _icon.sprite = RawData.RawInfo.Icon;
        _amount.text = AmountText;
        OnRefresh?.Invoke();
    }
}

public abstract class ItemUI<TItem, TInfo> : ItemUI where TItem : Item<TInfo> where TInfo : ScriptableWithNameAndSprite
{
    protected SlotUI<TItem, TInfo> _slotUI;

    public TItem Data => _slotUI.Data;

    public override Item RawData => Data;
    public override SlotUI RawSlotUI => _slotUI;

    public virtual void Init(SlotUI<TItem, TInfo> slotUI)
    {
        _slotUI = slotUI;
        _slotUI.OnChanged += Refresh;
        Refresh();
    }

    public virtual void Uninit()
    {
        if (_slotUI) _slotUI.OnChanged -= Refresh;
        Destroy(gameObject);
    }
}
