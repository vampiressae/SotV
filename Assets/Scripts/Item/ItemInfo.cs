using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Inventory;
using Actor;

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

        public virtual void TooltipInit(ActorHolder actor, TooltipForString tooltip)
        {
            var descriptions = new List<string>();
            TooltipDescriptions(actor, ref descriptions);

            tooltip.Init(Name, string.Join("\n", descriptions), true);
            if (Rank) Rank.SetText(tooltip.Title);
        }

        protected virtual void TooltipDescriptions(ActorHolder actor, ref List<string> descriptions) => descriptions.Add(Description);

#if UNITY_EDITOR
        protected override void OnValidate() 
        {
            base.OnValidate();
            if (Rank == null)
            {
                Rank = EditorUtils.LoadAssetsSafeFirst<ItemRank>("Item Rank 0");
                UnityEditor.EditorUtility.SetDirty(Rank);
            }
        }
#endif
    }
}