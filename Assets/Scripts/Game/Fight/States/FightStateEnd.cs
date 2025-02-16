using System.Collections.Generic;
using UnityEngine;
using Vamporium.UI;
using VamporiumState.GO;
using Items;

public class FightStateEnd : State
{
    [SerializeField] private UITag _victoryTag;
    [SerializeField] private UITag _containerTag;

    public override void Enter(StateMachine machine)
    {
        base.Enter(machine);
        Invoke(nameof(ShowSpoils), 4);
    }

    public void ShowSpoils()
    {
        UIManager.Hide(_victoryTag, 1);

        var enemies = FightController.Instance.Enemies;
        var popup = UIManager.Show(_containerTag);
        var spoils = new List<ItemData>();

        foreach (var enemy in enemies)
            spoils.AddRange(enemy.Info.Inventory);

        popup.GetComponent<UIPopupContainer>().Init("Spoils", spoils);
    }
}
