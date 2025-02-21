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

        var enemies = FightController.Enemies;
        var popup = UIManager.Show(_containerTag);
        var loot = new List<ItemData>();

        foreach (var enemy in enemies)
            loot.Add(enemy.Info.GetLoot());

        popup.GetComponent<UIPopupContainer>().Init("Spoils", loot);
    }
}
