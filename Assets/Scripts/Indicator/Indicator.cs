using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Pathfinding.OffMeshLinks;

public class Indicator : MonoBehaviour
{
    [SerializeField] private CanvasGroup _fader;
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _text;
    [Space]
    [SerializeField] private float _duration;
    [SerializeField] private Vector2 _movementTarget = Vector2.up;
    [SerializeField] private AnimationCurve _movementCurve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] private AnimationCurve _fadeCurve = AnimationCurve.Linear(0, 0, 1, 1);

    private float _time, _start;
    private Vector3 _position;

    private RectTransform _rect, _canvasRT;
    private Transform _transform;
    private Canvas _canvas;

    private Vector3 Position => _transform ? _transform.position + _position : _position;

    private void Start()
    {
        _start = Time.time;
        _canvas = GetComponentInParent<Canvas>();
        _canvasRT = _canvas.transform as RectTransform;
        _rect = transform as RectTransform;

        UpdateTransform();
    }

    public void Init(Transform transform, Vector3 offset, Sprite sprite, string text)
    {
        _transform = transform;
        _position = offset;
        _image.sprite = sprite;
        _text.text = text;
    }

    private void Update()
    {
        UpdateTransform();
        if (_time < 1 - Mathf.Epsilon) return;

        enabled = false;
        Destroy(gameObject);
    }

    private void UpdateTransform()
    {
        _time = Mathf.InverseLerp(_start, _start + _duration, Time.time);

        var anchored = (_canvasRT.sizeDelta * _canvas.worldCamera.WorldToViewportPoint(Position)) - (_canvasRT.sizeDelta * _rect.anchorMin);
        _rect.anchoredPosition3D = Vector3.Lerp(anchored, anchored + _movementTarget, _movementCurve.Evaluate(_time));
        _fader.alpha = _fadeCurve.Evaluate(_time);
    }
}
