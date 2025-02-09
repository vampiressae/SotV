using VamporiumState.GO;

public class FightStateStart : FightState
{
    public override void Enter(StateMachine machine)
    {
        base.Enter(machine);
        _isPlayerTurn.Value = false;
    }

    public override void Exit(StateMachine machine)
    {
        base.Exit(machine);
    }

    public override void UpdateLogics(StateMachine machine)
    {
        base.UpdateLogics(machine);
        if (machine.TimeInState > 0.3f)
            machine.ChangeState<FightStateTurnMe>();
    }
}
