using Items;
using Entity;
using Actor;
using UnityEngine;
using Sirenix.Utilities;

public enum ActionTarget { None = 0, Any = 10, AllyAny = 110, AllySelf = 120, AllyOther = 130, EnemyAny = 210, EnemyLand = 220, EnemyAir = 230 }

public abstract class ActionInfo : ScriptableWithNameAndSprite
{
    public bool EndTurn;
    public ItemRank Rank;
    public ActionTarget Target;
    [Space]
    public StatInfo[] InfluencingStats;

    public virtual bool CanAct(Item item, ActorHolder source, EntityHolder target)
    {
        switch (Target)
        {
            case ActionTarget.AllyAny:
            case ActionTarget.AllySelf:
                return target == FightController.Instance.Player;

            case ActionTarget.AllyOther: return false;

            case ActionTarget.EnemyAny:
            case ActionTarget.EnemyLand:
            case ActionTarget.EnemyAir:
                return target == FightController.Instance.Enemy;

            default: return true;
        }
    }

    public virtual void Act(Item item, ActorHolder source, EntityHolder target)
    {
        if (EndTurn) FightController.Instance.EndTurnWithDelay();
    }

    public IntRange InfluencedValueRange(EntityInfo entity, IntRange intRange)
    {
        var multiplier = 0;
        InfluencingStats?.ForEach(stat => multiplier += entity.GetStatValue(stat));
        return intRange + intRange * multiplier;
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
