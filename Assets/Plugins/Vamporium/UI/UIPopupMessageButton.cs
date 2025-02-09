using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Vamporium.UI;

public class UIPopupMessageButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _label;

    public Button Button => _button;

    private Action _action;

    public void Init(UIPopupMessage.Button button, Action action)
    {
        if (action != null) _action += action;
        _action += button.Action;
        _label.text = button.Label;
        _button.onClick.AddListener(Clicked);
    }

    private void Clicked() => _action?.Invoke();
}
