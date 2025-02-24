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
            var removes = new List<ModifierData>();
            foreach (var modifier in _modifiers)
                if (modifier.OnModifierEvent(ModifierEvent.OnTurnStarted, this))
                    removes.Add(modifier);

            foreach (var remove in removes)
            {
                remove.OnModifierEvent(ModifierEvent.OnRemoved, this);
                _modifiers.Remove(remove);
            }

            _modifiers.ForEach(modifier => modifier.OnModifierEvent(ModifierEvent.OnTurnEnded, this));
            Might.Regen();
        }

        public void OnTurnEnd() { }

        public void AddModifier(ModifierData data)
        {
            _modifiers.Add(data);
            data.OnModifierEvent(ModifierEvent.OnAdded, this);
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

        public bool GetSkill(SkillInfo info, out SkillData skill)
        {
            skill = Skills.Where(s => s.Info == info).FirstOrDefault();
            return skill != null;
        }

        public override List<ItemData> GetLoot()
        {
            var list = base.GetLoot();
            list.Add(Equipment, item => !item.Info.Hidden);
            return list;
        }

        public void Hurt(int amount) => Might.AddMissingValue(amount);
        public void Heal(int amount) => Might.RemoveMissingValue(amount);
        public void Kill() => Might.AddMissingValue(Might.Available);
        public void FullHeal() => Might.ResetMissingValue();
    }
}
