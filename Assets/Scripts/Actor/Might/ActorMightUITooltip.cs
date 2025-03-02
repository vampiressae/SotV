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

        [SerializeField] private LayoutElement _reservedHolder, _availableHolder, _consumedHolder, _missingHolder;
        [SerializeField] private TMP_Text _reserved, _available, _consumed, _missing;
        [Space]
        [SerializeField] private ActorMightUIReservedRow _rowPrefab;
        [SerializeField] private Transform _rowParent;
        [SerializeField] private float _minHeight = 20;

        private ActorMightUI _ui;
        private ActorMight _might;

        private TMP_Text[] _texts;
        private string[] _labels;
        private MightType[] _values;
        private RectTransform _faderRT;
        private Vector2 _faderSize;

        private void Awake() => _fader.alpha = 0;

        private void Start()
        {
            _ui = GetComponent<ActorMightUI>();

            _might = _ui.Might;
            _might.OnAnyValueChanged += Refresh;

            _faderRT = _fader.transform as RectTransform;
            _faderSize = _faderRT.sizeDelta;

            _texts = new TMP_Text[] { _reserved, _available, _consumed, _missing };
            _labels = new string[] { "Reserved", "Available", "Consumed", "Missing" };
            _values = new MightType[] { MightType.Reserved, MightType.Available, MightType.Consumed, MightType.Missing };
        }

        private void OnDestroy()
        {
            if (_might == null) return;
            _might.OnAnyValueChanged -= Refresh;
        }

        public void OnPointerEnter(PointerEventData _)
        {
            Kill();
            _faderRT.DOSizeDelta(_faderSize, 0.3f).SetEase(Ease.OutQuart);
            _fader.DOFade(1, 0.3f);
            _fader.gameObject.SetActive(true);
            Refresh();
        }

        public void OnPointerExit(PointerEventData _)
        {
            Kill();
            _faderRT.DOSizeDelta(new(_faderSize.x, _minHeight), 0.3f).SetEase(Ease.OutQuart);
            _fader.DOFade(0, 0.3f).onComplete += () => _fader.gameObject.SetActive(false);
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
            var recover = Mathf.Clamp(_might.Consumed / max, 0, 1);

            _reservedHolder.preferredWidth = _ui.Reserved.rectTransform.rect.width;
            _consumedHolder.preferredWidth = recover * width;
            _missingHolder.preferredWidth = missing * width;

            _reserved.gameObject.SetActive(_reservedHolder.preferredWidth > 1);
            _consumed.gameObject.SetActive(_consumedHolder.preferredWidth > 1);
            _missing.gameObject.SetActive(_missingHolder.preferredWidth > 1);

            _rowParent.Clear(element => !element.name.StartsWith("_"));
            foreach (var reserved in _might.AllReserved)
                Instantiate(_rowPrefab, _rowParent)
                     .Init(reserved.Key.Name, reserved.Value);
        }

        private void Kill()
        {
            DOTween.Kill(_fader);
            DOTween.Kill(_fader.transform);
        }
    }
}
