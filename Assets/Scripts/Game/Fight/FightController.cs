using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using VamporiumState.GO;
using Vamporium.UI;
using Actor;

[DefaultExecutionOrder(-999)]
public class FightController : MonoBehaviour
{
    public static FightController Instance;
    public static FightControllerSettings Settings => Instance._settings;

    public static ActorHolder Player => Instance._allies[0];
    public static IReadOnlyList<ActorHolder> Enemies => Instance._enemies;

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
    public RoundsPerTurnValue RoundsPerTurn => _roundsPerTurn;

    private void Awake()
    {
        Instance = this;
        _stateMachine = GetComponent<StateMachine>();
        _roundsPerTurn.Value = 0;

        if (_allies == null) _allies = new();
        else _allies.ForEach(ally => Destroy(ally.gameObject));
        SpawnActors(_alliesParent, new() { _playerInfoValue.Player }, _allies, true);

        if (_enemies == null) _enemies = new();
        else _enemies.ForEach(enemy => Destroy(enemy.gameObject));
        SpawnActors(_enemyParent, _actorList.PickActors(), _enemies, false);

        _playerInfoValue.SetHolder(_allies[0]);
        Player.Info.Might.Regen(true);
    }

    private void SpawnActors(ActorHolderParent parent, List<ActorInfo> infos, List<ActorHolder> holders, bool firstIsPlayer)
    {
        holders.Clear();
        for (int i = 0; i < infos.Count; i++)
        {
            var holder = Instantiate(_actorPrefab, parent.GetParent(i));
            var info = firstIsPlayer && i == 0 ? infos[i] : Instantiate(infos[i]);
            holder.Init(info);
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
