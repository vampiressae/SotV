using UnityEngine;
using Sirenix.OdinInspector;
using Items;

[CreateAssetMenu(menuName = "Items/Armor")]
public class ArmorInfo : ItemInfo, IEquipmentItem 
{
    [Title("Armor", TitleAlignment = TitleAlignments.Centered)]
    [SerializeField] private int _equippedMight = -1;
    [SerializeField] private bool _canEquipOnlyOne;

    public bool CanEquipOnlyOne => _canEquipOnlyOne;
    public int EquippedMight => _equippedMight;
}
