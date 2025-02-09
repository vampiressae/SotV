using UnityEngine;
using TMPro;

public static class RendererUtils 
{
    public static void Fade(this SpriteRenderer renderer, float alpha)
    {
        if (renderer == null) return;
        var color = renderer.color;
        color.a = alpha;
        renderer.color = color;
    }

    public static void Fade(this TMP_Text text, float alpha)
    {
        if (text == null) return;
        var color = text.color;
        color.a = alpha;
        text.color = color;
    }

}
