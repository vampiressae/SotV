using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.Utilities;
using Vamporium.UI;
using TMPro;
using Sirenix.OdinInspector;

public class UIPopupMessage : UIPopup
{
    [Serializable]
    public struct Button
    {
        public string Label;
        public Action Action;
        public bool Close;

        public Button(string label, Action action = null, bool close = true)
        {
            Label = label;
            Action = action;
            Close = close;
        }
    }

    [SerializeField] private TMP_Text _title, _body;
    [SerializeField] private Image _image;
    [Space]
    [SerializeField] private Transform _dividerPrefab;
    [SerializeField] private UIPopupMessageButton _buttonPrefab;
    [SerializeField] private Transform _buttonParent;
    [Space]
    [SerializeField] private LayoutElement _textLayout;
    [SerializeField] private int _maxWidth = 800, _maxHeight = 500;

    private Coroutine _autoClose;
    private List<UIPopupMessageButton> _buttons = new();

    public bool CanBeClosed { get; set; }
    public bool HasAutoClose => _autoClose != null;

    public UIPopupMessage Init(string title, string body)
    {
        var hasTitle = !string.IsNullOrEmpty(title);

        if (hasTitle) _title.text = title;
        else _title.gameObject.SetActive(false);

        //foreach (Transform t in _buttonParent) Destroy(t.gameObject);
        _buttonParent.gameObject.SetActive(false);

        _body.text = body;
        if (_image) _image.gameObject.SetActive(false);

        ClearInstantiatedElements();
        StartCoroutine(SetLayout());

        return this;
    }

    [Button]
    private IEnumerator SetLayout()
    {
        yield return null;
        _textLayout.enabled = true;

        if ((_textLayout.transform as RectTransform).rect.width > _maxWidth)
            _textLayout.preferredWidth = _maxWidth;
        else _textLayout.enabled = false;
    }

    private void ClearInstantiatedElements()
    {
        foreach (Transform t in _buttonParent)
            Destroy(t.gameObject);
        _buttons.Clear();
        CanBeClosed = false;
    }

    public UIPopupMessage SetSprite(Sprite sprite)
    {
        if (_image)
        {
            _image.sprite = sprite;
            _image.gameObject.SetActive(sprite);
        }
        return this;
    }

    public UIPopupMessage SetButtons(params Button[] buttons)
    {
        if (buttons.IsNullOrEmpty()) return this;

        _buttonParent.gameObject.SetActive(true);

        for (int i = 0; i < buttons.Length; i++)
        {
            if (i > 0 && _dividerPrefab)
                Instantiate(_dividerPrefab, _buttonParent);

            var button = Instantiate(_buttonPrefab, _buttonParent);
            button.transform.localScale = Vector3.one;

            Action action = null;
            if (buttons[i].Close)
            {
                action = Close;
                action += DeactivateAllButtons;
                CanBeClosed = true;
            }

            button.Init(buttons[i], action);
            _buttons.Add(button);
        }

        return this;
    }

    public UIPopupMessage SetButtonsYesNo(Action yesAction, Action noAction = null)
        => SetButtons(new Button("Yes", yesAction), new Button("No", noAction));

    public UIPopupMessage SetButtonClose(Action closeAction = null)
        => SetButtons(new Button("Close", closeAction));

    private void DeactivateAllButtons()
    {
        if (!_buttons.IsNullOrEmpty())
            _buttons.ForEach(b => b.Button.interactable = false);
    }

    public UIPopupMessage SetAutoClose(float autoClose)
    {
        if (_autoClose != null) StopCoroutine(_autoClose);
        _autoClose = StartCoroutine(AutoClose(autoClose));
        return this;
    }

    private IEnumerator AutoClose(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Close();
    }

    private void Close() => UIManager.Hide(_tag);
}
