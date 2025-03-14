using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;
using DG.Tweening;

public class ShowUIOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private CanvasGroup _fader;
    [SerializeField, MinMaxSlider(0, 1)] private Vector2 _alpha = Vector2.up;
    [SerializeField] private float _duration = 0.2f;

    private void OnEnable() => _fader.alpha = _alpha.x;
    private void OnDisable() => _fader.DOKill();

    public void OnPointerEnter(PointerEventData eventData)
    {
        _fader.DOKill();
        _fader.DOFade(_alpha.y, _duration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _fader.DOKill();
        _fader.DOFade(_alpha.x, _duration);
    }
}
