using System;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace Vamporium.UI
{
    public abstract class UIBase : MonoBehaviour
    {
        public event Action<UIBase> OnBeforeShow, OnAfterShow;
        public event Action<UIBase> OnBeforeHide, OnAfterHide;

        [HideInInspector] public Action AfterShowAction;

        [SerializeField] protected UITag _tag;
        [SerializeField] protected CanvasGroup _canvasGroup;

        public UITag Tag => _tag;
        public CanvasGroup CanvasGroup => _canvasGroup;
        public bool DontDestroyNextHide { get; set; }

        private void OnDestroy() => DOTween.Kill(_canvasGroup);

        [Button, HorizontalGroup] public void Show() => UIManager.Show(Tag);
        [Button, HorizontalGroup] public void Hide() => UIManager.Hide(Tag);

        public virtual void ShowByManager(float duration, float delay)
        {
            gameObject.SetActive(true);

            DOTween.Kill(_canvasGroup);
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.DOFade(1, duration).SetDelay(delay).onComplete += ShowDone;

            OnBeforeShow?.Invoke(this);
        }

        protected virtual void ShowDone()
        {
            _canvasGroup.interactable = true;
            OnAfterShow?.Invoke(this);
            AfterShowAction?.Invoke();
        }

        public virtual void HideByManager(float duration, float delay)
        {
            DOTween.Kill(_canvasGroup);
            _canvasGroup.interactable = false;
            _canvasGroup.DOFade(0, duration).SetDelay(delay).onComplete += HideDone;
            OnBeforeHide?.Invoke(this);
        }

        protected virtual void HideDone()
        {
            OnAfterHide?.Invoke(this);

            if (DontDestroyNextHide) DontDestroyNextHide = false;
            else Destroy(gameObject);
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (_canvasGroup == null) _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null) _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
#endif
    }
}