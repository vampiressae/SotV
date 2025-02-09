using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public abstract class Tooltip : MonoBehaviour
{
    [SerializeField] private CanvasGroup _fader;

    protected virtual float OverridenFadeDuration => 0;

    private void OnDisable() => KillTweens();

    public virtual void Show(TooltipTrigger trigger = null)
    {
        _fader.alpha = 0;
        _fader.interactable = false;

        var duration = OverridenFadeDuration > Mathf.Epsilon ? OverridenFadeDuration : trigger ? trigger.FadeDuration : 0.1f;
        _fader.DOFade(1, duration).onComplete += ShowComplete;
    }

    public virtual void Hide(TooltipTrigger trigger = null)
    {
        DOTween.Kill(_fader);
        _fader.interactable = false;

        var duration = OverridenFadeDuration > Mathf.Epsilon ? OverridenFadeDuration : trigger ? trigger.FadeDuration : 0.1f;
        _fader.DOFade(0, duration).SetDelay(0.1f).onComplete += HideComplete;
    }

    protected void ShowComplete() => _fader.interactable = true;
    protected void HideComplete() => Destroy(gameObject);

    protected virtual void KillTweens()
    {
        DOTween.Kill(_fader);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_fader == null) _fader = GetComponent<CanvasGroup>();
    }
#endif
}

public abstract class Tooltip<T> : Tooltip
{
    public abstract void Init(T target);
}
