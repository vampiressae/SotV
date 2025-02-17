using UnityEngine;
using VamporiumState.GO;
using Vamporium.UI;
using Actor;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

[DefaultExecutionOrder(-999)]
public class FightController : MonoBehaviour
{
    public static FightController Instance;
    public static FightControllerSettings Settings => Instance._settings;

    [SerializeField] private ActorInfo _playerInfo;
    [Space]
    [SerializeField] private ActorList _actorList;
    [SerializeField] private ActorHolder _actorPrefab;
    [SerializeField] private ActorHolderParent _alliesParent, _enemyParent;
    [SerializeField] private RoundsPerTurnValue _roundsPerTurn;
    [SerializeField] protected PlayerInfoValue _playerInfoValue;
    [SerializeField] private FightControllerSettings _settings;
    [Space]
    [SerializeField] private UITag _screenUI;
    [SerializeField] private UITag _uiClickBlock;
    [SerializeField] private UITag _uiVictory;
    [Space]
    [ShowInInspector, HideInEditorMode, ReadOnly] private List<ActorHolder> _allies;
    [ShowInInspector, HideInEditorMode, ReadOnly] private List<ActorHolder> _enemies;

    private StateMachine _stateMachine;

    public StateMachine StateMachine => _stateMachine;
    public ActorHolder Player => _allies[0];
    public IReadOnlyList<ActorHolder> Enemies => _enemies;
    public RoundsPerTurnValue RoundsPerTurn => _roundsPerTurn;

    private void Awake()
    {
        Instance = this;
        _stateMachine = GetComponent<StateMachine>();
        _roundsPerTurn.Value = 0;

        if (_allies == null) _allies = new();
        else _allies.ForEach(ally => Destroy(ally.gameObject));
        SpawnActors(_alliesParent, new ActorInfo[] { _playerInfo }, _allies);

        if (_enemies == null) _enemies = new();
        else _enemies.ForEach(enemy => Destroy(enemy.gameObject));
        SpawnActors(_enemyParent, _actorList.Actors.ToArray(), _enemies);

        _playerInfoValue.SetHolder(Player);
        Player.Info.Might.Regen(true);
    }

    private void SpawnActors(ActorHolderParent parent, ActorInfo[] infos, List<ActorHolder> holders)
    {
        holders.Clear();
        for (int i = 0; i < infos.Length; i++)
        {
            var holder = Instantiate(_actorPrefab, parent.GetParent(i));
            holder.Init(infos[i]);
            holders.Add(holder);
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
            Player.Info.Might.Regen();
            _stateMachine.ChangeState<FightStateTurnMe>();
        }
    }

    private bool EndFight()
    {
        if (Enemies.Where(enemy => enemy.IsAlive).Count() > 0) return false;
        UIManager.Show(_uiVictory, delay: 2);
        return true;
    }
}
