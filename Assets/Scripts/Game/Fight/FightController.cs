using UnityEngine;
using VamporiumState.GO;
using Vamporium.UI;
using Actor;

[DefaultExecutionOrder(-999)]
public class FightController : MonoBehaviour
{
    public static FightController Instance;

    [SerializeField] private ActorHolder _player, _enemy;
    [SerializeField] private RoundsPerTurnValue _roundsPerTurn;
    [Space]
    [SerializeField] private UITag _screenUI;
    [SerializeField] private UITag _uiClickBlock;
    [SerializeField] private UITag _uiVictory;

    private StateMachine _stateMachine;

    public StateMachine StateMachine => _stateMachine;
    public ActorHolder Player => _player;
    public ActorHolder Enemy => _enemy;
    public RoundsPerTurnValue RoundsPerTurn => _roundsPerTurn;

    private void Awake()
    {
        Instance = this;
        _player.Info.Might.Regen(true);
        _stateMachine = GetComponent<StateMachine>();
        _roundsPerTurn.Value = 0;
    }

    private void Start()
    {
        UIManager.Instance.ReloadScreenIfSameAsOld = true;
        UIManager.Show(_screenUI);
    }

    private void OnDestroy() => Instance = null;

    public void EndRound() => _roundsPerTurn.Value++;

    public void EndTurnWithDelay()
    {
        UIManager.Show(_uiClickBlock);
        Invoke(nameof(EndTurn), 1);
    }

    public void EndTurn()
    {
        UIManager.Hide(_uiClickBlock);

        if (EndFight()) return;

        _roundsPerTurn.Value = 0;

        if (_stateMachine.Current is FightStateTurnMe)
            _stateMachine.ChangeState<FightStateTurnOther>();
        else
        {
            _player.Info.Might.Regen();
            _stateMachine.ChangeState<FightStateTurnMe>();
        }
    }

    private bool EndFight()
    {
        if (_enemy.IsAlive) return false;
        UIManager.Show(_uiVictory, delay:2);
        return true;
    }
}
