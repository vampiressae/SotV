namespace Skills
{
    public class SkillsUIFight : SkillsUI
    {
        protected override bool ShowSkillsWithoutActions => false;
        public override bool ShowActions => true;
    }
}
