using UnityEngine;
using Sirenix.OdinInspector;
using Entity;
using Inventory;
using DG.Tweening;

namespace Actor
{
    public class ActorHolder : EntityHolder
    {
        [Space]
        [SerializeField, ShowIf("ShowInfoInInspector")] private ActorInfo _info;
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private PolygonCollider2D _collider;
        [Space]
        [ShowInInspector, HideInEditorMode] private InventoryHolder _inventory, _equipment;

        public virtual ActorInfo Info
        {
            get
            {
                if (!_inited)
                {
                    _info = Instantiate(_info);
                    _info.Init();
                    _inited = true;
                }
                return _info;
            }
        }

        public bool IsAlive => Info && Info.Might.Alive;
        public bool IsDead => Info == null || Info.Might.Dead;

        private bool _inited;

        private void OnDestroy() => Uninit();

        public void Init(ActorInfo info)
        {
            _info = info;
            _renderer.sprite = _info.Sprite;
            transform.localPosition += new Vector3(-1, 0);
            name = "Actor Holder - " + info.Name;

            _collider.FromSprite(_info.Sprite, 0.1f);
        }

        private void Uninit()
        {
            if (_inventory) _inventory.OnAnyChanged -= OnAnyChanged;
            if (_equipment) _equipment.OnAnyChanged -= OnAnyChanged;
            _renderer.DOKill();
        }

        public void InitInventory(InventoryHolder inventory)
        {
            _inventory = inventory;
            if (_inventory) _inventory.OnAnyChanged += OnAnyChanged;
            OnAnyChanged();
        }

        public void InitEquipment(EquipmentHolder equipment)
        {
            _equipment = equipment;
            if (_equipment) _equipment.OnAnyChanged += OnAnyChanged;
            OnAnyChanged();
        }

        private void OnAnyChanged()
        {
            Info.UpdateMightReserved();
        }

#if UNITY_EDITOR
        protected virtual bool ShowInfoInInspector => true;
#endif
    }
}
