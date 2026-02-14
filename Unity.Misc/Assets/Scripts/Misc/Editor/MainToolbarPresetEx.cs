using System.Reflection;
using UnityEditor;
using UnityEngine;

// Suppress trimming warnings - this is Editor-only code, trimming does not apply
#pragma warning disable IL2026, IL2065, IL2067, IL2070, IL2072, IL2075, IL2077

[InitializeOnLoad]
public static class MainToolbarPresetEx
{
    const int VERSION = 1;
    const string k_PresetPath = "Assets/Settings/ToolbarPreset.asset";
    const string k_VersionKey = "MainToolbarPresetEx.AppliedVersion";
    
    static MainToolbarPresetEx()
    {
        // Apply if version changed or never applied
        int appliedVersion = EditorPrefs.GetInt(k_VersionKey, 0);
        if (appliedVersion < VERSION)
        {
            EditorApplication.delayCall += ApplyToolbarPreset;
        }
    }

    public static void Reset()
    {
        EditorPrefs.DeleteKey(k_VersionKey);
        ApplyToolbarDefault();
    }

    private static void MarkVersionApplied()
    {
        EditorPrefs.SetInt(k_VersionKey, VERSION);
    }

    public static bool ApplyToolbarDefault()
    {
        var mainToolbarWindowType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.MainToolbarWindow");
        if (mainToolbarWindowType == null)
        {
            Debug.LogError("[MainToolbarPresetEx] MainToolbarWindow type not found.");
            return false;
        }

        var windows = Resources.FindObjectsOfTypeAll(mainToolbarWindowType);
        if (windows == null || windows.Length == 0)
        {
            Debug.LogError("[MainToolbarPresetEx] No MainToolbarWindow instance found.");
            return false;
        }

        var window = windows[0] as EditorWindow;
        if (window == null)
        {
            Debug.LogError("[MainToolbarPresetEx] MainToolbarWindow instance is null.");
            return false;
        }

        var overlayCanvasProperty = typeof(EditorWindow).GetProperty("overlayCanvas", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (overlayCanvasProperty == null)
        {
            Debug.LogError("[MainToolbarPresetEx] overlayCanvas property not found on EditorWindow.");
            return false;
        }

        var overlayCanvas = overlayCanvasProperty.GetValue(window);
        if (overlayCanvas == null)
        {
            Debug.LogError("[MainToolbarPresetEx] overlayCanvas is null.");
            return false;
        }

        var canvasType = overlayCanvas.GetType();

        // OverlayPresetManager.instance.GetAllPresets(window.GetType()) 중에서 "Default" 선택
        object defaultPreset = GetDefaultPresetFromManager(window.GetType());
        if (defaultPreset == null)
        {
            Debug.LogError("[MainToolbarPresetEx] Default preset not found.");
            return false;
        }

        var applyPresetMethod = canvasType.GetMethod("ApplyPreset", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (applyPresetMethod == null)
        {
            Debug.LogError("[MainToolbarPresetEx] ApplyPreset method not found on OverlayCanvas.");
            return false;
        }

        applyPresetMethod.Invoke(overlayCanvas, new object[] { defaultPreset });

        return true;
    }

    static object GetDefaultPresetFromManager(System.Type windowType)
    {
        var editorAssembly = typeof(UnityEditor.Editor).Assembly;
        var managerType = editorAssembly.GetType("UnityEditor.Overlays.OverlayPresetManager");
        if (managerType == null)
        {
            Debug.LogError("[MainToolbarPresetEx] OverlayPresetManager type not found.");
            return null;
        }

        var getDefaultPresetMethod = managerType.GetMethod("GetDefaultPreset", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new[] { typeof(System.Type) }, null);
        if (getDefaultPresetMethod == null)
        {
            Debug.LogError("[MainToolbarPresetEx] GetDefaultPreset method not found on OverlayPresetManager.");
            return null;
        }

        return getDefaultPresetMethod.Invoke(null, new object[] { windowType });
    }

    /// <summary>
    /// Applies the toolbar preset to hide unwanted toolbar elements using reflection
    /// </summary>
    public static void ApplyToolbarPreset()
    {
        // Load preset as ScriptableObject (OverlayPreset is internal)
        var preset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(k_PresetPath);
        if (preset == null)
        {
            Debug.LogError($"[MainToolbarPresetEx] Toolbar preset not found at: {k_PresetPath}");
            return;
        }

        // Apply preset
        ApplyToolbarPreset(preset);

        // Mark current version as applied
        MarkVersionApplied();
    }

    /// <summary>
    /// Applies the toolbar preset to hide unwanted toolbar elements using reflection
    /// <param name="preset">The preset to apply</param>
    /// </summary>
    public static void ApplyToolbarPreset(ScriptableObject preset)
    {
        // Get MainToolbarWindow type
        var mainToolbarWindowType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.MainToolbarWindow");
        if (mainToolbarWindowType == null)
        {
            Debug.LogError("[MainToolbarPresetEx] MainToolbarWindow type not found.");
            return;
        }

        // Find all MainToolbarWindow instances
        var windows = Resources.FindObjectsOfTypeAll(mainToolbarWindowType);
        if (windows == null || windows.Length == 0)
        {
            Debug.LogError("[MainToolbarPresetEx] No MainToolbarWindow instance found.");
            return;
        }

        // Get overlayCanvas property from EditorWindow
        var overlayCanvasProperty = typeof(EditorWindow).GetProperty("overlayCanvas", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (overlayCanvasProperty == null)
        {
            Debug.LogError("[MainToolbarPresetEx] overlayCanvas property not found on EditorWindow.");
            return;
        }

        var mainToolbarWindow = windows[0] as EditorWindow;
        var overlayCanvas = overlayCanvasProperty.GetValue(mainToolbarWindow);
        if (overlayCanvas == null)
        {
            Debug.LogError("[MainToolbarPresetEx] overlayCanvas is null.");
            return;
        }

        // Call overlayCanvas.ApplyPreset(preset)
        var canvasType = overlayCanvas.GetType();
        var applyPresetMethod = canvasType.GetMethod("ApplyPreset", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (applyPresetMethod == null)
        {
            Debug.LogError("[MainToolbarPresetEx] ApplyPreset method not found on OverlayCanvas.");
            return;
        }

        applyPresetMethod.Invoke(overlayCanvas, new object[] { preset });
    }
}
