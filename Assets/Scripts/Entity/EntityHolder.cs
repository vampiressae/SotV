using UnityEngine;

namespace Entity
{
    public class EntityHolder : MonoBehaviour, IFlyingText
    {
        [SerializeField] private EntityValue _hoveredEntity;
        [SerializeField] private Vector2 _flyingTextPosition;
        [SerializeField] private float _flyingTextRadius = 5;

        public Vector2 Position => Offset + transform.position;
        public float Radius => _flyingTextRadius;

        private Vector3 Offset => new Vector2(_flyingTextPosition.x * Direction, _flyingTextPosition.y);
        private int Direction => transform.lossyScale.x < 0 ? -1 : 1;

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
            Gizmos.DrawWireSphere(Position, _flyingTextRadius);
        }
#endif
    }
}
