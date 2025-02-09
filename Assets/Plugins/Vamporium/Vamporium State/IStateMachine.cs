namespace VamporiumState
{
    public interface IStateMachine
    {
        string CurrentStateName { get; }
        float TimeInState { get; }
    }
}
