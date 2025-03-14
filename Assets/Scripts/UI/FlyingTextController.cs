using UnityEngine;
using TMPro;

public interface IFlyingText
{
    Vector2 Position { get; }
    float Radius { get; }
}

public class FlyingTextController : MonoBehaviour
{
    public static FlyingTextController Instance;

    [SerializeField] private FlyingText _prefab;

    [SerializeField] private TMP_ColorGradient _positiveColor;
    [SerializeField] private TMP_ColorGradient _negativeColor;

    private void Awake() => Instance = this;
    private void OnDestroy() => Instance = null;

    public static FlyingText Show(IFlyingText source, string text) => Instance.ShowIt(source, null, Color.clear, text, Color.white);
    public static FlyingText Show(IFlyingText source, string text, Color color) => Instance.ShowIt(source, null, Color.clear, text, color);
    public static FlyingText Show(IFlyingText source, Sprite sprite)  => Instance.ShowIt(source, sprite, Color.white, string.Empty, Color.clear);
    public static FlyingText Show(IFlyingText source, Sprite sprite, Color color)  => Instance.ShowIt(source, sprite, color, string.Empty, Color.clear);
    public static FlyingText Show(IFlyingText source, Sprite sprite, Color spriteColor, string text, Color textColor) => Instance.ShowIt(source, sprite, spriteColor, text, textColor);

    public static FlyingText Show(IFlyingText source, int value) => Instance.ShowIt(source, null, Color.clear, value, Color.white);
    public static FlyingText Show(IFlyingText source, int value, Color color) => Instance.ShowIt(source, null, Color.clear, value, color);
    public static FlyingText Show(IFlyingText source, Sprite sprite, Color spriteColor, int value, Color textColor) => Instance.ShowIt(source, sprite, spriteColor, value, textColor);

    private FlyingText ShowIt(IFlyingText source, Sprite sprite, Color spriteColor, string text, Color textColor)
    {
        var txt = Instantiate(_prefab, source.Position + Random.insideUnitCircle * source.Radius, default, transform);
        txt.Init(sprite, spriteColor, text, textColor);
        return txt;
    }

    private FlyingText ShowIt(IFlyingText source, Sprite sprite, Color spriteColor, int value, Color textColor)
    {
        var txt = Instantiate(_prefab, source.Position + Random.insideUnitCircle * source.Radius, default, transform);
        txt.Init(sprite, spriteColor, value, textColor);
        return txt;
    }
}
