using UnityEngine;
using VamporiumState.GO;

public abstract class FightState : State
{
    [SerializeField] protected ScriptableBool _isPlayerTurn;
}
