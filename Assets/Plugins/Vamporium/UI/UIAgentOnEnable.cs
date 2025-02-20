using UnityEngine;
using UnityEngine.Events;

namespace Vamporium.UI
{
    public class UIAgentOnEnable : UIAgent
    {
        [SerializeField] private UnityEvent _event;

        private void OnEnable() => _event.Invoke();
    }
}
