using UnityEngine;
using UnityEditor;
using TMPro;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Editor utility to help create TextMeshPro Sprite Assets from the Arlan Trindade emoji icons
/// This provides instructions and opens the TMP Sprite Importer window
/// </summary>
public class CreateEmojiSpriteAsset : EditorWindow
{
    private string sourceFolderPath = "Assets/Arlan Trindade/Free emojis pixel art/emojis-x2-64x64";

    [MenuItem("Tools/Create Custom Emoji Sprite Asset")]
    static void ShowWindow()
    {
        GetWindow<CreateEmojiSpriteAsset>("Create Emoji Sprite Asset");
    }

    void OnGUI()
    {
        GUILayout.Label("Create TMP Sprite Asset from Emoji Icons", EditorStyles.boldLabel);
        GUILayout.Space(10);

        EditorGUILayout.HelpBox(
            "This tool helps you create a TextMeshPro Sprite Asset from the Arlan Trindade emoji icons.\n\n" +
            "After creating the sprite asset, you can use emojis in your text like this:\n" +
            "<sprite=\"CustomEmojis\" name=\"E1\">\n\n" +
            "The emoji files are named E1.png, E2.png, etc.",
            MessageType.Info
        );

        GUILayout.Space(10);

        sourceFolderPath = EditorGUILayout.TextField("Source Folder:", sourceFolderPath);

        GUILayout.Space(10);

        if (GUILayout.Button("Open Emoji Folder", GUILayout.Height(30)))
        {
            OpenEmojiFolder();
        }

        GUILayout.Space(5);

        if (GUILayout.Button("Open TMP Sprite Importer", GUILayout.Height(30)))
        {
            OpenTMPSpriteImporter();
        }

        GUILayout.Space(10);

        EditorGUILayout.HelpBox(
            "INSTRUCTIONS:\n\n" +
            "1. Click 'Open Emoji Folder' to see the emoji images\n" +
            "2. Select the emoji images you want to use (Ctrl+A for all)\n" +
            "3. Click 'Open TMP Sprite Importer' to open the import window\n" +
            "4. In the Sprite Importer, click 'Create Sprite Asset'\n" +
            "5. Save the asset to: Assets/TextMesh Pro/Resources/Sprite Assets/CustomEmojis.asset",
            MessageType.None
        );

        GUILayout.Space(10);

        EditorGUILayout.HelpBox(
            "Quick Reference - Common Emoji Names:\n" +
            "E1 = Smile, E2 = Laugh, E3 = Heart Eyes, E4 = Wink\n" +
            "E5 = Cool, E6 = Thinking, E7 = Surprised, E8 = Sad\n" +
            "Check the Preview.png in the emoji folder for all icons!",
            MessageType.None
        );
    }

    void OpenEmojiFolder()
    {
        if (!Directory.Exists(sourceFolderPath))
        {
            EditorUtility.DisplayDialog("Error", "Source folder not found: " + sourceFolderPath, "OK");
            return;
        }

        // Select the folder in the Project window
        Object folderObj = AssetDatabase.LoadAssetAtPath<Object>(sourceFolderPath);
        if (folderObj != null)
        {
            Selection.activeObject = folderObj;
            EditorGUIUtility.PingObject(folderObj);
            Debug.Log($"Opened emoji folder: {sourceFolderPath}");
        }
    }

    void OpenTMPSpriteImporter()
    {
        // Open the TextMeshPro Sprite Importer window
        EditorApplication.ExecuteMenuItem("Window/TextMeshPro/Sprite Importer");

        EditorUtility.DisplayDialog(
            "TMP Sprite Importer",
            "The TextMeshPro Sprite Importer window is now open.\n\n" +
            "STEPS:\n" +
            "1. Make sure you have emoji images selected in the Project window\n" +
            "2. In the Sprite Importer, click 'Create Sprite Asset'\n" +
            "3. Save as 'CustomEmojis' in: Assets/TextMesh Pro/Resources/Sprite Assets/\n\n" +
            "Then you can use: <sprite=\"CustomEmojis\" name=\"E1\">",
            "OK"
        );
    }
}

