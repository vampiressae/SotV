using UnityEngine;

namespace VamporiumState
{
    public class StateMachineGUI : MonoBehaviour
    {
        [SerializeField] protected bool _showGUI;

        protected IStateMachine _machine;

        private void Awake() => _machine = GetComponent<IStateMachine>();

        protected virtual void OnGUI()
        {
            if (!_showGUI) return;

            GUILayout.BeginVertical("box");
            {
                if (_machine == null)
                {
                    GUI.color = Color.red;
                    GUILayout.Label("StateMachine not found for " + name);
                    GUI.color = Color.white;
                }
                else
                {
                    var time = _machine.TimeInState.ToString("f1");
                    GUILayout.Label($"State: {_machine.CurrentStateName} ({time})");
                }
            }
            GUILayout.EndVertical();
        }
    }
}
