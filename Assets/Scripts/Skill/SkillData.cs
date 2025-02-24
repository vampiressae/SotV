using Sirenix.OdinInspector;

namespace Skills
{
    [System.Serializable]
    public class SkillData : Item<SkillInfo>
    {
        public int Experience => _amount;

        public override bool IsEmpty => false;
        public override bool IsFull =>false;

        [ShowInInspector, ReadOnly, HorizontalGroup(90), HideLabel, ShowIf("ShowAmount")]
        public SkillExpertise Expertise => Info ? Info.GetExpertise(Experience) : SkillExpertise.Apprentice;

        public override void InvokeOnItemDataChanging() {/* not important now */}
        public override void InvokeOnItemDataChanged() {/* not important now */}
        public override void Swap(Item with) {/* not important now */}

#if UNITY_EDITOR
        protected override bool ShowAmount => Info && Info.NeedsExpertise;
#endif
    }
}
