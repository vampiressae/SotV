using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using Items;
using Entity;
using Skills;
using Modifier;

namespace Actor
{
    [CreateAssetMenu(menuName = "Entity/Actor Info")]
    public sealed partial class ActorInfo : EntityInfo
    {
        [BoxGroup("Inventories")] public List<ItemData> Equipment;
        [BoxGroup("Actor")] public List<SkillData> Skills;
        [BoxGroup("Actor")] public Sprite Sprite;

        [InlineProperty, HideLabel, BoxGroup("Might")] public ActorMight Might;

        [ListDrawerSettings(CustomRemoveElementFunction = "RemoveModifier")]
        [SerializeField] private List<ModifierData> _modifiers = new();

        public void OnTurnStart()
        {
            InvokeModifierEvent(ModifierEvent.OnTurnStarted);
            Might.Regen();
        }

        public void OnTurnEnd() => InvokeModifierEvent(ModifierEvent.OnTurnEnded);

        public void UpdateMightReserved()
        {
            var cost = 0;

            foreach (var item in Inventory)
                if (!item.IsEmpty)
                    cost += item.Info.Might * item.Amount;

            foreach (var item in Equipment)
                if (!item.IsEmpty)
                    cost += (item.Info.EquippedMight < 0 ? item.Info.Might : item.Info.EquippedMight) * item.Amount;

            Might.AddReservedValue(this, cost);
        }

        private int GetSkillAmount(SkillInfo info)
        {
            var skill = Skills.Where(s => s.Info == info).FirstOrDefault();
            return skill == null ? 0 : skill.Experience;
        }

        public SkillExpertise GetActionAmount(SkillInfo info)
            => info.GetExpertise(GetSkillAmount(info));

        public override List<ItemData> GetLoot()
        {
            var list = base.GetLoot();
            list.Add(Equipment, item => !item.Info.Hidden);
            return list;
        }

        public void Hurt(int amount)
        {
            InvokeModifierEvent(ModifierEventInt.OnDamageReceive, ref amount);
            Might.AddMissingValue(amount);
        }

        public void Heal(int amount) => Might.RemoveMissingValue(amount);
        public void Kill() => Might.AddMissingValue(Might.Available);
        public void FullHeal() => Might.ResetMissingValue();

#if UNITY_EDITOR
        private void RemoveModifier(ModifierData modifier)
        {
            modifier.OnUnvalidate(this); 
            _modifiers.Remove(modifier);
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            _modifiers.ForEach(modifier => modifier.OnValidate(this));
        }
#endif
    }
}
