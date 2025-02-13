using TMPro;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(menuName = "Items/Rank")]
    public class ItemRank : ScriptableWithNameAndSprite
    {
        public Color Color = Color.gray;
        public Material Material;
        public TMP_ColorGradient Gradient;

        public void SetText(TMP_Text text)
        {
            text.enableVertexGradient = Gradient;
            if (Gradient) text.colorGradientPreset = Gradient;
            else text.color = Color;
        }
    }
}
