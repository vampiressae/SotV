using UnityEngine;
using DG.Tweening;
using Vamporium.UI;

public class UIPopupFightVictory : MonoBehaviour
{
    [SerializeField] private UITag _victoryUI;
    [SerializeField] private UITag _containerUI;

    [SerializeField] private Transform _victoryText;
    [SerializeField] private float _maxScale = 1.5f;
    [SerializeField] private float _duration = 2;
    [SerializeField] private Ease _ease;

    private void Start() => Invoke(nameof(StartAnimation), 2);
    private void OnDestroy() => CancelInvoke(nameof(StartAnimation));

    private void StartAnimation()
    {
        _victoryText.DOScale(_maxScale, _duration)
            .SetEase(_ease)
            .onComplete += Complete;
    }

    private void Complete()
    {
        UIManager.Hide(_victoryUI);
        UIManager.Show(_containerUI);
    }
}
