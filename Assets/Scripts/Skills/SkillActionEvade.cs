using Actor;
using Entity;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(menuName = "Skill/Actions/Evade")]
    public class SkillActionEvade : SkillActionActive
    {
        public override void Act(Item item, ActorHolder source, EntityHolder target)
        {
            base.Act(item, source, target);
        }
    }
}
