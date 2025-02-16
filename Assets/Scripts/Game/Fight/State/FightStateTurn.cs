using System.Linq;
using UnityEngine;
using Vamporium.UI;
using VamporiumState.GO;

public abstract class FightStateTurn : FightState
{
    [SerializeField] private UITag _popupTag;
    [SerializeField] private UITag _victoryTag;
    [SerializeField] private UITag _defeatTag;

    protected abstract string ActorName { get; }

    public override void Enter(StateMachine machine)
    {
        base.Enter(machine);

        FightController.Instance.Player.Info.Might.OnAnyValueChanged += OnAnyMightChanged;
        FightController.Instance.Enemies[0].Info.Might.OnAnyValueChanged += OnAnyMightChanged;

        if (UIManager.Show(_popupTag).TryGetComponent<UIPopupFightTurn>(out var ui))
            ui.Init(ActorName);
    }

    public override void Exit(StateMachine machine)
    {
        base.Exit(machine);
        FightController.Instance.Player.Info.Might.OnAnyValueChanged -= OnAnyMightChanged;
        FightController.Instance.Enemies[0].Info.Might.OnAnyValueChanged -= OnAnyMightChanged;
    }

    private void OnAnyMightChanged()
    {
        if (EndInVictory() || EndInDefeat())
            FightController.Instance.StateMachine.ChangeState<FightStateEnd>();
    }

    private bool EndInVictory()
    {
        if (FightController.Instance.Enemies.Where(enemy => enemy.IsAlive).Count() > 0) return false;
        UIManager.Show(_victoryTag, delay: 2);
        return true;
    }

    private bool EndInDefeat()
    {
        if (FightController.Instance.Player.IsAlive) return false;
        UIManager.Show(_defeatTag, delay: 2);
        return true;
    }
}
