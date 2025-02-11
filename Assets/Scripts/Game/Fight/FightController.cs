using UnityEngine;
using VamporiumState.GO;
using Actor;
using Vamporium.UI;

[DefaultExecutionOrder(-999)]
public class FightController : MonoBehaviour
{
    public static FightController Instance;

    [SerializeField] private UITag _screenUI;
    [SerializeField] private ActorHolder _player, _enemy;
    [SerializeField] private RoundsPerTurnValue _roundsPerTurn;
    [SerializeField] private UITag _uiClickBlock;

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

    private void Start() => UIManager.Show(_screenUI);

    private void OnDestroy()
    {
        Instance = null;
    }

    public void EndRound() => _roundsPerTurn.Value++;
    
    public void EndTurnWithDelay()
    {
        UIManager.Show(_uiClickBlock);
        Invoke(nameof(EndTurn), 1);
    }
    
    public void EndTurn()
    {
        UIManager.Hide(_uiClickBlock);
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
