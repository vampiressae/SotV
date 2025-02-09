using UnityEngine;

[CreateAssetMenu(menuName = "Indicator/Event Trigger")]
public class IndicatorEventTrigger : ScriptableEvent
{
    public Transform Transform { get; set; }
    public Vector3 Position { get; set; }
    public Sprite Sprite { get; set; }
    public string Text { get; set; }

    public void Trigger(Vector3 position, Sprite sprite, string text)
    {
        Transform = null;
        Position = position;
        Sprite = sprite;
        Text = text;
        TriggerBase();
    }

    public void Trigger(Transform transform, Vector3 offset, Sprite sprite, string text)
    {
        Transform = transform;
        Position = offset;
        Sprite = sprite;
        Text = text;
        TriggerBase();
    }

    protected virtual void TriggerBase() => Trigger();
}
