using UnityEngine;

namespace Console
{
    public class ConsoleController : MonoBehaviour
    {
        [SerializeField] private ConsoleTrigger _defaultTrigger;
        public static ConsoleController Instance { get; private set; }

        private void Awake() => Instance = this;

        public static void Show(string data) => Instance._defaultTrigger.Trigger(data);
        public static void Show(ConsoleData data) => Instance._defaultTrigger.Trigger(data);
    }
}
