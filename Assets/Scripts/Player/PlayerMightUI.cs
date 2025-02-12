using Actor;
using UnityEngine;

public class PlayerMightUI : ActorMightUI
{
    [Space]
    [SerializeField] protected PlayerInfoValue _playerInfoValue;

    protected override void Awake()
    {
        Init(_playerInfoValue.Player.Might);
        base.Awake();
    }
}
