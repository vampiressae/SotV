using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

using static Actor.ActorMight;

namespace Actor
{
    [DefaultExecutionOrder(10)]
    public class ActorMightUITooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private CanvasGroup _fader;
        [SerializeField] private RectTransform _rtForWidth;

        [SerializeField] private LayoutElement _reservedHolder, _availableHolder, _recoverableHolder, _missingHolder;
        [SerializeField] private TMP_Text _reserved, _available, _recoverable, _missing;
        [Space]
        [SerializeField] private ActorMightUIReservedRow _rowPrefab;
        [SerializeField] private Transform _rowParent;

        private ActorMightUI _ui;
        private ActorMight _might;

        private TMP_Text[] _texts;
        private string[] _labels;
        private MightType[] _values;

        private void Awake() => _fader.alpha = 0;

        private void Start()
        {
            _ui = GetComponent<ActorMightUI>();

            _might = _ui.Might;
            _might.OnAnyValueChanged += Refresh;

            _texts = new TMP_Text[] { _reserved, _available, _recoverable, _missing };
            _labels = new string[] { "Reserved", "Available", "Recovering", "Missing" };
            _values = new MightType[] { MightType.Reserved, MightType.Available, MightType.Recoverable, MightType.Missing };
        }

        private void OnDestroy()
        {
            if (_might == null) return;
            _might.OnAnyValueChanged -= Refresh;
        }

        public void OnPointerEnter(PointerEventData _)
        {
            Kill();
            _fader.DOFade(1, 0.3f);
            Refresh();
        }

        public void OnPointerExit(PointerEventData _)
        {
            Kill();
            _fader.DOFade(0, 0.3f);
        }

        private void Refresh()
        {
            for (int i = 0; i < _texts.Length; i++)
            {
                var value = _might.GetValueByType(_values[i]);
                _texts[i].text = $"{(value > 8 ? _labels[i] + ": " : "")}<color=white>{value}</color>";
            }

            var width = _rtForWidth.rect.width;
            var max = (float)_might.Max; //to divide with float
            var missing = Mathf.Clamp(_might.Missing / max, 0, 1);
            var recover = Mathf.Clamp(_might.Recoverable / max, 0, 1);

            _reservedHolder.preferredWidth = _ui.Reserved.rectTransform.rect.width;
            _recoverableHolder.preferredWidth = recover * width;
            _missingHolder.preferredWidth = missing * width;

            _reserved.gameObject.SetActive(_reservedHolder.preferredWidth > 1);
            _recoverable.gameObject.SetActive(_recoverableHolder.preferredWidth > 1);
            _missing.gameObject.SetActive(_missingHolder.preferredWidth > 1);

            _rowParent.Clear(element => !element.name.StartsWith("_"));
            foreach (var reserved in _might.AllReserved)
                Instantiate(_rowPrefab, _rowParent)
                     .Init(reserved.Key.Name, reserved.Value);
        }

        private void Kill() => DOTween.Kill(_fader);
    }
}
