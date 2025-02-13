using UnityEngine;
using Inventory;
using Actor;
using Skills;

public class UIScreenFight : MonoBehaviour
{
    [SerializeField] private ActorMightUI _might;
    [SerializeField] private InventoryHolder _inventory, _equipment;
    [SerializeField] private SkillsUIFight _skills;
}
