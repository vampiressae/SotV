using Sirenix.Utilities;
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

        FightController.Player.Info.Might.OnAnyValueChanged += OnAnyMightChanged;
        FightController.Enemies.ForEach(enemy => enemy.Info.Might.OnAnyValueChanged += OnAnyMightChanged);

        if (UIManager.Show(_popupTag).TryGetComponent<UIPopupFightTurn>(out var ui))
            ui.Init(ActorName);
    }

    public override void Exit(StateMachine machine)
    {
        base.Exit(machine);
        FightController.Player.Info.Might.OnAnyValueChanged -= OnAnyMightChanged;
        FightController.Enemies.ForEach(enemy => enemy.Info.Might.OnAnyValueChanged -= OnAnyMightChanged);
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
        => FightController.Enemies.Where(enemy => enemy.IsAlive).Count() > 0 ? EndMode.None : EndMode.Victory;

    private EndMode EndInDefeat()
        => FightController.Player.IsAlive ? EndMode.None : EndMode.GameOver;
}
