using UnityEngine;
using Sirenix.OdinInspector;
using Items;

[CreateAssetMenu(menuName = "Items/Armor")]
public class ArmorInfo : ItemInfo, IEquipmentItem
{
    [Title("Armor", TitleAlignment = TitleAlignments.Centered)]
    [SerializeField, ToggleLeft] private bool _canEquipOnlyOne;
    [SerializeField, Indent, HideLabel, ShowIf(nameof(_canEquipOnlyOne))] private ScriptableObject _canEquipKey;

    public bool CanEquipOnlyOne => _canEquipOnlyOne;
    public ScriptableObject CanEquipKey => _canEquipKey;
}
