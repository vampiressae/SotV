using Actor;

namespace Modifier
{
    public abstract class ModifierInfo : ScriptableWithNameAndSpriteAndTooltip
    {
        public abstract void OnEvent(ModifierEvent e, ActorInfo actor);
    }
}