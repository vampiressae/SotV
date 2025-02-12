using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISettingsVisual : MonoBehaviour
{
    [SerializeField] private GameObject m_SettingsCanvas;
    [SerializeField] private GameObject m_LoadingCanvas;
    [SerializeField] private TMP_Dropdown m_FullScreenModeDropdown;
    [SerializeField] private TMP_Dropdown m_ResolutionDropdown;
    [SerializeField] private TMP_Dropdown m_DisplayDropdown;
    [SerializeField] private Toggle m_VSyncToggle;

    private List<TMP_Dropdown.OptionData> m_OptionDataCache = new List<TMP_Dropdown.OptionData>();
    private Resolution[] m_Resolutions;
    private Resolution[] m_EmptyResolution = new Resolution[1];
    private List<DisplayInfo> m_Displays = new List<DisplayInfo>();
    private bool m_MoveWindowInProgress = false;

    private Vector2Int m_LastWindowPosition;

    static readonly FullScreenMode[] m_FullScreenModes = new[]
    {
        FullScreenMode.Windowed,
        FullScreenMode.FullScreenWindow,
#if UNITY_STANDALONE_WIN
        FullScreenMode.ExclusiveFullScreen, // Exclusive full screen is only supported on Windows Standalone player
#endif
    };

    static readonly ResolutionComparer s_ResolutionComparer = new ResolutionComparer();
    static readonly Dictionary<(int, int), string> s_ResolutionNameCache = new Dictionary<(int, int), string>();
    static readonly Dictionary<DisplayInfo, string> s_DisplayNameCache = new Dictionary<DisplayInfo, string>();

    void Awake()
    {
        m_SettingsCanvas.SetActive(false);
        m_LoadingCanvas.SetActive(false);

        var fullScreenModeOptions = m_FullScreenModeDropdown.options;
        MoveOptionsToCache(fullScreenModeOptions);

        foreach (var mode in m_FullScreenModes)
            fullScreenModeOptions.Add(new TMP_Dropdown.OptionData(mode.ToString()));

        RefreshFullScreenModes();
        RefreshResolutions();
        RefreshDisplays();

        m_FullScreenModeDropdown.onValueChanged.AddListener(OnFullScreenModeChanged);
        m_ResolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        m_DisplayDropdown.onValueChanged.AddListener(OnDisplayChanged);
        m_VSyncToggle.onValueChanged.AddListener(OnVSyncChanged);

        // So that first update thinks that the position changed and updates the text
        m_LastWindowPosition = new Vector2Int(int.MinValue, int.MaxValue);
    }

    void Update()
    {
        if (m_MoveWindowInProgress) return;

        if (Input.GetKey(KeyCode.Escape))
        {
            var wasActive = m_SettingsCanvas.activeSelf;
            m_SettingsCanvas.SetActive(!wasActive);

            if (!wasActive) Cursor.lockState = CursorLockMode.None;
            else Cursor.lockState = CursorLockMode.Locked;
        }

        if (m_SettingsCanvas.activeSelf)
        {
            RefreshFullScreenModes();
            RefreshResolutions();
            RefreshDisplays();
        }
    }

    private void RefreshFullScreenModes()
    {
        var currentFullScreenMode = Screen.fullScreenMode;
        var currentFullScreenModeIndex = 0;

        for (int i = 0; i < m_FullScreenModes.Length; i++)
        {
            if (m_FullScreenModes[i] == currentFullScreenMode)
            {
                currentFullScreenModeIndex = i;
                break;
            }
        }

        m_FullScreenModeDropdown.SetValueWithoutNotify(currentFullScreenModeIndex);
    }

    private void RefreshResolutions()
    {
        var resolutions = Screen.resolutions;

        int uniqueResolutionCount;
        if (resolutions.Length > 0)
        {
            // Filter out non-unique resolutions
            Array.Sort(resolutions, s_ResolutionComparer);

            int lastUniqueResolution = 0;
            for (int i = 1; i < resolutions.Length; i++)
            {
                if (s_ResolutionComparer.Compare(resolutions[i], resolutions[lastUniqueResolution]) != 0)
                {
                    lastUniqueResolution++;
                    if (lastUniqueResolution != i)
                        resolutions[lastUniqueResolution] = resolutions[i];
                }
            }

            uniqueResolutionCount = lastUniqueResolution + 1;
        }
        else
        {
            uniqueResolutionCount = 1;
            m_EmptyResolution[0] = Screen.currentResolution;
            resolutions = m_EmptyResolution;
        }

        var currentWidth = Screen.width;
        var currentHeight = Screen.height;
        int currentResolutionIndex = uniqueResolutionCount - 1;

        var resolutionDropdownOptions = m_ResolutionDropdown.options;
        MoveOptionsToCache(resolutionDropdownOptions);

        for (int i = 0; i < uniqueResolutionCount; i++)
        {
            var option = GetNewOption();
            var resolution = resolutions[i];
            option.text = GetCachedResolutionText(resolution.width, resolution.height);
            resolutionDropdownOptions.Add(option);

            if (resolution.width == currentWidth && resolution.height == currentHeight)
                currentResolutionIndex = i;
        }

        m_Resolutions = resolutions;
        m_ResolutionDropdown.SetValueWithoutNotify(currentResolutionIndex);
    }

    private void RefreshDisplays()
    {
        Screen.GetDisplayLayout(m_Displays);

        var displayOptions = m_DisplayDropdown.options;
        MoveOptionsToCache(displayOptions);

        if (m_Displays.Count == 0)
        {
            var currentResolution = Screen.currentResolution;
            var fakeDisplay = new DisplayInfo() { name = "Fake display" };
            fakeDisplay.width = currentResolution.width;
            fakeDisplay.height = currentResolution.height;
            fakeDisplay.workArea = new RectInt(0, 0, currentResolution.width, currentResolution.height);
            fakeDisplay.refreshRate.denominator = 1;
            m_Displays.Add(fakeDisplay);
        }

        var currentWindowPosition = Screen.mainWindowPosition;
        var currentDisplay = Screen.mainWindowDisplayInfo;
        int currentDisplayIndex = 0;

        for (int i = 0; i < m_Displays.Count; i++)
        {
            var display = m_Displays[i];

            var option = GetNewOption();
            option.text = display.name;
            m_DisplayDropdown.options.Add(option);

            if (display.Equals(currentDisplay))
                currentDisplayIndex = i;
        }

        m_DisplayDropdown.SetValueWithoutNotify(currentDisplayIndex);
        if (m_LastWindowPosition != currentWindowPosition)
            m_LastWindowPosition = currentWindowPosition;
    }

    private void OnFullScreenModeChanged(int index)
    {
        var newMode = m_FullScreenModes[index];
        if (Screen.fullScreenMode != newMode)
        {
            Debug.Log("Setting FullScreenMode." + newMode);

            if (newMode == FullScreenMode.Windowed)
                Screen.fullScreenMode = newMode;
            else
            {
                var display = Screen.mainWindowDisplayInfo;
                Screen.SetResolution(display.width, display.height, newMode);
            }
        }
    }

    private void OnResolutionChanged(int index)
    {
        var newResolution = m_Resolutions[index];

        if (Screen.width != newResolution.width || Screen.height != newResolution.height)
        {
            Debug.Log("Setting resolution: " + GetCachedResolutionText(newResolution.width, newResolution.height));
            Screen.SetResolution(newResolution.width, newResolution.height, Screen.fullScreenMode);
        }
    }

    private void OnDisplayChanged(int index) => StartCoroutine(MoveToDisplay(index));

    private void OnVSyncChanged(bool isChecked) => QualitySettings.vSyncCount = isChecked ? 1 : 0;

    private IEnumerator MoveToDisplay(int index)
    {
        m_MoveWindowInProgress = true;
        m_SettingsCanvas.SetActive(false);
        m_LoadingCanvas.SetActive(true);

        try
        {
            var display = m_Displays[index];

            Debug.Log($"Moving window to {display.name}");

            Vector2Int targetCoordinates = new Vector2Int(0, 0);
            if (Screen.fullScreenMode != FullScreenMode.Windowed)
            {
                // Target the center of the display. Doing it this way shows off
                // that MoveMainWindow snaps the window to the top left corner
                // of the display when running in fullscreen mode.
                targetCoordinates.x += display.width / 2;
                targetCoordinates.y += display.height / 2;
            }

            var moveOperation = Screen.MoveMainWindowTo(display, targetCoordinates);
            yield return moveOperation;
        }
        finally
        {
            m_LoadingCanvas.SetActive(false);
            m_SettingsCanvas.SetActive(true);
            m_MoveWindowInProgress = false;
        }
    }

    private string GetCachedResolutionText(int width, int height)
    {
        var key = (width, height);
        if (s_ResolutionNameCache.TryGetValue(key, out var name))
            return name;

        name = $"{width} x {height}";
        s_ResolutionNameCache.Add(key, name);
        return name;
    }

    private void MoveOptionsToCache(List<TMP_Dropdown.OptionData> options)
    {
        m_OptionDataCache.AddRange(options);
        options.Clear();
    }

    private TMP_Dropdown.OptionData GetNewOption()
    {
        if (m_OptionDataCache.Count == 0)
            return new TMP_Dropdown.OptionData();

        var last = m_OptionDataCache[m_OptionDataCache.Count - 1];
        m_OptionDataCache.RemoveAt(m_OptionDataCache.Count - 1);
        return last;
    }

    class ResolutionComparer : IComparer<Resolution>
    {
        public int Compare(Resolution x, Resolution y)
        {
            // Highest to lowest
            if (x.width == y.width)
                return y.height.CompareTo(x.height);

            return x.width > y.width ? -1 : 1;
        }
    }
}