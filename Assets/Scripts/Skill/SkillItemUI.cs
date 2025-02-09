using UnityEngine;

namespace Skills
{
    public class SkillItemUI : ItemUI<SkillData, SkillInfo>
    {

        public override void Init(SlotUI<SkillData, SkillInfo> slotUI)
        {
            base.Init(slotUI);
        }

        protected override void Refresh()
        {
            base.Refresh();
        }
    }
}
