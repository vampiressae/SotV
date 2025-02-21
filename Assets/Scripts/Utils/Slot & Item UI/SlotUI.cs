using System;
using Inventory;

public abstract class SlotUI : TooltipAgent<TooltipForString>, ITooltipString
{
    public event Action OnChanged;

    public abstract Item RawData { get; set; }
    public abstract ItemUI RawItemUI { get; }

    public override bool IsEmpty => RawData == null || RawData.IsEmpty;

    public virtual string Title => !IsEmpty && RawData.RawInfo ? RawData.RawInfo.Name : string.Empty;
    public virtual string Description => !IsEmpty && RawData.RawInfo ? RawData.RawInfo.GetTooltip() : string.Empty;
    public virtual bool Divider => !string.IsNullOrEmpty(Description);

    protected void Init(Item data)
    {
        RawData = data;
        RawData.OnChanged += OnDataChanged;
        OnDataChanged();
    }

    public virtual void Uninit()
    {
        RawData.OnChanged -= OnDataChanged;
        RawData = null;
    }

    public abstract void OnDataChanged();
    public virtual bool CanAcceptItem(Item item) => true;

    protected virtual void OnItemUIRefresh() { }
    public void InvokeOnChanged() => OnChanged.Invoke();
}

public abstract class SlotUI<TItem, TInfo> : SlotUI where TItem : Item<TInfo> where TInfo : ScriptableWithNameAndSpriteAndTooltip
{
    public override Item RawData { get => Data; set => Data = value as TItem; }
    public override ItemUI RawItemUI => _ui;

    public TItem Data { get; protected set; }

    protected ItemUI<TItem, TInfo> _ui;

    protected abstract ItemUI<TItem, TInfo> Prefab { get; }

    public override void OnDataChanged()
    {
        if (_ui)
        {
            _ui.OnRefresh -= OnItemUIRefresh;
            _ui.Uninit();
        }

        if (!Data.IsEmpty)
        {
            _ui = Instantiate(Prefab, transform);
            _ui.Init(this);
            _ui.OnRefresh -= OnItemUIRefresh;
            _ui.OnRefresh += OnItemUIRefresh;
        }

        InvokeOnChanged();
    }
}
