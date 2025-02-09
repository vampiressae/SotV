using UnityEngine;
using Items;

[CreateAssetMenu(menuName = "Items/Transfer Stat")]
public class ItemInfoTransferStat : ItemInfoWithAction<TransferStatAction>, IEquipmentItem
{
    [SerializeField] private int _equippedMight = -1;

    public bool CanEquipOnlyOne => false;
    public int EquippedMight => _equippedMight;
}
