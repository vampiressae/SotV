using Sirenix.OdinInspector;
using UnityEngine;

namespace Entity
{
    [CreateAssetMenu(menuName = "Entity/Stat Info")]
    public class StatInfo : ScriptableWithNameAndSprite
    {
        [LabelWidth(70)] public int Max;
        [LabelWidth(70)] public float Influence;
    }
}

