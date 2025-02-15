using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Actor
{
    public class ActorMightUI : MonoBehaviour
    {
        [SerializeField] private Image _reserved, _missing, _preview, _recoverable;
        [SerializeField] private float _duration = 0.2f;

        public Image Reserved => _reserved;
        public Image Missing => _missing;
        public Image Preview => _preview;
        public Image Rerecoverable => _recoverable;
        public ActorMight Might => _might;

        private ActorMight _might;
        private Image[] _images;

        protected virtual void Awake()
        {
            _images = new Image[] { _reserved, _missing, _preview, _recoverable };
            for (int i = 0; i < _images.Length; i++)
                _images[i].gameObject.SetActive(true);
        }

        private void OnDestroy() => Uninit();

        public void Init(ActorMight might)
        {
            _might = might;
            _might.OnAnyValueChanged += Refresh;
            Refresh();
        }

        public void Uninit()
        {
            _might.OnAnyValueChanged -= Refresh;
            Kill();
        }

        private void Refresh()
        {
            Kill();
            var max = (float)_might.Max; //to divide with float
            var missing = Mathf.Clamp(_might.Missing / max, 0, 1);
            var recover = Mathf.Clamp(_might.Recoverable / max, 0, 1);
            var preview = Mathf.Clamp(1 - missing - recover - _might.Preview / max, 0, 1);

            _reserved.rectTransform.DOAnchorMax(new(_might.Reserved / max, 1), _duration).SetEase(Ease.OutSine);
            _missing.rectTransform.DOAnchorMin(new(1 - missing, 0), _duration).SetEase(Ease.OutSine);
            _recoverable.rectTransform.DOAnchorMin(new(1 - missing - recover, 0), _duration).SetEase(Ease.OutSine);
            _preview.rectTransform.DOAnchorMin(new(preview, 0), _duration).SetEase(Ease.OutSine);
        }

        private void Kill()
        {
            for (int i = 0; i < _images?.Length; i++)
                DOTween.Kill(_images[i].rectTransform);
        }
    }
}
