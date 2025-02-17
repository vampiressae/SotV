using UnityEngine;
using DG.Tweening;
using Entity;

public class ActorHolderSelector : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Color _blurred = Color.gray;
    [SerializeField] private EntityValue _entityValue;
    [SerializeField] private ActionBaseInfo _actionBaseInfo;

    private bool _selected;

    private void OnEnable()
    {
        _entityValue.OnChanged += OnEntityValueChanged;
        _actionBaseInfo.OnChanged += OnActionBaseInfoChanged;
    }

    private void OnDisable()
    {
        _entityValue.OnChanged -= OnEntityValueChanged;
        _actionBaseInfo.OnChanged -= OnActionBaseInfoChanged;
    }

    private void OnEntityValueChanged(EntityHolder entity)
    {
        var selected = entity == null || entity.gameObject == gameObject || _actionBaseInfo.Value == null;
        if (_selected == selected) return;

        _selected = selected;
        _renderer.DOKill();
        _renderer.DOColor(_selected ? Color.white : _blurred, 0.3f);
    }

    private void OnActionBaseInfoChanged(Items.ActionBase _)
    {
        OnEntityValueChanged(_entityValue.Value);
    }
}
