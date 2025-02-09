using Sirenix.OdinInspector;
using UnityEngine;

namespace Console
{
    public class ConsoleUI : MonoBehaviour
    {
        [SerializeField] private ConsoleTrigger _trigger;
        [SerializeField] private ConsoleRowUI _prefab;
        [SerializeField] private Transform _parent;

        private void OnEnable()
        {
            _trigger.OnConsoleTriggered += OnConsoleShown;
            foreach (Transform t in _parent)
                Destroy(t.gameObject);
        }

        private void OnDisable() => _trigger.OnConsoleTriggered += OnConsoleShown;

        private void OnConsoleShown(ConsoleData data)
        {
            var ui = Instantiate(_prefab, _parent);
            data.SetUI(ui);
            ui.Init(data);
        }

#if UNITY_EDITOR
        [Button]
        private void SendConsole(string text) => _trigger.Trigger(text);
#endif
    }
}
