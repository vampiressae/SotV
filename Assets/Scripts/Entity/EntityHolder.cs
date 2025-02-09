using UnityEngine;

namespace Entity
{
    public class EntityHolder : MonoBehaviour, IFlyingText
    {
        [SerializeField] private EntityValue _hoveredEntity;
        [SerializeField] private Vector2 _flyingTextPosition;
        [SerializeField] private float _flyingTextRadius = 5;

        public Vector2 Position => _flyingTextPosition + (Vector2)transform.position;
        public float Radius => _flyingTextRadius;

        private void OnMouseEnter()
        {
            _hoveredEntity.Value = this;
        }

        private void OnMouseExit()
        {
            if (_hoveredEntity.Value == this)
                _hoveredEntity.Value = null;
        }

#if UNITY_EDITOR
        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position + (Vector3)_flyingTextPosition, _flyingTextRadius);
        }
#endif
    }
}
