using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Event")]
public class ScriptableEvent : ScriptableObject
{
    public event Action OnTrigger;
    public void Trigger() => OnTrigger?.Invoke();
}
