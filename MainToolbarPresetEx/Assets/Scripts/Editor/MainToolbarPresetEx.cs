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

    /// <summary>
    /// Applies the toolbar preset to hide unwanted toolbar elements using reflection
    /// </summary>
    static void ApplyToolbarPreset()
    {
        // Load preset as ScriptableObject (OverlayPreset is internal)
        var preset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(k_PresetPath);
        if (preset == null)
        {
            Debug.LogWarning($"[MainToolbarPresetEx] Toolbar preset not found at: {k_PresetPath}");
            return;
        }

        // Get MainToolbarWindow type
        var mainToolbarWindowType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.MainToolbarWindow");
        if (mainToolbarWindowType == null)
        {
            Debug.LogWarning("[MainToolbarPresetEx] MainToolbarWindow type not found.");
            return;
        }

        // Find all MainToolbarWindow instances
        var windows = Resources.FindObjectsOfTypeAll(mainToolbarWindowType);
        if (windows == null || windows.Length == 0)
        {
            Debug.LogWarning("[MainToolbarPresetEx] No MainToolbarWindow instance found.");
            return;
        }

        // Get overlayCanvas property from EditorWindow
        var overlayCanvasProperty = typeof(EditorWindow).GetProperty("overlayCanvas", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (overlayCanvasProperty == null)
        {
            Debug.LogWarning("[MainToolbarPresetEx] overlayCanvas property not found on EditorWindow.");
            return;
        }

        var mainToolbarWindow = windows[0] as EditorWindow;
        var overlayCanvas = overlayCanvasProperty.GetValue(mainToolbarWindow);
        if (overlayCanvas == null)
        {
            Debug.LogWarning("[MainToolbarPresetEx] overlayCanvas is null.");
            return;
        }
        
        // Call overlayCanvas.ApplyPreset(preset)
        var canvasType = overlayCanvas.GetType();
        var applyPresetMethod = canvasType.GetMethod("ApplyPreset", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (applyPresetMethod == null)
        {
            Debug.LogWarning("[MainToolbarPresetEx] ApplyPreset method not found on OverlayCanvas.");
            return;
        }

        applyPresetMethod.Invoke(overlayCanvas, new object[] { preset });
        // Mark current version as applied
        EditorPrefs.SetInt(k_VersionKey, VERSION);
    }
}
