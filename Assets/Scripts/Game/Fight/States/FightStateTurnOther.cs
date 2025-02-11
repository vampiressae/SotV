using Actor;
using System.Collections;
using UnityEngine;
using Vamporium.UI;
using VamporiumState.GO;

public class FightStateTurnOther : FightStateTurn
{
    [SerializeField] private ActorHolder _player, _enemy;
    [SerializeField] AIControllerForFight _ai;
    [SerializeField] private UITag _uiFightEnemy;

    protected override string ActorName => "Enemy Turn";

    public override void Enter(StateMachine machine)
    {
        base.Enter(machine);
        _isPlayerTurn.Value = false;

        StartCoroutine(Act());
        UIManager.Show(_uiFightEnemy);
    }

    public override void Exit(StateMachine machine)
    {
        base.Exit(machine);
        StopAllCoroutines();
        UIManager.Hide(_uiFightEnemy);
    }

    private IEnumerator Act()
    {
        yield return new WaitForSeconds(2);

        var ok = _ai.SimpleAttack(_enemy, _player);
        if (ok) yield return new WaitForSeconds(1);

        FightController.Instance.EndTurn();
    }
}
