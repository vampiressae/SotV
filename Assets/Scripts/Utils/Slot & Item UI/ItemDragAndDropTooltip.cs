using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Inventory;

public class ItemDragAndDropTooltip : Tooltip<Item>
{
    [Space]
    [SerializeField] private MonoBehaviour _moveUI;
    [SerializeField] private Image _icon;

    private ItemDragAndDrop _ui;
    private Item _data;

    protected override float OverridenFadeDuration => 0.1f;

    public override void Init(Item target)
    {
        _data = target;
        if (_data == null) return;
        _icon.sprite = _data.RawInfo.Icon;
    }

    public void Init(ItemDragAndDrop ui) => _ui = ui;

    public override void Hide(TooltipTrigger trigger = null)
    {
        _moveUI.enabled = false;
        transform.DOMove(_ui.transform.position, 0.1f).onComplete += HideComplete;
    }

    protected override void KillTweens()
    {
        base.KillTweens();
        DOTween.Kill(transform);
    }
}
