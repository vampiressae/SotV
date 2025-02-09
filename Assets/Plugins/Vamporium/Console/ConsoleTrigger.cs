using System;
using UnityEngine;

namespace Console
{
    [CreateAssetMenu(menuName = "Console Trigger")]
    public class ConsoleTrigger : ScriptableObject
    {
        public event Action<ConsoleData> OnConsoleTriggered;

        public void Trigger(string text) => Trigger(new ConsoleData(text));
        public void Trigger(ConsoleData data) => OnConsoleTriggered?.Invoke(data);
    }
}
