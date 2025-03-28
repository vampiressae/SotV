using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Inventory;
using Actor;

namespace Items
{
    [LabelWidth(70)]
    [CreateAssetMenu(menuName = "Items/Info")]
    public partial class ItemInfo : ScriptableWithNameAndSpriteAndTooltip, IItemHasStack
    {
        [Space]
        public int Stack = 1;
        [HorizontalGroup("might")] public int Might = 1;
        [HorizontalGroup("might"), LabelText(" Equipped")] public int EquippedMight = -1;
        [HorizontalGroup("rank")] public ItemRank Rank;
        [HorizontalGroup("rank", 60), LabelWidth(44)] public bool Hidden;
        [Space]
        [GUIColor(0.8f, 1, 1)]
        public ItemTag[] Tags;

        [GUIColor(1, 0.8f, 1)]
        [ListDrawerSettings(CustomRemoveElementFunction = "RemoveEffect", CustomAddFunction = "AddEffect")]
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

        public override void TooltipInit(ActorHolder actor, TooltipForString tooltip, bool actionSummary)
        {
            base.TooltipInit(actor, tooltip, actionSummary);
            if (Rank) Rank.SetText(tooltip.Title);
        }

        protected override void TooltipDescriptions(ActorHolder actor, ref List<string> descriptions)
        {
            base.TooltipDescriptions(actor, ref descriptions);
            foreach (var effect in _effects)
                effect.GetTooltip(ref descriptions);
        }

#if UNITY_EDITOR
        private void AddEffect() => _effects.AddScriptableCopy(this);
        private void RemoveEffect(ItemEffect item) => _effects.RemoveScriptableCopy(item);

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