using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIWithStringAndIcon : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Image _icon;

    public TMP_Text Text => _text;
    public Image Icon => _icon;

    public void Init(string text, Sprite icon)
    {
        _text.text = text;
        _icon.sprite = icon;
    }
}
