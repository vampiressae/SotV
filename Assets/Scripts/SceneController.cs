using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vamporium.UI;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    public static event Action<string> OnSceneLoading, OnSceneUnloading;
    public static event Action<string> OnSceneLoaded, OnSceneUnloaded;

    [SerializeField] private UITag _loadingUI;
    [SerializeField] private float _minLoading;

    private string _desired;
    private Scene _current;
    private float _started;

    public Scene Current => _current;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start() => LoadScene("Main");

    public void LoadScene(string scene)
    {
        _desired = scene;
        _started = Time.time;
        StartCoroutine(LoadSceneAsync(false));
    }

    public void UnloadCurrentScene() => StartCoroutine(UnloadCurrentSceneAsync(false));
    private void RetryLoadScene(string scene) => StartCoroutine(LoadSceneAsync(true));

    public IEnumerator LoadSceneAsync(bool auto)
    {
        if (!auto)
        {
            UIManager.Show(_loadingUI);
            yield return new WaitForSeconds(UIManager.Instance.FadeDuration);
            UIManager.HidePopups();
        }

        if (_current.IsValid())
        {
            OnSceneUnloaded -= RetryLoadScene;
            OnSceneUnloaded += RetryLoadScene;
            StartCoroutine(UnloadCurrentSceneAsync(true));
            yield break;
        }

        OnSceneUnloaded -= RetryLoadScene;
        OnSceneLoading?.Invoke(_desired);

        SceneManager.sceneLoaded += SceneLoaded;

        var loadScene = SceneManager.LoadSceneAsync(_desired, LoadSceneMode.Additive);
        loadScene.allowSceneActivation = false;

        while (!loadScene.isDone)
        {
            if (loadScene.progress > 0.8999f) break;
            yield return null;
        }

        loadScene.allowSceneActivation = true;

        while (!loadScene.isDone)
            yield return null;

        var wait = UIManager.Instance.FadeDuration;
        wait += Mathf.Max(_minLoading - (Time.time - _started), 0f);

        yield return new WaitForSeconds(wait);

        UIManager.Hide(_loadingUI);
        OnSceneLoaded?.Invoke(_current.name);
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    private void SceneLoaded(Scene scene, LoadSceneMode _) => _current = scene;

    private IEnumerator UnloadCurrentSceneAsync(bool auto)
    {
        if (!auto)
        {
            UIManager.Show(_loadingUI);
            yield return new WaitForSeconds(UIManager.Instance.FadeDuration);
            UIManager.HidePopups();
        }

        var currentSceneName = _current.name;
        OnSceneUnloading?.Invoke(currentSceneName);

        var unloadScene = SceneManager.UnloadSceneAsync(_current);

        while (!unloadScene.isDone)
        {
            if (unloadScene.progress >= 0.9f) break;
            yield return null;
        }

        if (!auto) UIManager.Hide(_loadingUI);

        OnSceneUnloaded?.Invoke(currentSceneName);
    }
}
