﻿using System.Collections.Generic;
using UnityEngine;
using Actor;
using Inventory;

namespace Skills
{
    public class SkillsUI : MonoBehaviour
    {
        [SerializeField] protected PlayerInfoValue _playerInfoValue;

        public ActorHolder Actor;
        public List<SkillData> Skills = new();

        [SerializeField] private SkillSlotUI _slotPrefab;

        private readonly List<InventorySlotUI> _uis = new();

        protected virtual bool ShowSkillsWithoutActions => true;
        public virtual bool ShowActions => false;

        private void Awake() => Init(_playerInfoValue.Holder);

        public void Init(ActorHolder actor)
        {
            Actor = actor;
            Init(Actor.Info.Skills);
        }

        public void Init(List<SkillData> skills)
        {
            Skills = skills;

            foreach (var skill in Skills)
            {
                if (!ShowSkillsWithoutActions)
                    if (skill.Info.Actions.Length == 0)
                        continue;
                var ui = Instantiate(_slotPrefab, transform);
                ui.Init(this, skill, Actor);
            }
            _uis.ForEach(ui => ui.OnDataChanged());
        }
    }
}
