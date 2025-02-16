using System.Collections.Generic;
using UnityEngine;

namespace Actor
{
    [CreateAssetMenu(menuName = "Entity/Actor List")]
    public class ActorList : ScriptableObject
    {
        [SerializeField] private ActorInfo[] _actors;

        public IReadOnlyList<ActorInfo> Actors => _actors;
    }
}
