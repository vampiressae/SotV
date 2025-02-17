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
        public List<ItemData> Equipment;
        public List<SkillData> Skills;
        public Sprite Sprite;

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
            skill = Skills.Where(s=>s.Info == info).FirstOrDefault();
            return skill != null;
        }
    }
}
