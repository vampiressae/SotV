using UnityEngine;
using DG.Tweening;
using Entity;
using Items;

namespace Actor
{
    public class ActorEffects : MonoBehaviour
    {
        private ActorHolder _actor;

        private void Awake() => _actor = GetComponent<ActorHolder>();

        private void OnEnable()
        {
            _actor.Info.Might.OnMissingChanged += OnMissingChanged;

            ActionBase.OnActed += OnItemActionActed;
        }

        private void OnDisable()
        {
            _actor.Info.Might.OnMissingChanged -= OnMissingChanged;

            ActionBase.OnActed -= OnItemActionActed;
        }

        private void OnMissingChanged(int delta)
        {
            transform.localScale = Vector3.one * 1.08f;
            transform.DOScale(1, 0.1f);

            FlyingTextController.Show(_actor, (-delta).ToStringWithSign());
        }

        private void OnItemActionActed(ActorHolder source, EntityHolder target, Item item, ActionBase action)
        {
            if (source != _actor) return;

            _actor.transform.DOPunchPosition(new(0, 0.2f, 0), 0.2f, 0, 0);
            if (action is ActionWithInfo awi)
                FlyingTextController.Show(_actor, awi.InfoRaw.Name);
        }
    }
}
