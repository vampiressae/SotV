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
        Invoke(nameof(ShowSpoils), 2.1f);
    }

    public void ShowSpoils()
    {
        var popup = UIManager.Show(_containerTag, delay: 2);
        var spoils = new List<ItemData>();

        spoils.AddRange(FightController.Instance.Enemy.Info.Inventory);
        popup.GetComponent<UIPopupContainer>().Init("Spoils", spoils);
    }
}
