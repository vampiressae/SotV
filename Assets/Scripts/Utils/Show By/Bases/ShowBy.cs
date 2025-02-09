using UnityEngine;

public abstract class ShowBy : MonoBehaviour
{
    [SerializeField] private bool _inverted;
    [SerializeField] private GameObject[] _targets;

    protected abstract bool ShouldShow { get; }

    protected virtual void OnEnable() => Refresh();
    protected virtual void OnDisable() { }

    protected virtual void Refresh()
    {
        var show = _inverted ? !ShouldShow : ShouldShow;
        foreach (var target in _targets)
            target.SetActive(show);
    }
}
