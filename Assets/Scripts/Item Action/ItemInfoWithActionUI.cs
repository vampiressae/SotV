using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Entity;
using Actor;

namespace Items
{
    public class ItemInfoWithActionUI : TooltipAgent<TooltipForString>, IActorMightPreviewer
    {
        [Space]
        [SerializeField] private Image _icon;
        [SerializeField] private Image _frame;
        [SerializeField] private GameObject _unavailable;
        [Space]
        [SerializeField] private Canvas _canvas;
        [SerializeField] private MonoBehaviour _moveUI;
        [Space]
        [SerializeField] private RoundsPerTurnValue _rounds;
        [SerializeField] private IndicatorEventTrigger _mouseUpEvent;
        [SerializeField] private EntityValue _hoveredEntity;
        [SerializeField] private ActionBaseInfo _actionBaseInfo;

        private ItemUI _ui;
        private ActionBase _action;
        private ActorHolder _actor;
        private bool _dragged;
        private bool _available;

        private bool Targetless => _action is ActionWithInfo awi && awi.RawInfo.Target == ActionTarget.None;
        private int ScaledMight => _actor ? _rounds.GetMightValue(_actor, _action.Might) : _action.Might;
        public string Name => _action != null ? _action.Name : "[NO ACTION FOUND]";

        private void OnDestroy() => Uninit();

        public void Init(ActionBase action, ItemUI ui)
        {
            _ui = ui;
            _actor = _ui.RawSlotUI is IHasAttachedEntity entity ? entity.Entity as ActorHolder : null;

            _action = action;
            _icon.sprite = action.Icon;
            _icon.transform.localPosition = default;
            if (_action.Rank) _frame.material = _action.Rank.Material;

            _mouseUpEvent.OnTrigger += OnMouseUpTrigger;

            if (_actor) _actor.Info.Might.OnAnyValueChanged += Refresh;

            Refresh();
        }

        protected override void Init(TooltipForString tooltip) 
            => _action.TooltipInit(_actor, tooltip);

        public void Uninit()
        {
            _mouseUpEvent.OnTrigger -= OnMouseUpTrigger;
            if (_actor) _actor.Info.Might.OnAnyValueChanged -= Refresh;
        }

        private void Refresh()
        {
            if (_actor == null) return;
            _available = ScaledMight <= _actor.Info.Might.Available;
            _unavailable.SetActive(!_available);
        }

        protected override void Enter()
        {
            base.Enter();
            if (_actor == null) return;
            _actor.Info.Might.AddPreviewValue(this, ScaledMight);
        }

        protected override void Exit()
        {
            base.Exit();
            if (_actor) _actor.Info.Might.RemovePreviewValue(this);
        }

        protected override void Down()
        {
            /* skip base.Down() */
            if (!_available) return;
            if (Targetless) return;

            _trigger.UnsetCurrent();
            _trigger.InvokeOnTooltipShown();
            _canvas.overrideSorting = _moveUI.enabled = _dragged = true;
            _actionBaseInfo.Value = _action;
        }

        protected override void Up()
        {
            /* skip base.Up() */

            if (_actionBaseInfo.Value == _action)
                _actionBaseInfo.Value = null;

            if (Targetless) Act(_actor, _actor);
        }

        private void OnMouseUpTrigger()
        {
            if (!_dragged) return;

            _moveUI.enabled = _dragged = false;
            _moveUI.transform.DOLocalMove(default, 0.1f)
                .onComplete += () => _canvas.overrideSorting = false;

            Act(_actor, _hoveredEntity.Value);
        }

        private void Act(ActorHolder source, EntityHolder target)
        {
            var ok = _action.Act(_ui.RawData, source, target);
            Refresh();
        }

#if UNITY_EDITOR
        private void OnValidate() => _activate = ActivatingMode.Hover;
#endif
    }
}
