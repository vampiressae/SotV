using UnityEngine;
using DG.Tweening;

public class UIPopupScaledTextBase : MonoBehaviour
{
    [SerializeField] private Transform _victoryText;
    [SerializeField] private float _maxScale = 1.5f;
    [SerializeField] protected float _duration = 2;
    [SerializeField] private Ease _ease = Ease.OutQuart;

    protected virtual void Start() => Invoke(nameof(StartAnimation), 2);

    protected virtual void OnDestroy()
    {
        CancelInvoke(nameof(StartAnimation));
        _victoryText.DOKill();
    }

    protected virtual void StartAnimation() => _victoryText.DOScale(_maxScale, _duration).SetEase(_ease);
}
