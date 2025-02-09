using Actor;
using Entity;
using Items;

namespace Skills
{
    [System.Serializable]
    public class SkillAction : ActionWithInfoMight<SkillActionInfo>
    {
        protected override bool ActIt(Item item, ActorHolder source, EntityHolder target, bool useMight)
        {
            return base.ActIt(item, source, target, useMight);
        }
    }
}
