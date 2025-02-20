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

        protected override List<ItemData> InitInventory()
        {
            var list = base.InitInventory();
            if (FightController.Instance.Player.Info != this)
                _inventory.Add(Equipment, item => !item.Info.Hidden);
            return list;
        }
    }
}
