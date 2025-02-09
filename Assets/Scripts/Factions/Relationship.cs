using Sirenix.OdinInspector;

namespace Factions
{
    [System.Serializable]
    public class Relationship
    {
        [System.Serializable]
        public class Module
        {
            [HideLabel] public Reaction Reaction;
            public FactionInfo[] Factions;
        }

        [HideLabel, HorizontalGroup("1"), GUIColor(0.5f, 1, 1)] public FactionInfo Faction;
        [HideLabel, HorizontalGroup("1", 100)] public Reaction DefaultReaction;
        [LabelText("Relationships")] public Module[] Modules;
    }
}