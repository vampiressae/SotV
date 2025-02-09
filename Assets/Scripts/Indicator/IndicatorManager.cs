using UnityEngine;

public class IndicatorManager : MonoBehaviour
{
    [SerializeField] private IndicatorEventTrigger _eventTrigger;
    [SerializeField] private Indicator _prefab;
    [SerializeField] private Transform _parent;

    private void OnEnable() => _eventTrigger.OnTrigger += OnTrigger;
    private void OnDisable() => _eventTrigger.OnTrigger -= OnTrigger;

    private void OnTrigger()
    {
        var indicator = Instantiate(_prefab, _eventTrigger.Position, default, _parent);

        if (_eventTrigger.Transform)
            indicator.Init(_eventTrigger.Transform, _eventTrigger.Position, _eventTrigger.Sprite, _eventTrigger.Text);
        else
        {

        }
    }
}
