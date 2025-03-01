using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Entity;
using Actor;
using Skills;

namespace Items
{
    public interface IHasAction
    {
        ActionBase[] Actions { get; }
        ItemInfoWithActionsUI ActionsUI { get; }
        SkillInfo UseSkill { get; }
        void HandleExtendedUI(ItemUI itemUI);
    }

    [Serializable]
    public abstract class ActionBase
    {
        public static event Action<ActorHolder, EntityHolder, Item, ActionBase> OnActed;

        [PropertyOrder(1), ShowIf("ShowExpertise")]
        [HorizontalGroup("1", 90), HideLabel] public SkillExpertise Expertise;

        public abstract string Name { get; }
        public abstract Sprite Icon { get; }
        public abstract ItemRank Rank { get; }
        public abstract int Might { get; }

        public bool Act(Item item, ActorHolder source, EntityHolder target, bool useMight = true)
        {
            if (source == null || target == null) return false;

            var ok = ActIt(item, source, target, useMight);
            if (!ok) return false;

            if (item != null)
                if (item.RawInfo is IItemHasStack stackable && stackable.Stack != 1)
                    if (item is IItemCanAdd canAdd)
                        canAdd.AddAmount(-1);

            FightController.Instance.EndRound();

            OnActed?.Invoke(source, target, item, this);
            return true;
        }

        protected abstract bool ActIt(Item item, ActorHolder source, EntityHolder target, bool useMight);

        public virtual void TooltipInit(ActorHolder actor, TooltipForString tooltip) { }
        public virtual void TooltipDescriptions(ActorHolder actor, ref List<string> descriptions) { }
        public virtual void TooltipSummary(ActorHolder actor, ref List<string> descriptions) { }

#if UNITY_EDITOR
        protected virtual bool ShowExpertise => true;
        public virtual void OnValidate(ItemInfo item) { }
#endif
    }

    public abstract class ActionWithInfo : ActionBase
    {
        public abstract ActionInfo RawInfo { get; }
        public override string Name => RawInfo ? RawInfo.Name : "[NO ACTION INFO WAS SET]";

        public override void TooltipSummary(ActorHolder actor, ref List<string> descriptions)
        {
            base.TooltipSummary(actor, ref descriptions);
            if (RawInfo.Properties.HasFlag(ActionInfo.Property.HideInTooltipSummary)) return;

            var color = $"<color=#{ColorUtility.ToHtmlStringRGBA(RawInfo.Rank.Color)}>";
            var details = TooltipSummaryDetails(actor, $"• {color}{RawInfo.Name}</color>");
            descriptions.Add(details);
        }

        public virtual string TooltipSummaryDetails(ActorHolder actor, string tooltip) => tooltip;
    }

    public abstract class ActionWithInfo<T> : ActionWithInfo where T : ActionInfo
    {
        [HorizontalGroup("1"), LabelText("Key"), LabelWidth(40)] public T Info;
        public override ActionInfo RawInfo => Info;
        public override Sprite Icon => Info.Icon;
        public override ItemRank Rank => Info.Rank;

        protected virtual bool CanActIt(Item item, ActorHolder source, EntityHolder target, bool useMight) => true;

        protected override bool ActIt(Item item, ActorHolder source, EntityHolder target, bool useMight)
        {
            if (!CanActIt(item, source, target, useMight)) return false;
            if (useMight && !Info.CanAct(item, source, target)) return false;

            Info.Act(item, source, target);
            return true;
        }

        public override void TooltipInit(ActorHolder actor, TooltipForString tooltip)
        {
            base.TooltipInit(actor, tooltip);

            var descriptions = new List<string>();
            TooltipDescriptions(actor, ref descriptions);
            if (Info.EndTurn) descriptions.Add("<i>Ends Turn</i>".ToValueString());

            tooltip.Init(Info.Name, string.Join("\n", descriptions), true);
            if (Info.Rank) Info.Rank.SetText(tooltip.Title);
        }
    }

    public abstract class ActionWithInfoMight<T> : ActionWithInfo<T> where T : ActionInfo
    {
        [HorizontalGroup("2"), LabelWidth(40), LabelText(" Might"), SerializeField] protected int _might;
        public override int Might => _might;

        protected override bool ActIt(Item item, ActorHolder source, EntityHolder target, bool useMight)
        {
            if (!base.ActIt(item, source, target, useMight)) return false;
            if (useMight)
            {
                var value = FightController.Instance.RoundsPerTurn.GetMightValue(source, _might);
                source.Info.Might.AddRecoverableValue(value);
            }
            return true;
        }

        public override void TooltipDescriptions(ActorHolder actor, ref List<string> descriptions)
        {
            base.TooltipDescriptions(actor, ref descriptions);
            var might = FightController.Instance.RoundsPerTurn.GetMightValue(actor, Might);
            descriptions.Add($"Cost: {might.ToValueString()} Might ({_might.ToInitialValueString(false)})");
        }
    }

    public abstract class ActionWithInfoValueMight<T> : ActionWithInfoMight<T> where T : ActionInfo
    {
        [HorizontalGroup("2", 155), InlineProperty, LabelWidth(40)] public IntRange Value;
        protected abstract string ValueName { get; }

        public IntRange InfluencedValueRange(EntityInfo entity) => Info.InfluencedValueRange(entity, Value);

        public override void TooltipDescriptions(ActorHolder actor, ref List<string> descriptions)
        {
            base.TooltipDescriptions(actor, ref descriptions);
            descriptions.Add($"{ValueName}: {InfluencedValueRange(actor.Info).ToValueString()} ({Value})");
        }
    }
}
