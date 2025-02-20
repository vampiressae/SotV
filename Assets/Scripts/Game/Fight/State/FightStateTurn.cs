using NUnit.Framework.Constraints;
using System.Linq;
using UnityEngine;
using Vamporium.UI;
using VamporiumState.GO;

public abstract class FightStateTurn : FightState
{
    private enum EndMode { None, Victory, GameOver }

    [SerializeField] private UITag _popupTag;

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
        var end = EndInVictory();
        if (end == EndMode.None) end = EndInDefeat();
        if (end == EndMode.None) return;

        FightController.Instance.StateMachine.ChangeState<FightStateEnd>()
            .SetEnd(end == EndMode.Victory);
    }

    private EndMode EndInVictory()
        => FightController.Instance.Enemies.Where(enemy => enemy.IsAlive).Count() > 0 ? EndMode.None : EndMode.Victory;

    private EndMode EndInDefeat()
        => FightController.Instance.Player.IsAlive ? EndMode.None : EndMode.GameOver;
}
