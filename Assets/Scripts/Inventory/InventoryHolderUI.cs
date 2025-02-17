using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class InventoryHolderUI : MonoBehaviour
    {
        [SerializeField] private InventoryHolder _inventory;
        [SerializeField] private InventoryItemUI _itemPrefab;
        [SerializeField] private InventorySlotUI _slotPrefab;
        [SerializeField] private Transform _slotParent;
        [SerializeField] private bool _shrinkToMin;
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

            if (_shrinkToMin && _inventory.Slots < 10)
                if (TryGetComponent<GridLayoutGroup>(out var layout))
                    layout.constraintCount = _inventory.Slots;
        }

        private void OnDestroy()
        {
            for (int i = 0; i < _uis.Count; i++)
                _uis[i].Uninit();
        }
    }
}
