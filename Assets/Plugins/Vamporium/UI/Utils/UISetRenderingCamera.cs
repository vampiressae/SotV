using UnityEngine;
using Vamporium.UI;

public class UISetRenderingCamera : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;

    private void Start()
    {
        if (UIManager.Instance == null) return;

        var manager = UIManager.Instance.GetComponent<Canvas>();
        _canvas.renderMode = RenderMode.ScreenSpaceCamera;
        _canvas.worldCamera = manager.worldCamera;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_canvas == null) _canvas = GetComponent<Canvas>();
    }
#endif
}
