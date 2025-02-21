using Actor;
using System.Collections;
using UnityEngine;
using Vamporium.UI;
using VamporiumState.GO;

public class FightStateTurnOther : FightStateTurn
{
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
        var wait = new WaitForSeconds(1);
        var player = FightController.Player;
        var enemies = FightController.Enemies;

        yield return new WaitForSeconds(2);

        foreach (var enemy in enemies)
        {
            if (enemy.IsDead) continue;

            var ok = _ai.SimpleAttack(enemy, player);
            if (ok) yield return wait;
        }

        FightController.Instance.EndTurn();
    }
}
