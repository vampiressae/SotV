using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using Modifier;
using Entity;
using Actor;

namespace Skills
{
    [CreateAssetMenu(menuName = "Skill/Actions/Add Skill")]
    public class SkillActionInfoAddSkill : SkillActionInfo
    {
        private enum Targetting { OnSource, OnTarget }

        [System.Serializable]
        private struct ModifierModule
        {
            [HorizontalGroup(90), HideLabel] public Targetting Targetting;
            [HorizontalGroup, HideLabel] public ModifierInfo Info;
            [HorizontalGroup(50), LabelWidth(10), LabelText("x")] public int Amount;
            [HorizontalGroup(50), LabelWidth(10), LabelText("T")] public int Timer;
            public ModifierData GetData() => new(Info, Amount, Timer);
        }

        [SerializeField] private List<ModifierModule> _addSkills;

        public override void Act(Item item, ActorHolder source, EntityHolder target)
        {
            if (target is ActorHolder targetActor)
                foreach (ModifierModule m in _addSkills)
                {
                    var to = m.Targetting == Targetting.OnSource ? source.Info : targetActor.Info;
                    to.AddModifier(m.GetData());
                }

            base.Act(item, source, target);
        }
    }
}
