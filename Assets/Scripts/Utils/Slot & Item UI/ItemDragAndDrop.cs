using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory
{
    public abstract class ItemDragAndDrop : MonoBehaviour, IDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private enum DropResult { Return, Transfer, Stack, Swap }

        [SerializeField] private ScriptableEvent _mouseUpEvent;
        [SerializeField] private TooltipTrigger _tooltipTrigger;
        [SerializeField] private ItemDragAndDropTooltip _tooltipPrefab;
        [SerializeField] protected ItemDragAndDropInfo _value;
        [Space]
        [SerializeField, Range(0, 1)] private float _faded = 0.3f;

        private bool _dragged;
        private float _delay = 0.1f;

        private CanvasGroup _fader;
        private ItemDragAndDropTooltip _tt;
        protected SlotUI _ui;

        protected abstract bool InteractionBlocked(SlotUI ui);
        protected abstract bool CanInfoBeStacked(ScriptableWithNameAndSprite info);

        private DropResult Transfer
        {
            get
            {
                if (_value.Value == null || _value.Value == this) return DropResult.Return;
                var otherUI = _value.Value._ui;
                if (otherUI.IsEmpty) return DropResult.Transfer;
                return otherUI.RawData.RawInfo == _ui.RawData.RawInfo && CanInfoBeStacked(otherUI.RawData.RawInfo) ? DropResult.Stack : DropResult.Swap;
            }
        }

        private void Awake() => _ui = GetComponent<SlotUI>();

        private void OnEnable()
        {
            _ui.OnChanged += OnDataChanged;
            _mouseUpEvent.OnTrigger += OnUpTrigger;
        }

        private void OnDisable()
        {
            _ui.OnChanged -= OnDataChanged;
            _mouseUpEvent.OnTrigger -= OnUpTrigger;
        }

        private void OnDataChanged()
        {
            _fader = _ui.IsEmpty ? null : _ui.RawItemUI.GetComponent<CanvasGroup>();
        }

        private void OnUpTrigger()
        {
            if (!_dragged) return;

            var transfer = _value.Value == null || InteractionBlocked(_value.Value._ui) ? DropResult.Return : Transfer;
            switch (transfer)
            {
                case DropResult.Return: SetItemVisible(true); break;

                case DropResult.Transfer:
                case DropResult.Swap:
                    _tt.Init(_value.Value);
                    _ui.RawData.Swap(_value.Value._ui.RawData);
                    break;
            }

            _tt = null;
            _dragged = false;
            _tooltipTrigger.UnsetCurrent();
            _tooltipTrigger.InvokeOnTooltipShown();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_dragged || _value.Value == null) return;
            if (InteractionBlocked(_value.Value._ui)) return;
            if (_ui.IsEmpty)
            {
                eventData.Use();
                return;
            }

            _dragged = true;
            _value.Value = this;

            _tt = _tooltipTrigger.SetCurrent(_tooltipPrefab, this);
            _tt.Init(_ui.RawData);
            _tt.Init(this);

            _tooltipTrigger.InvokeOnTooltipShown();
            SetItemVisible(false);
        }

        public void OnPointerEnter(PointerEventData _) => _value.Value = this;
        public void OnPointerExit(PointerEventData _) { if (_value.Value == this) _value.Value = null; }

        public void SetItemVisibleFromZero()
        {
            if (_fader) _fader.alpha = 0;
            else _ui.RawItemUI.gameObject.SetActive(false);
            SetItemVisible(true);
        }

        private void SetItemVisible(bool visible)
        {
            if (_fader) _fader.alpha = visible ? 1 : _faded;
            else
            {
                if (visible) Invoke(nameof(SetItemVisibleTrue), _delay);
                else _ui.RawItemUI.gameObject.SetActive(false);
            }
        }

        private void SetItemVisibleTrue() => _ui.RawItemUI.gameObject.SetActive(true);
    }
}