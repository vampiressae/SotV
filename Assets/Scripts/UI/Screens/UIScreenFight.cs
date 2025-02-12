using UnityEngine;
using Inventory;
using Actor;
using Skills;

public class UIScreenFight : MonoBehaviour
{
    [SerializeField] private ActorMightUI _might;
    [SerializeField] private InventoryHolder _inventory, _equipment;
    [SerializeField] private SkillsUIFight _skills;

    private void Awake()
    {
        var player = FightController.Instance.Player;

       // _might.Init(player.Info.Might);
        //_inventory.Init(player.Info.Inventory, player);
        //_equipment.Init(player.Info.Equipment, player);
        //_skills.Init(player.Info.Skills, player);

        player.InitInventories(_inventory, _equipment);
    }
}
