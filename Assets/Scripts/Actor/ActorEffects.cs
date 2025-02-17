using UnityEngine;
using DG.Tweening;
using Entity;
using Items;

namespace Actor
{
    public class ActorEffects : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private Collider2D _collider;

        private ActorHolder _actor;

        private void Awake() => _actor = GetComponent<ActorHolder>();

        private void Start()
        {
            _actor.Info.Might.OnMissingChanged += OnMissingChanged;
            ActionBase.OnActed += OnItemActionActed;
        }

        private void OnDestroy()
        {
            _actor.Info.Might.OnMissingChanged -= OnMissingChanged;
            ActionBase.OnActed -= OnItemActionActed;
        }

        private void OnMissingChanged(int delta)
        {
            transform.localScale = Vector3.one * 1.08f;
            transform.DOScale(1, 0.1f);

            FlyingTextController.Show(_actor, -delta);

            if (_actor.IsDead) Die();
        }

        private void OnItemActionActed(ActorHolder source, EntityHolder target, Item item, ActionBase action)
        {
            if (source != _actor) return;

            _actor.transform.DOPunchPosition(new(0, 0.2f, 0), 0.2f, 0, 0);
            if (action is ActionWithInfo awi)
            {
                var text = FlyingTextController.Show(_actor, awi.RawInfo.Name);
                if (text && action.Rank) action.Rank.SetText(text.Text);
            }
        }

        private void Die()
        {
            _collider.enabled = false;
            _renderer.DOColor(new(1, 0, 0, 0), 0.5f).SetDelay(0.3f).onComplete += DieComplete;
        }

        private void DieComplete()
        {
            gameObject.SetActive(false);
        }
    }
}
