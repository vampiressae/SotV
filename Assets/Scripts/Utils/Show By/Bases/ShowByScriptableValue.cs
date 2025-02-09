using UnityEngine;

public abstract class ShowByScriptableValue<T> : ShowBy where T : ScriptableValue
{
    [SerializeField] protected T _value;

    protected override void OnEnable()
    {
        base.OnEnable();
        _value.OnChangedRaw += Refresh;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _value.OnChangedRaw -= Refresh;
    }
}
