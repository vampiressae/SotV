using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Items;
using Actor;

namespace Skills
{
    [CreateAssetMenu(menuName = "Skill/Info")]
    public class SkillInfo : ScriptableWithNameAndSpriteAndTooltip, IHasAction
    {
        [Serializable]
        private struct ExpertiseModule
        {
            [HideLabel, HorizontalGroup] public SkillExpertise Expertise;
            [HideLabel, HorizontalGroup(50)] public int Experience;
        }

        [OnValueChanged("OnExpertiseValueChanged", true)]
        [SerializeField] private ExpertiseModule[] _expertise;
        [SerializeField] private SkillAction[] _actions;
        [Space]
        [SerializeField] private ItemInfoWithActionsUI _actionsUI;

        public ItemInfoWithActionsUI ActionsUI => _actionsUI;
        public ActionBase[] Actions => _actions;
        public bool NeedsExpertise => _expertise.Length > 1;

        public SkillInfo UseSkill => this;

        public SkillExpertise GetExpertise (int experience)
        {
            if (_expertise.Length < 2) return SkillExpertise.Master;
            for (int i = 0; i < _expertise.Length; i++)
                if (experience < _expertise[i].Experience)
                    return _expertise[i - 1].Expertise;
            return SkillExpertise.Master;
        }

        public void HandleExtendedUI(ItemUI itemUI) => this.HandleExtendedUI(itemUI, _actionsUI);

        protected override void TooltipActionsSummary(ActorHolder actor, ref List<string> descriptions)
        {
            var count = _actions.Length;
            var expertise = (int)SkillExpertise.Master;

            if (UseSkill != null) expertise = (int)actor.Info.GetActionAmount(UseSkill);
            for (int i = 0; i < Mathf.Min(_actions.Length, count + 1); i++)
                if (expertise >= (int)_actions[i].Expertise)
                    _actions[i].TooltipSummary(actor, ref descriptions);
        }

#if UNITY_EDITOR
        private void OnExpertiseValueChanged()
        {
            var previous = 0;
            var max = Enum.GetValues(typeof(SkillExpertise)).Length;

            for (int i = 0; i < _expertise.Length; i++)
            {
                if (i >= max) { _expertise = _expertise[0..max]; break; }

                _expertise[i].Expertise = (SkillExpertise)i;

                if (i == 0) _expertise[i].Experience = 0;
                else if (_expertise[i].Experience <= previous)
                    _expertise[i].Experience = previous + 1;

                previous = _expertise[i].Experience;
            }
        }
#endif
    }
}