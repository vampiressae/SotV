using System.Collections.Generic;
using UnityEngine;
using Vamporium.UI;
using VamporiumState.GO;
using Items;

public class FightStateEnd : State
{
    [SerializeField] private UITag _victoryTag;
    [SerializeField] private UITag _defeatTag;
    [SerializeField] private UITag _containerTag;

    public void SetEnd(bool win)
    {
        UIManager.Show(win ? _victoryTag : _defeatTag, delay: 2);
        if (win) Invoke(nameof(ShowSpoils), 4);
    }

    public void ShowSpoils()
    {
        UIManager.Hide(_victoryTag, 1);

        var enemies = FightController.Instance.Enemies;
        var popup = UIManager.Show(_containerTag);
        var spoils = new List<ItemData>();

        foreach (var enemy in enemies)
            spoils.Add(enemy.Info.Inventory);

        popup.GetComponent<UIPopupContainer>().Init("Spoils", spoils);
    }
}
