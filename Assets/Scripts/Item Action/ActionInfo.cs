using System.Linq;
using UnityEngine;
using Sirenix.Utilities;
using Items;
using Entity;
using Actor;

public abstract class ActionInfo : ScriptableWithNameAndSprite
{
    [System.Flags] public enum Property { None, HideInTooltipSummary }

    public bool EndTurn;
    public ItemRank Rank;
    public ActionTarget Target;
    public Property Properties;
    [Space]
    public StatInfo[] InfluencingStats;

    public virtual bool CanAct(Item item, ActorHolder source, EntityHolder target)
    {
        switch (Target)
        {
            case ActionTarget.AllyAny:
            case ActionTarget.AllySelf:
                return target == FightController.Player;

            case ActionTarget.AllyOther: return false;

            case ActionTarget.EnemyAny:
            case ActionTarget.EnemyLand:
            case ActionTarget.EnemyAir:
                return FightController.Enemies.Contains(target);

            default: return true;
        }
    }

    public virtual void Act(Item item, ActorHolder source, EntityHolder target)
    {
        if (EndTurn) FightController.Instance.EndTurnWithDelay();
    }

    public IntRange InfluencedValueRange(EntityInfo entity, IntRange intRange)
    {
        var multiplier = 0f;
        InfluencingStats?.ForEach(stat => multiplier += entity.GetInfluencedStatValue(stat));
        return intRange + Mathf.FloorToInt(multiplier);
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        if (Rank == null)
        {
            Rank = EditorUtils.LoadAssetsSafeFirst<ItemRank>("Item Rank 0");
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
#endif
}
