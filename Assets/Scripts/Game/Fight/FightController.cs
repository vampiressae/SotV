using UnityEngine;
using VamporiumState.GO;
using Actor;

[DefaultExecutionOrder(-999)]
public class FightController : MonoBehaviour
{
    public static FightController Instance;

    [SerializeField] private ActorHolder _player, _enemy;
    [SerializeField] private RoundsPerTurnValue _roundsPerTurn;

    private StateMachine _stateMachine;

    public StateMachine StateMachine => _stateMachine;
    public ActorHolder Player => _player;
    public ActorHolder Enemy => _enemy;
    public RoundsPerTurnValue RoundsPerTurn => _roundsPerTurn;

    private void Awake()
    {
        Instance = this;
        _stateMachine = GetComponent<StateMachine>();
        _roundsPerTurn.Value = 0;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void EndRound() => _roundsPerTurn.Value++;
    
    public void EndTurnWithDelay()
    {
        Invoke(nameof(EndTurn), 1);
    }
    
    public void EndTurn()
    {
        _roundsPerTurn.Value = 0;

        if (_stateMachine.Current is FightStateTurnMe)
            _stateMachine.ChangeState<FightStateTurnOther>();
        else
        {
            _player.Info.Might.Regen();
            _stateMachine.ChangeState<FightStateTurnMe>();
        }
    }
}
