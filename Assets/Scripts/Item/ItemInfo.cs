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

        //public virtual void HandleExtendedUI(ItemUI ui) { }

        public virtual string GetTooltip() => Name;

        [GUIColor(1, 0.8f, 1)]
        [ListDrawerSettings(HideAddButton = true, CustomRemoveElementFunction = "RemoveItemEffect")]
        [SerializeField, InlineEditor] private List<ItemEffect> _effects = new();

        int IItemHasStack.Stack => Stack;

        public void Effects_AddedToInventory(InventoryHolder inventory, ItemData data) 
            => _effects.ForEach(fx => fx.AddedToInventory(inventory, data));

        public void Effects_RemovedToInventory(InventoryHolder inventory, ItemData data) 
            => _effects.ForEach(fx => fx.RemovedFromInventory(inventory, data));
    }
}