using UnityEngine;
using TMPro;

/// <summary>
/// Centralized font manager for Vietnamese text support
/// Provides a single Vietnamese-compatible font for all UI builders
/// </summary>
public class FontManager : MonoBehaviour
{
    private static FontManager instance;
    
    [Header("Vietnamese Font Asset")]
    [Tooltip("Assign a TMP Font Asset that supports Vietnamese characters")]
    [SerializeField] private TMP_FontAsset vietnameseFont;
    
    [Header("Fallback")]
    [Tooltip("Use default font if Vietnamese font is not assigned")]
    [SerializeField] private bool useDefaultFontAsFallback = true;
    
    void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            if (vietnameseFont != null)
            {
                Debug.Log($"✅ FontManager initialized with font: {vietnameseFont.name}");
            }
            else
            {
                Debug.LogWarning("⚠️ FontManager: No Vietnamese font assigned! Vietnamese characters may not display correctly.");
                Debug.LogWarning("💡 To fix: Assign a Vietnamese-compatible TMP Font Asset in the FontManager Inspector");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// Get the Vietnamese font asset
    /// </summary>
    public static TMP_FontAsset GetVietnameseFont()
    {
        if (instance == null)
        {
            Debug.LogWarning("⚠️ FontManager instance not found! Creating temporary one...");
            GameObject obj = new GameObject("FontManager");
            instance = obj.AddComponent<FontManager>();
        }
        
        return instance.vietnameseFont;
    }
    
    /// <summary>
    /// Apply Vietnamese font to a TextMeshProUGUI component
    /// </summary>
    public static void ApplyVietnameseFont(TextMeshProUGUI textComponent)
    {
        if (textComponent == null) return;
        
        TMP_FontAsset font = GetVietnameseFont();
        if (font != null)
        {
            textComponent.font = font;
        }
        else if (instance != null && !instance.useDefaultFontAsFallback)
        {
            Debug.LogWarning($"⚠️ No Vietnamese font available for: {textComponent.gameObject.name}");
        }
    }
    
    /// <summary>
    /// Create TextMeshProUGUI with Vietnamese font support
    /// </summary>
    public static TextMeshProUGUI CreateText(GameObject parent, string name = "Text")
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent.transform, false);
        
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        ApplyVietnameseFont(tmp);
        
        return tmp;
    }
}

