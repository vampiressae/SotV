using UnityEngine;
using Entity;
using Items;
using Actor;

namespace Skills
{
    public class SkillSlotUI : SlotUI<SkillData, SkillInfo>, IHasAttachedEntity
    {
        [SerializeField] private SkillItemUI _itemPrefab;

        public EntityHolder Entity => _actor;
        protected override ItemUI<SkillData, SkillInfo> Prefab => _itemPrefab;

        private ActorHolder _actor;

        public void Init(SkillData data, ActorHolder actor)
        {
            _actor = actor;
            base.Init(data);
        }

        protected override void OnItemUIRefresh()
        {
            base.OnItemUIRefresh();
            if (Data.Info is IHasAction hasAction)
                hasAction.HandleExtendedUI(RawItemUI);
        }

        protected override void Init(TooltipForString tooltip)
            => RawData.TooltipInit(_actor, tooltip);
    }
}
