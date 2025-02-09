using UnityEngine;
using TMPro;

public class TooltipForTooltipString : TooltipForComponent
{
    [SerializeField] TMP_Text _text;

    public override void Init(Component target)
    {
        if (target is IHasTooltipString tts)
            _text.text = tts.TooltipString;
    }
}
