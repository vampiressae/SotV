using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

using static Actor.ActorMight;

namespace Actor
{
    [DefaultExecutionOrder(10)]
    public class ActorMightUIDetails : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private CanvasGroup _fader;
        [SerializeField] private LayoutElement _sizeElement;
        [SerializeField] private RectTransform _rtForWidth;

        [SerializeField] private LayoutElement _reservedHolder, _availableHolder, _consumedHolder, _missingHolder;
        [SerializeField] private TMP_Text _reserved, _available, _consumed, _missing;
        [Space]
        [SerializeField] private ActorMightUIReservedRow _rowPrefab;
        [SerializeField] private Transform _rowParent;
        [SerializeField] private float _minHeight = 20;
        [SerializeField] private Color _labelColor = new(0, 0, 0, 0.4f), _valueColor = Color.black;

        private ActorMightUI _ui;
        private ActorMight _might;

        private TMP_Text[] _texts;
        private string[] _labels;
        private MightType[] _values;
        private Vector2 _originSize;

        private void Awake() => _fader.alpha = 0;

        private void Start()
        {
            _ui = GetComponent<ActorMightUI>();

            _might = _ui.Might;
            _might.OnAnyValueChanged += Refresh;

            _originSize = new(_sizeElement.preferredWidth, _sizeElement.preferredHeight);
            _sizeElement.preferredHeight = 0;

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
            _sizeElement.DOPreferredSize(_originSize, 0.3f).SetEase(Ease.OutQuart);
            _fader.DOFade(1, 0.3f);
            _fader.gameObject.SetActive(true);
            Refresh();
        }

        public void OnPointerExit(PointerEventData _)
        {
            Kill();
            _sizeElement.DOPreferredSize(new(_originSize.x, 0), 0.3f).SetEase(Ease.OutQuart);
            _fader.DOFade(0, 0.3f).onComplete += () => _fader.gameObject.SetActive(false);
        }

        private void Refresh()
        {
            for (int i = 0; i < _texts.Length; i++)
            {
                var value = _might.GetValueByType(_values[i]);
                var lcolor = ColorUtility.ToHtmlStringRGBA(_labelColor);
                var vcolor = ColorUtility.ToHtmlStringRGBA(_valueColor);
                var label = value > 8 ? _labels[i] + ": " : "";
                _texts[i].text = $"<color=#{lcolor}>{label}<color=#{vcolor}>{value}</color></color>";
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
