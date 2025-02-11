using UnityEngine;
using UnityEngine.UI;

public abstract class UIButtonBase : MonoBehaviour
{
    private Button _button;

    private void OnEnable()
    {
        if (_button == null) _button = GetComponent<Button>();
        _button.onClick.AddListener(Click);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(Click);
    }

    protected abstract void Click();
}
