using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using Items;
using Skills;
using Entity;

namespace Actor
{
    [CreateAssetMenu(menuName = "Entity/Actor Info")]
    public sealed class ActorInfo : EntityInfo
    {
        [BoxGroup("Inventories")] public List<ItemData> Equipment;
        [BoxGroup("Actor")] public List<SkillData> Skills;
        [BoxGroup("Actor")] public Sprite Sprite;

        [InlineProperty, HideLabel, BoxGroup("Might")] public ActorMight Might;

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
