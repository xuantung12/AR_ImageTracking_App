using UnityEngine;
using UnityEditor;

/// <summary>
/// Editor menu for Vietnamese Font Fixer
/// Adds menu items to easily fix Vietnamese font issues
/// </summary>
public class VietnameseFontFixerEditor : Editor
{
    [MenuItem("Tools/Vietnamese Font/Fix Current Scene")]
    public static void FixCurrentScene()
    {
        VietnameseFontFixer fixer = FindObjectOfType<VietnameseFontFixer>();
        
        if (fixer == null)
        {
            Debug.LogWarning("⚠️ No VietnameseFontFixer found in scene. Creating one...");
            GameObject fixerObj = new GameObject("VietnameseFontFixer");
            fixer = fixerObj.AddComponent<VietnameseFontFixer>();
            Debug.Log("✅ Created VietnameseFontFixer. Please assign a Vietnamese font asset in the Inspector.");
        }
        else
        {
            fixer.FixAllFontsInScene();
        }
    }

    [MenuItem("Tools/Vietnamese Font/Fix All Scenes")]
    public static void FixAllScenes()
    {
        VietnameseFontFixer fixer = FindObjectOfType<VietnameseFontFixer>();
        
        if (fixer == null)
        {
            Debug.LogWarning("⚠️ No VietnameseFontFixer found. Creating temporary one...");
            GameObject fixerObj = new GameObject("VietnameseFontFixer_Temp");
            fixer = fixerObj.AddComponent<VietnameseFontFixer>();
        }

        fixer.FixAllScenesInProject();

        // Clean up temporary fixer
        if (fixer.gameObject.name.Contains("_Temp"))
        {
            DestroyImmediate(fixer.gameObject);
        }
    }

    [MenuItem("Tools/Vietnamese Font/Create Font Asset Guide")]
    public static void ShowFontAssetGuide()
    {
        string guide = @"
📖 HOW TO CREATE VIETNAMESE FONT ASSET FOR TEXTMESHPRO

Step 1: Open Font Asset Creator
   • Window → TextMeshPro → Font Asset Creator

Step 2: Select Font
   • Source Font File: Choose a font that supports Vietnamese
   • Recommended fonts:
     - Arial (built-in Windows)
     - Roboto (download from Google Fonts)
     - Noto Sans (download from Google Fonts)

Step 3: Configure Settings
   • Sampling Point Size: 36-48 (higher = better quality)
   • Padding: 5
   • Packing Method: Optimum
   • Atlas Resolution: 2048 x 2048 (or higher for more characters)

Step 4: Add Vietnamese Characters
   • Character Set: Unicode Range (Hex)
   • Click 'Add Range'
   • Add these ranges:
     - 0x0000-0x007F (Basic Latin)
     - 0x0080-0x00FF (Latin-1 Supplement)
     - 0x0100-0x017F (Latin Extended-A)
     - 0x0180-0x024F (Latin Extended-B)
     - 0x1E00-0x1EFF (Latin Extended Additional - VIETNAMESE!)
   • Or simply use: 0x0000-0x1EFF (includes all above)

Step 5: Generate
   • Click 'Generate Font Atlas'
   • Wait for generation to complete

Step 6: Save
   • Click 'Save' or 'Save as...'
   • Save to: Assets/Fonts/VietnameseTMP_SDF.asset
   • Name it: VietnameseTMP_SDF

Step 7: Assign to Fixer
   • Find VietnameseFontFixer in your scene
   • Drag the font asset to 'Vietnamese Font' field

Step 8: Fix All Scenes
   • Tools → Vietnamese Font → Fix All Scenes

✅ DONE! All Vietnamese text should now display correctly!

💡 TIP: If some characters still don't show:
   • Increase Atlas Resolution to 4096 x 4096
   • Or add more specific Unicode ranges
";

        Debug.Log(guide);
        EditorUtility.DisplayDialog("Vietnamese Font Asset Guide", 
            "Guide printed to Console. Check the Console window for detailed instructions.", 
            "OK");
    }
}

