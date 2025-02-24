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
    public sealed class ActorInfo : EntityInfo
    {
        [BoxGroup("Inventories")] public List<ItemData> Equipment;
        [BoxGroup("Actor")] public List<SkillData> Skills;
        [BoxGroup("Actor")] public Sprite Sprite;

        [InlineProperty, HideLabel, BoxGroup("Might")] public ActorMight Might;

        [SerializeField] private List<ModifierData> _modifiers;

        public void OnTurnStart()
        {
            InvokeModifierEvent(ModifierEvent.OnTurnStarted);
            Might.Regen();
        }

        public void OnTurnEnd() => InvokeModifierEvent(ModifierEvent.OnTurnEnded);

        public void AddModifier(ModifierData data)
        {
            _modifiers.Add(data);
            data.OnModifierEvent(ModifierEvent.OnAdded, this);
        }

        private void InvokeModifierEvent(ModifierEvent e)
        {
            var removes = new List<ModifierData>();
            foreach (var modifier in _modifiers)
                if (modifier.OnModifierEvent(e, this))
                    removes.Add(modifier);
            RemoveExpiredModifiers(removes);
        }

        private void InvokeModifierEvent(ModifierEventInt e, ref int value)
        {
            var removes = new List<ModifierData>();
            foreach (var modifier in _modifiers)
                if (modifier.OnModifierEvent(e, this, ref value))
                    removes.Add(modifier);
            RemoveExpiredModifiers(removes);
        }

        private void RemoveExpiredModifiers(List<ModifierData> list)
        {
            foreach (var data in list)
            {
                _modifiers.Remove(data);
                data.OnModifierEvent(ModifierEvent.OnRemoved, this);
            }
        }

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
        //[PropertySpace, Button] private void AddModifier(ModifierInfo info, int value, int time) => AddModifier(new(info, value, time));
#endif
    }
}
