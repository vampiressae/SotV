using Actor;
using Inventory;
using UnityEngine;

public class PlayerEquipmentHolder : EquipmentHolder
{
    [Space]
    [SerializeField] protected PlayerInfoValue _playerInfoValue;

    protected void Awake() => Init(_playerInfoValue.Holder);
}
