using VamporiumState.GO;

public class FightStateTurnMe : FightStateTurn
{
    protected override string ActorName => "Your Turn";

    public override void Enter(StateMachine machine)
    {
        base.Enter(machine);
        _isPlayerTurn.Value = true;
    }
}
