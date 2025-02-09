using UnityEngine;

public interface IFlyingText
{
    Vector2 Position { get; }
    float Radius { get; }
}

public class FlyingTextController : MonoBehaviour
{
    public static FlyingTextController Instance;

    [SerializeField] private FlyingText _prefab;

    private void Awake() => Instance = this;
    private void OnDestroy() => Instance = null;

    public static void Show(IFlyingText source, string text)
        => Instance.ShowIt(source, null, Color.clear, text, Color.white);

    public static void Show(IFlyingText source, string text, Color color)
        => Instance.ShowIt(source, null, Color.clear, text, color);

    public static void Show(IFlyingText source, Sprite sprite) 
        => Instance.ShowIt(source, sprite, Color.white, string.Empty, Color.clear);

    public static void Show(IFlyingText source, Sprite sprite, Color color) 
        => Instance.ShowIt(source, sprite, color, string.Empty, Color.clear);

    public static void Show(IFlyingText source, Sprite sprite, Color spriteColor, string text, Color textColor) 
        => Instance.ShowIt(source, sprite, spriteColor, text, textColor);

    private void ShowIt(IFlyingText source, Sprite sprite, Color spriteColor, string text, Color textColor)
    {
        var txt = Instantiate(_prefab, source.Position + Random.insideUnitCircle * source.Radius, default, transform);
        txt.Init(sprite, spriteColor, text, textColor);
    }
}
