using UnityEngine;
using DG.Tweening;
using TMPro;

public class FlyingText : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private TMP_Text _text;
    [Space]
    [SerializeField] private float _duration;
    [SerializeField] private Vector2 _movement;
    [SerializeField] private Ease _ease;
    [SerializeField] private AnimationCurve _rendererFade, _textFade;
    [Space]
    [SerializeField] private TMP_ColorGradient _positiveColor;
    [SerializeField] private TMP_ColorGradient _negativeColor;

    public TMP_Text Text => _text;
    public SpriteRenderer Sprite => _renderer;

    public void Init(int value, Color textColor) => Init(null, Color.clear, value, textColor);
    public void Init(string value, Color textColor) => Init(null, Color.clear, value, textColor);

    public void Init(Sprite sprite, Color spriteColor, int value, Color textColor)
    {
        Init(sprite, spriteColor, value.ToString(), textColor);
        if (value >= 0) SetTextGradient(_positiveColor);
        else if (value < 0) SetTextGradient(_negativeColor);
    }

    public void Init(Sprite sprite, Color spriteColor, string text, Color textColor)
    {
        _renderer.sprite = sprite;
        _renderer.color = spriteColor;
        _text.text = text;
        _text.color = textColor;

        transform.localScale = Vector3.one * 1.3f;
        transform.DOScale(1, 0.3f);

        transform.DOMove(transform.position + (Vector3)_movement, _duration)
            .SetEase(_ease).onComplete += () => Destroy(gameObject);

        _renderer.DOFade(0, _duration).SetEase(_rendererFade);
        _text.DOFade(0, _duration).SetEase(_textFade);
    }

    private void OnDestroy()
    {
        DOTween.Kill(transform);
        DOTween.Kill(_renderer);
        DOTween.Kill(_text);
    }

    public void SetTextGradient(TMP_ColorGradient gradient)
    {
        if (gradient == null) return;

        _text.color = Color.white;
        _text.enableVertexGradient = true;
        _text.colorGradientPreset = gradient;
    }

    public void SetText(string text) => _text.text = text;
    public void SetTextColor(Color color) => _text.color = color;
    public void SetIcon(Sprite sprite) => _renderer.sprite = sprite;
}
