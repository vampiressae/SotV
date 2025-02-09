using UnityEngine;

namespace Items
{
    [CreateAssetMenu(menuName = "Items/Rank")]
    public class ItemRank : ScriptableWithNameAndSprite
    {
        public Color Color = Color.gray;
        public Material Material;
    }
}
