using System;
using System.Collections.Generic;
using UnityEngine;
using Items;
using Skills;

namespace Actor
{
    [CreateAssetMenu(menuName = "Entity/Actor Info")]
    public class ActorInfo : ScriptableWithNameAndSprite
    {
        public event Action OnMightChanged;

        public ActorMight Might;
        public List<ItemData> Inventory, Equipment;
        public List<SkillData> Skills;
    }
}
