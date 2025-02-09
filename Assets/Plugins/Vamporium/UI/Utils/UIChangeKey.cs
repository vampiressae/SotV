using UnityEngine;
using Vamporium.UI;

public class UIChangeKey : MonoBehaviour
{
    [SerializeField] protected UITag _tag;
    [Space]
    [SerializeField] private bool _onStart;
    [SerializeField] private bool _hidePopups;

    private void Start()
    {
        if (_onStart) Trigger();
    }

    public void Trigger()
    {
        if (_hidePopups) UIManager.HidePopups();
        if (_tag) UIManager.Show(_tag);
    }
}
