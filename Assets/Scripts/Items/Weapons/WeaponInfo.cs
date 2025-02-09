using UnityEngine;
using Sirenix.OdinInspector;
using Items;

[CreateAssetMenu(menuName = "Items/Weapon")]
public class WeaponInfo : ItemInfoWithAction<WeaponAction>, IEquipmentItem
{
    [Title("Weapon", TitleAlignment = TitleAlignments.Centered)]
    [SerializeField, HideLabel] private WeaponTypeInfo _type;
    [SerializeField] private int _equippedMight = -1;
    [SerializeField] private bool _canEquipOnlyOne;

    public bool CanEquipOnlyOne => _canEquipOnlyOne;
    public int EquippedMight => _equippedMight;
}
