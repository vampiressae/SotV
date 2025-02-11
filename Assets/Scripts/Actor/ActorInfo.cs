using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Items;
using Skills;
using Entity;

namespace Actor
{
    [CreateAssetMenu(menuName = "Entity/Actor Info")]
    public class ActorInfo : EntityInfo
    {
        public event Action OnMightChanged;

        public List<ItemData> Inventory, Equipment;
        public List<SkillData> Skills;

        [InlineProperty, HideLabel, BoxGroup("Might")] public ActorMight Might;
    }
}
