using Items;
using System.Collections.Generic;

public abstract class ScriptableListWithIntRangeChance<T> : ScriptableListWithIntRange<T> where T : Chances
{
    public List<T> PickWithChance(bool shuffle = true)
    {
        var random = UnityEngine.Random.value;
        return base.Pick(shuffle, item => item.Chance > random);
    }
}
