using UnityEngine;
using TMPro;

public class TooltipForString : Tooltip
{
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private GameObject _divider;

    public TMP_Text Title => _title;
    public TMP_Text Text => _text;

    public TooltipForString Init(string text)
    {
        _text.text = text;

        _title.gameObject.SetActive(false);
        _divider.SetActive(false);
        return this;
    }

    public TooltipForString Init(string title, string text, bool divider = false)
    {
        Init(text);
        _title.text = title;

        _title.gameObject.SetActive(true);
        _divider.SetActive(divider);
        return this;
    }
}
