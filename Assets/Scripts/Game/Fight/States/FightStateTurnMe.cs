using VamporiumState.GO;

public class FightStateTurnMe : FightStateTurn
{
    protected override string ActorName => "Your Turn";

    public override void Enter(StateMachine machine)
    {
        base.Enter(machine);
        _isPlayerTurn.Value = true;

        FightController.Player.Info.OnTurnStart();
    }

    public override void Exit(StateMachine machine)
    {
        base.Exit(machine);

        FightController.Player.Info.OnTurnEnd();
    }
}
