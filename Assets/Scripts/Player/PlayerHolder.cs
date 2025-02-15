using UnityEngine;

namespace Actor
{
    public class PlayerHolder : ActorHolder
    {
        [Space]
        [SerializeField] private PlayerInfoValue _playerInfoValue;

        public override ActorInfo Info => _playerInfoValue.Player;

        private void Awake() => _playerInfoValue.SetHolder(this);

#if UNITY_EDITOR
        protected override bool ShowInfoInInspector => false;
#endif
    }
}