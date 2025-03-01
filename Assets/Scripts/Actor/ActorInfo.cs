using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using Items;
using Entity;
using Skills;
using Modifier;
using Affliction;

namespace Actor
{
    [CreateAssetMenu(menuName = "Entity/Actor Info")]
    public sealed partial class ActorInfo : EntityInfo
    {
        private class MightReserverInventory : IActorMightReserver { public string Name => "Inventory"; }
        private class MightReserverEquipment : IActorMightReserver { public string Name => "Equipped"; }

        private MightReserverInventory _mightReserverInventory = new();
        private MightReserverEquipment _mightReserverEquipment = new();

        [BoxGroup("Inventories")] public List<ItemData> Equipment;
        [BoxGroup("Actor")] public List<SkillData> Skills;
        [BoxGroup("Actor")] public Sprite Sprite;

        [InlineProperty, HideLabel, BoxGroup("Might")] public ActorMight Might;

        [ListDrawerSettings(CustomRemoveElementFunction = "RemoveModifier")]
        [SerializeField] private List<ModifierData> _modifiers = new();

        [ListDrawerSettings(CustomRemoveElementFunction = "RemoveEffect", CustomAddFunction = "AddEffect")]
        [SerializeField, InlineEditor, GUIColor(1, 0.7f, 0.7f)] private List<AfflictionInfo> _afflictions;


        public void OnTurnStart()
        {
            InvokeModifierEvent(ModifierEvent.OnTurnStarted);
            InvokAfflictions(AfflictionMoment.TurnStart);
            InvokAfflictions(AfflictionMoment.RoundStart);
            Might.Regen();
        }

        public void OnTurnEnd()
        {
            InvokeModifierEvent(ModifierEvent.OnTurnEnded);
            InvokAfflictions(AfflictionMoment.TurnEnd);
            InvokAfflictions(AfflictionMoment.RoundEnd);
        }

        public void UpdateMightReserved()
        {
            var cost = 0;
            foreach (var item in Inventory)
                if (!item.IsEmpty)
                    cost += item.Info.Might * item.Amount;
            Might.AddReservedValue(_mightReserverInventory, cost);

            cost = 0;
            foreach (var item in Equipment)
                if (!item.IsEmpty)
                    cost += (item.Info.EquippedMight < 0 ? item.Info.Might : item.Info.EquippedMight) * item.Amount;
            Might.AddReservedValue(_mightReserverEquipment, cost);
        }

        private int GetSkillAmount(SkillInfo info)
        {
            var skill = Skills.Where(s => s.Info == info).FirstOrDefault();
            return skill == null ? 0 : skill.Experience;
        }

        public void AddSkillAmount(SkillInfo info, int amount = 1)
        {
            var skill = Skills.Where(s => s.Info == info).FirstOrDefault();
            if (skill == null)
            {
                skill = new() { Info = info, Amount = 1 };
                Skills.Add(skill);
            }
            else skill.Amount += amount;
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
        private void AddEffect() => _afflictions.AddScriptableCopy(this);
        private void RemoveEffect(AfflictionInfo item) => _afflictions.RemoveScriptableCopy(item);
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
