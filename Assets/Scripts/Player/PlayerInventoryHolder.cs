using Actor;
using Inventory;
using UnityEngine;

public class PlayerInventoryHolder : InventoryHolder
{
    [Space]
    [SerializeField] protected PlayerInfoValue _playerInfoValue;

    protected void Awake() => Init(_playerInfoValue.Holder);
}
