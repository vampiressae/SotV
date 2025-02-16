using UnityEngine;
using VamporiumState.GO;
using Vamporium.UI;
using Actor;
using System.Collections.Generic;
using System.Linq;

[DefaultExecutionOrder(-999)]
public class FightController : MonoBehaviour
{
    public static FightController Instance;

    [SerializeField] private ActorHolder _player;
    [SerializeField] private List<ActorHolder> _enemies;
    [Space]
    [SerializeField] private ActorList _actorList;
    [SerializeField] private ActorHolder _actorPrefab;
    [SerializeField] private Transform _enemyParent;
    [SerializeField] private RoundsPerTurnValue _roundsPerTurn;
    [Space]
    [SerializeField] private UITag _screenUI;
    [SerializeField] private UITag _uiClickBlock;
    [SerializeField] private UITag _uiVictory;

    private StateMachine _stateMachine;

    public StateMachine StateMachine => _stateMachine;
    public ActorHolder Player => _player;
    public IReadOnlyList<ActorHolder> Enemies => _enemies;
    public RoundsPerTurnValue RoundsPerTurn => _roundsPerTurn;

    private void Awake()
    {
        Instance = this;
        _player.Info.Might.Regen(true);
        _stateMachine = GetComponent<StateMachine>();
        _roundsPerTurn.Value = 0;

        for (int i = 0; i < _enemies?.Count; i++)
            Destroy(_enemies[i].gameObject);

        _enemies.Clear();
        foreach (var info in _actorList.Actors)
        {
            var holder = Instantiate(_actorPrefab, _enemyParent);
            holder.Init(info);
            _enemies.Add(holder);
        }
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
        if (Enemies.Where(enemy => enemy.IsAlive).Count() > 0) return false;
        UIManager.Show(_uiVictory, delay:2);
        return true;
    }
}
