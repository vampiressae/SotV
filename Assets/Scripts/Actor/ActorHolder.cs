using System;
using UnityEngine;
using Sirenix.OdinInspector;
using Inventory;
using Entity;

namespace Actor
{
    public class ActorHolder : EntityHolder
    {
        [Space]
        [SerializeField] private ActorInfo _info;
        public ActorInfo Info
        {
            get
            {
                if (!_inited)
                {
                    _info = Instantiate(_info);
                    _inited = true;
                }
                return _info;
            }
        }

        [ShowInInspector, HideInEditorMode] private InventoryHolder _inventory, _equipment;
        [ShowInInspector, HideInEditorMode] private ActorMightUI _mightUI;

        private bool _inited;

        private void OnDestroy()
        {
            if (_inventory) _inventory.OnAnyChanged -= InventoryOnAnyChanged;
            if (_equipment) _equipment.OnAnyChanged -= EquipmentOnAnyChanged;
            if (_mightUI) _mightUI.Uninit();
        }

        public void InitInventories(InventoryHolder inventory, InventoryHolder equipment)
        {
            _inventory = inventory;
            _equipment = equipment;

            _inventory.OnAnyChanged += InventoryOnAnyChanged;
            _equipment.OnAnyChanged += EquipmentOnAnyChanged;
            AnyInventoryHolderChanged();
        }

        private void InventoryOnAnyChanged() => AnyInventoryHolderChanged();
        private void EquipmentOnAnyChanged() => AnyInventoryHolderChanged();
        private void AnyInventoryHolderChanged() => UpdateMightCost();

        private void UpdateMightCost()
        {
            var cost = 0;
            if (_inventory) cost += _inventory.GetMightCost();
            if (_equipment) cost += _equipment.GetMightCost();
            Info.Might.AddReservedValue(this, cost);
        }
    }
}
