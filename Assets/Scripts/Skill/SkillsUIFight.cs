using System.Collections.Generic;
using UnityEngine;
using Actor;
using Inventory;

namespace Skills
{
    public class SkillsUIFight : MonoBehaviour
    {
        public ActorHolder Actor;
        public List<SkillData> Skills = new();

        [SerializeField] private SkillSlotUI _slotPrefab;

        private readonly List<InventorySlotUI> _uis = new();

        public void Init(List<SkillData> skills, ActorHolder actor = null)
        {
            Actor = actor;
            Skills = skills;

            foreach (var skill in Skills)
            {
                var ui = Instantiate(_slotPrefab, transform);
                ui.Init(skill, Actor);
            }
            _uis.ForEach(ui => ui.OnDataChanged());
        }
    }
}
