using UnityEngine;
using Vamporium.UI;
using VamporiumState.GO;

public abstract class FightStateTurn : FightState
{
    [SerializeField] private UITag _popupTag;

    protected abstract string ActorName { get; }

    public override void Enter(StateMachine machine)
    {
        base.Enter(machine);
        if (UIManager.Show(_popupTag).TryGetComponent<UIPopupFightTurn>(out var ui))
            ui.Init(ActorName);
    }
}
