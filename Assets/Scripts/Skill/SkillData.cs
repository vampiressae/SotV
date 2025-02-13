using Actor;
using UnityEngine;

namespace Skills
{
    [System.Serializable]
    public class SkillData : Item<SkillInfo>
    {
        public int Level => _amount;

        public override bool IsEmpty => false;
        public override bool IsFull =>false;

        public override void InvokeOnItemDataChanging() {/* not important now */}
        public override void InvokeOnItemDataChanged() {/* not important now */}
        public override void Swap(Item with) {/* not important now */}
    }
}
