using UnityEngine;

public class MoveUIWithMouse : MonoBehaviour
{
    public bool IsShown { get; private set; }

    [SerializeField] private bool _dynamicSize = true;
    [SerializeField] private bool _keepStartOffset = true;
    [SerializeField] private bool _constrained = true;
    [SerializeField] private float _padding;
    [Space]
    [SerializeField] private Vector2 _offset;
    [SerializeField] private Canvas _canvas;

    private bool _inited;
    private Vector2 _pointInRect;
    private RectTransform _rt, _canvasRT;

    private float _screenWidth, _screenHeight;
    private float _rtPivotX, _rtPivotY;
    [SerializeField] private Vector2 _grabOffset;

    protected virtual Vector2 MousePosition => Input.mousePosition;

    private void Awake() => transform.position = new(-99999, 0);

    private void Update() => MoveToMouse();
    private void OnEnable() => SetGrabOffset();

    private void Init()
    {
        if (_inited) return;

        _rt = (RectTransform)transform;
        if (_canvas == null) _canvas = GetComponentInParent<Canvas>();
        if (_canvas) _canvasRT = _canvas.transform as RectTransform;
        CalculateSizes();
        _inited = true;
    }

    private void CalculateSizes()
    {
        if (_canvas == null) return;

        _screenWidth = Screen.width - _padding - _rt.rect.width * _canvas.scaleFactor;
        _screenHeight = _rt.rect.height * _canvas.scaleFactor + _padding;
        _rtPivotX = _rt.pivot.x * _rt.rect.width;
        _rtPivotY = (1 - _rt.pivot.y) * _rt.rect.height;
    }

    public void MoveToMouse()
    {
        Init();
        if (_canvas == null) return;

        var offset = _offset;
        var position = MousePosition + offset - _grabOffset;

        if (_dynamicSize) CalculateSizes();
        if (_constrained)
        {
            if (position.x < _padding + _rtPivotX)
                position.x = _padding + _rtPivotX;
            else if (position.x > _screenWidth + _rtPivotX)
                position.x = _screenWidth + _rtPivotX;

            if (position.y < _screenHeight - _rtPivotY)
                position.y = _screenHeight - _rtPivotY;
            else if (position.y > Screen.height - _padding - _rtPivotY)
                position.y = Screen.height - _padding - _rtPivotY;
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRT, position, _canvas.worldCamera, out _pointInRect);
        transform.position = _canvasRT.TransformPoint(_pointInRect);
    }

    public void SetGrabOffset()
    {
        if (_rt == null || _canvas == null || !_keepStartOffset) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rt, MousePosition, _canvas.worldCamera, out _pointInRect);
        _grabOffset = _pointInRect;
    }
}
