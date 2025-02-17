using UnityEngine;
using Sirenix.OdinInspector;

namespace Actor
{
    [CreateAssetMenu(menuName = "Scriptable Values/Player Info")]
    public class PlayerInfoValue : ScriptableObject
    {
        [SerializeField] private ActorInfo _defaultPlayer;

        [ShowInInspector, HideInEditorMode] private ActorInfo _player;
        [ShowInInspector, HideInEditorMode] public ActorHolder Holder { get; private set; }

        public ActorInfo Player => GetPlayer();

        private ActorInfo GetPlayer()
        {
            if (_player == null)
            {
                _player = Instantiate(_defaultPlayer);
                _player.Init();
            }
            return _player;
        }

        public void SetHolder(ActorHolder holder) => Holder = holder;
    }
}