using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Inventory;

namespace Items
{
    [CreateAssetMenu(menuName = "Items/Info")]
    public partial class ItemInfo : ScriptableWithNameAndSprite, IItemHasStack
    {
        [Space]
        public int Stack = 1;
        public int Might = 1;
        public ItemRank Rank;
        [Space]
        [GUIColor(0.8f, 1, 1)]
        public ItemTag[] Tags;

        [GUIColor(1, 0.8f, 1)]
        [ListDrawerSettings(HideAddButton = true, CustomRemoveElementFunction = "RemoveItemEffect")]
        [SerializeField, InlineEditor] private List<ItemEffect> _effects = new();

        protected override void GetTooltip(ref List<string> list)
        {
            base.GetTooltip(ref list);
            for (int i = 0; i < _effects.Count; i++)
                _effects[i].GetTooltip(ref list);
        }

        int IItemHasStack.Stack => Stack;

        public void Effects_AddedToInventory(InventoryHolder inventory, ItemData data)
            => _effects.ForEach(fx => fx.AddedToInventory(inventory, data));

        public void Effects_RemovedFromInventory(InventoryHolder inventory, ItemData data)
            => _effects.ForEach(fx => fx.RemovedFromInventory(inventory, data));

        public void Effects_AddedToEquipment(EquipmentHolder equipment, ItemData data)
            => _effects.ForEach(fx => fx.AddedToInventory(equipment, data));

        public void Effects_RemovedFromEquipment(EquipmentHolder equipment, ItemData data)
            => _effects.ForEach(fx => fx.RemovedFromInventory(equipment, data));
    }
}