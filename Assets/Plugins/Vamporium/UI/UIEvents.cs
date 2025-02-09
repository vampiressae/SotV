using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Vamporium.UI
{
    public class UIEvents : MonoBehaviour
    {
        [SerializeField] private UIBase _ui;
        [SerializeField, FoldoutGroup("Show")] private UnityEvent _beforeShow, _afterShow;
        [SerializeField, FoldoutGroup("Hide")] private UnityEvent _beforeHide, _afterHide;

        private void Awake()
        {
            _ui.OnBeforeShow += OnBeforeShow;
            _ui.OnAfterShow += OnAfterShow;
            _ui.OnBeforeHide += OnBeforeHide;
            _ui.OnAfterHide += OnAfterHide;
        }

        private void OnDestroy()
        {
            _ui.OnBeforeShow -= OnBeforeShow;
            _ui.OnAfterShow -= OnAfterShow;
            _ui.OnBeforeHide -= OnBeforeHide;
            _ui.OnAfterHide -= OnAfterHide;
        }

        private void OnAfterHide(UIBase ui) => _afterHide.Invoke();
        private void OnBeforeHide(UIBase ui) => _beforeHide.Invoke();
        private void OnAfterShow(UIBase ui) => _afterShow.Invoke();
        private void OnBeforeShow(UIBase ui) => _beforeShow.Invoke();

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_ui == null) _ui = GetComponent<UIBase>();
        }
#endif
    }
}
