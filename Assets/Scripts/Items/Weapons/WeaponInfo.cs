using UnityEngine;
using Sirenix.OdinInspector;
using Items;

[CreateAssetMenu(menuName = "Items/Weapon")]
public class WeaponInfo : ItemInfoWithAction<WeaponAction>, IEquipmentItem
{
    [Title("Weapon", TitleAlignment = TitleAlignments.Centered)]
    [SerializeField, HideLabel] private WeaponTypeInfo _type;
    [SerializeField, ToggleLeft] private bool _canEquipOnlyOne;

    public bool CanEquipOnlyOne => _canEquipOnlyOne;

    public ScriptableObject CanEquipKey => _type;
}
