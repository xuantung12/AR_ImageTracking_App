using UnityEngine;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// Utility to fix Vietnamese font display issues in TextMeshPro
/// Automatically finds and updates all TextMeshProUGUI components to use a Vietnamese-compatible font
/// </summary>
public class VietnameseFontFixer : MonoBehaviour
{
    [Header("Font Settings")]
    [Tooltip("Vietnamese-compatible TMP Font Asset (assign in Inspector)")]
    [SerializeField] private TMP_FontAsset vietnameseFont;

    [Header("Auto-Fix Settings")]
    [Tooltip("Automatically fix fonts on scene load")]
    [SerializeField] private bool autoFixOnStart = true;

    [Header("Debug")]
    [SerializeField] private bool showDebugLogs = true;

    void Start()
    {
        if (autoFixOnStart)
        {
            FixAllFontsInScene();
        }
    }

    /// <summary>
    /// Find all TextMeshProUGUI components in scene and update their fonts
    /// </summary>
    public void FixAllFontsInScene()
    {
        if (vietnameseFont == null)
        {
            Debug.LogWarning("⚠️ VietnameseFontFixer: No Vietnamese font assigned! Please assign a TMP Font Asset in the Inspector.");
            Debug.LogWarning("💡 To create a Vietnamese font asset:");
            Debug.LogWarning("   1. Window → TextMeshPro → Font Asset Creator");
            Debug.LogWarning("   2. Select a font that supports Vietnamese (Arial, Roboto, Noto Sans)");
            Debug.LogWarning("   3. Character Set: Unicode Range → Add range: 0x0000-0x1EF9 (includes Vietnamese)");
            Debug.LogWarning("   4. Click 'Generate Font Atlas'");
            Debug.LogWarning("   5. Save the font asset");
            Debug.LogWarning("   6. Assign it to this VietnameseFontFixer component");
            return;
        }

        // Find all TextMeshProUGUI components in the scene
        TextMeshProUGUI[] allTexts = FindObjectsOfType<TextMeshProUGUI>(true); // Include inactive objects

        int fixedCount = 0;
        List<string> fixedObjects = new List<string>();

        foreach (TextMeshProUGUI text in allTexts)
        {
            // Check if text is using a font that doesn't support Vietnamese
            if (text.font != vietnameseFont)
            {
                text.font = vietnameseFont;
                fixedCount++;
                fixedObjects.Add(text.gameObject.name);

                if (showDebugLogs)
                {
                    Debug.Log($"✅ Fixed font for: {text.gameObject.name} (was: {text.font?.name ?? "null"})");
                }
            }
        }

        if (showDebugLogs)
        {
            Debug.Log($"🎉 VietnameseFontFixer: Fixed {fixedCount} text components out of {allTexts.Length} total");
            if (fixedCount > 0)
            {
                Debug.Log($"📝 Fixed objects: {string.Join(", ", fixedObjects)}");
            }
        }
    }

    /// <summary>
    /// Fix fonts for all scenes in the project (Editor only)
    /// </summary>
    public void FixAllScenesInProject()
    {
        #if UNITY_EDITOR
        Debug.Log("🔄 Fixing fonts in all scenes...");

        // Get all scene paths
        string[] scenePaths = new string[]
        {
            "Assets/Scenes/MainMenuScene.unity",
            "Assets/Scenes/QuizScene.unity",
            "Assets/Scenes/StoryListScene.unity",
            "Assets/Scenes/StoryDetailScene.unity",
            "Assets/Scenes/HomeScene.unity",
            "Assets/Scenes/ARScene.unity"
        };

        foreach (string scenePath in scenePaths)
        {
            if (System.IO.File.Exists(scenePath))
            {
                Debug.Log($"📂 Processing scene: {scenePath}");
                UnityEditor.SceneManagement.EditorSceneManager.OpenScene(scenePath);
                FixAllFontsInScene();
                UnityEditor.SceneManagement.EditorSceneManager.SaveOpenScenes();
            }
        }

        Debug.Log("✅ All scenes processed!");
        #else
        Debug.LogWarning("⚠️ FixAllScenesInProject() can only be called in the Unity Editor");
        #endif
    }
}

