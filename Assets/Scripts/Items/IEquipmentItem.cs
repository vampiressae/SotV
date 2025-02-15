
using UnityEngine;

public interface IEquipmentItem
{
    ScriptableObject CanEquipKey { get; }
    bool CanEquipOnlyOne { get; }
}
