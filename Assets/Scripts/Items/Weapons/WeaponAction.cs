using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Items;
using Entity;
using Actor;
using Affliction;

[System.Serializable]
public class WeaponAction : ActionWithInfoValueMight<WeaponAttackInfo>
{
    [ListDrawerSettings(CustomRemoveElementFunction = "RemoveEffect", CustomAddFunction = "AddEffect")]
    [SerializeField, InlineEditor, GUIColor(1, 0.9f, 0.9f)] private List<AfflictionInfo> _afflictions;

    [ShowInInspector, ReadOnly, HideIf(nameof(ParentScriptable)), GUIColor("red")]
    public ScriptableObject ParentScriptable { get; private set; }

    protected override string ValueName => "Damage";

    protected override bool ActIt(Item item, ActorHolder source, EntityHolder target, bool useMight)
    {
        if (!base.ActIt(item, source, target, useMight)) return false;
        if (target is not ActorHolder actor) return false;

        actor.Info.Hurt(InfluencedValueRange(source.Info).RandomUnity);
        actor.Info.AddAfflictions(_afflictions);
        return true;
    }

    public override string TooltipSummaryDetails(ActorHolder actor, string tooltip) 
        => $"{base.TooltipSummaryDetails(actor, tooltip)} - {(actor ? InfluencedValueRange(actor.Info) : 0)} : {Might}";

#if UNITY_EDITOR
    private void AddEffect() => _afflictions.AddScriptableCopy(ParentScriptable);
    private void RemoveEffect(AfflictionInfo item) => _afflictions.RemoveScriptableCopy(item);
    public override void OnValidate(ItemInfo item)
    {
        ParentScriptable = item;
        _afflictions.ForEach(affliction => affliction.ShowMainData = false);
    }
#endif
}
