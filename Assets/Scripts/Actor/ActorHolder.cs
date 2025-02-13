using UnityEngine;
using Sirenix.OdinInspector;
using Inventory;
using Entity;

namespace Actor
{
    public class ActorHolder : EntityHolder
    {
        [Space]
        [SerializeField, ShowIf("ShowInfoInInspector")] private ActorInfo _info;

        public virtual ActorInfo Info
        {
            get
            {
                if (!_inited)
                {
                    _info = Instantiate(_info);
                    _info.Init();
                    _inited = true;
                }
                return _info;
            }
        }

        public bool IsAlive => Info && Info.Might.Alive;
        public bool IsDead => Info == null || Info.Might.Dead;

        private bool _inited;

#if UNITY_EDITOR
        protected virtual bool ShowInfoInInspector => true;
#endif
    }
}
