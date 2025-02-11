using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Actor
{
    public class ActorMightUISimple : MonoBehaviour
    {
        [SerializeField] private Image _fill;
        [SerializeField] private float _duration = 0.1f;

        private ActorHolder _actor;
        private ActorMight _might;

        private void Awake() => _fill.fillAmount = 0;
        private void OnDisable() => Kill();

        private void Start()
        {
            _actor = FightController.Instance.Enemy;
            Init(_actor.Info.Might);
        }

        public void Init(ActorMight might)
        {
            _might = might;
            _might.OnAnyValueChanged += Refresh;
            Refresh();
        }

        public void Uninit() => _might.OnAnyValueChanged -= Refresh;

        private void Refresh()
        {
            Kill();
            var max = (float)_might.Max; //to divide with float
            _fill.DOFillAmount(_might.Available / max, _duration).SetEase(Ease.OutSine);
        }

        private void Kill() => DOTween.Kill(_fill.fillAmount);
    }
}