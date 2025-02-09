using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class InventoryHolderUI : MonoBehaviour
    {
        [SerializeField] private InventoryHolder _inventory;
        [SerializeField] private InventoryItemUI _itemPrefab;
        [SerializeField] private InventorySlotUI _slotPrefab;
        [SerializeField] private Transform _slotParent;
        
        public bool Blocked;

        public InventoryHolder Inventory => _inventory;
        public InventoryItemUI ItemPrefab => _itemPrefab;

        private readonly List<InventorySlotUI> _uis = new();

        private void Start()
        {
            for (int i = 0; i < _inventory.Slots; i++)
            {
                var ui = Instantiate(_slotPrefab, _slotParent);
                ui.Init(this, _inventory.Items[i]);
                ui.name = $"{name} SLOT #{i}";
                _uis.Add(ui);
            }

            _uis.ForEach(ui => ui.OnDataChanged());
        }
    }
}
