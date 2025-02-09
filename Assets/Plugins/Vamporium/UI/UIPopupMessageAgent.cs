using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Vamporium.UI;

public class UIPopupMessageAgent : MonoBehaviour
{
    [Serializable]
    private class Module
    {
        public string Label;
        public bool Close = true;
        public UnityEvent Event;
    }

    [SerializeField] private string _title, _text;
    [Space]
    [SerializeField] private Module[] _buttons;

    private Dictionary<UIPopupMessage.Button, Module> _events = new();

    protected virtual string _showTitle => _title;
    protected virtual string _showText => _text;

    public virtual void Show()
    {
        var ui = UIManager.ShowMessage();
        ui.Init(_showTitle, _showText);

        var buttons = new UIPopupMessage.Button[_buttons.Length];

        for (int i = 0; i < _buttons.Length; i++)
        {
            var button = new UIPopupMessage.Button(_buttons[i].Label, close: _buttons[i].Close);
            button.Action += () => InvokeEvent(button);
            buttons[i] = button;

            _events.Add(button, _buttons[i]);
        }
        ui.SetButtons(buttons);
    }

    private void InvokeEvent(UIPopupMessage.Button button)
    {
        if (_events.TryGetValue(button, out var module))
            module.Event.Invoke();
    }
}
