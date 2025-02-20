using System.Collections.Generic;
using UnityEngine;

namespace Actor
{
    [CreateAssetMenu(menuName = "Entity/Actor List")]
    public class ActorList : ScriptableListWithIntRange<ActorInfoWithChance>
    {
        public List<ActorInfo> PickActors()
        {
            var result = new List<ActorInfo>();

            foreach(var actor in Pick(true))
                for (int i = 0; i < actor.RangeRandom; i++)
                    result.Add(actor.Item);

            return result;
        }
    }
}
