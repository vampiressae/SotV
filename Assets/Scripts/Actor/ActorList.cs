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

            var random = Random.value;
            var actors = Pick(true, actor => random < actor.Chance);
            foreach (var actor in actors)
                for (int i = 0; i < actor.RangeRandom; i++)
                    result.Add(actor.Item);

            return result;
        }
    }
}
