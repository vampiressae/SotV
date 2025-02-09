using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities;

namespace Vamporium.UI
{
    public class UIManager : MonoBehaviour
    {
        public static event Action<UIBase> OnShowUI, OnHideUI;
        public static event Action<UIPopup> OnShowUIPopup;

        public static UIPopup LoadingPopup;
        public static int ShowingPopupsCount;

        public static UIManager Instance;
        public static UIBase Show(UITag tag, float duration = -1, float delay = 0) => Instance.ShowUI(tag, duration, delay);
        public static UIBase Hide(UITag tag, float duration = -1, float delay = 0) => Instance.HideUI(tag, duration, delay);

        public static void HidePopups() => Instance.HidePopupUIs();
        public static UIPopupMessage ShowMessage(float duration = -1) => Instance.ShowUIMessage(duration);

        public static UIBase ShowPreviousScreen(float duration = -1, float delay = 0)
        {
            Instance.RegisterHiddenAsPrevious = false;
            return Instance.ShowUI(Instance._previousScreenTags.Pop(), duration, delay);
        }

        public static bool ShowLoadingPopup(float duration = -1, float delay = 0)
        {
            if (LoadingPopup) return false;
            LoadingPopup = Instance.ShowUI(Instance._loadingPopup, duration, delay) as UIPopup;
            OnShowUIPopup?.Invoke(LoadingPopup);
            return true;
        }

        public static UIPopup HideLoadingPopup(float duration = -1, float delay = -1)
        {
            if (LoadingPopup == null) return null;
            if (delay < -Mathf.Epsilon) delay = 2;
            return Instance.HideUI(Instance._loadingPopup, duration, delay) as UIPopup;
        }

        [SerializeField] private float _startDelay = 1;
        [SerializeField] private UITag _startingScreen;
        [SerializeField] private UITag _loadingPopup;
        [SerializeField] private UITag _messagePopup;
        [Space]
        [SerializeField] private Transform _screenParent;
        [SerializeField] private Transform _popupParent;
        [Space]
        [SerializeField] private float _fadeDuration = 0.3f;
        [Space]
        [SerializeField] private UIBase[] _uis;

        protected UITag _currentTag;
        protected UIScreen _currentScreen;
        protected Dictionary<UITag, UIPopup> _popups;

        [Space][ShowInInspector, HideInEditorMode]
        protected Stack<UITag> _previousScreenTags;

        public UIScreen CurrentScreen => _currentScreen;
        public UITag PreviousScreenTag => _previousScreenTags.Peek();
        public float FadeDuration => _fadeDuration;
        public bool RegisterHiddenAsPrevious { get; set; }

        protected virtual void Awake()
        {
            Instance = this;
            RegisterHiddenAsPrevious = true;

            _popups = new();
            _previousScreenTags = new();

            foreach (Transform t in _screenParent) Destroy(t.gameObject);
            foreach(Transform t in _popupParent) Destroy(t.gameObject);
            if (_loadingPopup) ShowUI(_loadingPopup, 0);
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(_startDelay);

            if (_startingScreen) ShowUI(_startingScreen);
            HideUI(_loadingPopup);
        }

        protected virtual void OnDestroy()
        {
            ShowingPopupsCount = 0;
            if (Instance == this) Instance = null;
        }

        protected bool GetUI(UITag tag, out UIBase ui)
        {
            ui = null;
            if (!_uis.IsNullOrEmpty())
                for (int i = 0; i < _uis.Length; i++)
                {
                    if (_uis[i].Tag != tag) continue;
                    ui = _uis[i];
                    break;
                }
            var result = ui != null;
            if (!result) Debug.LogError($"Didn't find UI prefab with tag{tag}!");
            return result;
        }

        protected virtual UIBase ShowUI(UITag tag, float duration = -1, float delay = 0)
        {
            if (tag == null) return null;
            if (!GetUI(tag, out var prefab)) return null;

            Transform parent = null;
            UIScreen screen = prefab as UIScreen;
            UIPopup popup = prefab as UIPopup;

            if (duration < -Mathf.Epsilon) duration = _fadeDuration;

            if (screen)
            {
                parent = _screenParent;
                if (_currentScreen)
                {
                    if (_currentScreen.Tag == tag) return _currentScreen;
                    _currentScreen.HideByManager(duration, delay);

                    if (!RegisterHiddenAsPrevious) RegisterHiddenAsPrevious = true;
                    else _previousScreenTags.Push(_currentScreen.Tag);
                }
            }

            if (popup)
            {
                if (_popups.TryGetValue(tag, out var existing)) return existing;
                parent = _popupParent;
            }

            var ui = Instantiate(prefab, parent);
            ui.transform.localScale = Vector3.one;
            ui.CanvasGroup.alpha = 0;

            if (popup)
            {
                _popups.Add(tag, ui as UIPopup);
                ShowingPopupsCount++;
            }

            if (screen)
            {
                _currentScreen = ui as UIScreen;
                _currentTag = tag;
            }

            ui.ShowByManager(duration, delay);
            OnShowUI?.Invoke(ui);
            return ui;
        }

        protected virtual UIBase HideUI(UITag tag, float duration = -1, float delay = 0)
        {
            if (tag == null) return null;

            UIBase ui = null;

            if (_currentTag == tag)
            {
                ui = _currentScreen;
                _currentTag = null;
                _currentScreen = null;
            }


            if (_popups.ContainsKey(tag))
            {
                ui = _popups[tag];
                _popups.Remove(tag);
                ShowingPopupsCount--;
            }

            if (ui == null) return null;

            if (duration < -Mathf.Epsilon) duration = _fadeDuration;

            ui.HideByManager(duration, delay);
            OnHideUI?.Invoke(ui);
            return ui;
        }

        protected virtual UIPopupMessage ShowUIMessage(float duration)
        {
            if (_popups.ContainsKey(_messagePopup)) Hide(_messagePopup);
            return ShowUI(_messagePopup, duration) as UIPopupMessage;
        }

        private void HidePopupUIs()
        {
            var popups = _popups.Keys.ToArray();
            popups.ForEach(p => HideUI(p));
        }

        public void TogglePopupUI(UITag tag)
        {
            if (_popups.ContainsKey(tag)) Hide(tag);
            else Show(tag);
        }

        public UIBase HideCurrentScreen(bool destroy = false, float delay = 0)
        {
            if (_currentScreen == null) return null;
            _currentScreen.DontDestroyNextHide = !destroy;
            _currentScreen.HideByManager(_fadeDuration, delay);
            return _currentScreen;
        }

        public void UnhideCurrentScreen(float delay = 0)
        {
            if (_currentScreen == null) return;
            _currentScreen.ShowByManager(_fadeDuration, delay);
        }

        public bool ShowingPopup(UITag tag)
        {
            if (tag == null)
            {
                Debug.Log("Empty tag received");
                return false;
            }
            return _popups.ContainsKey(tag);
        }
    }
}