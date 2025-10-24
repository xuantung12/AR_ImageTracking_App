using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Builds the Home Screen UI programmatically
/// Matches the Flutter design with gradient background, icon, title, button, and instructions
/// </summary>



#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif




[ExecuteAlways]
public class HomeUIBuilder : MonoBehaviour
{
    private bool uiBuilt = false;
    private Canvas canvas;
    private GameObject backgroundPanel;
    private GameObject contentContainer;

    void Start()
    {
        Debug.Log("🏗️ HomeUIBuilder: Building Home UI...");
        BuildCompleteUI();
    }

    void BuildCompleteUI()
    {
        // Get canvas
        canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas not found!");
            return;
        }

        // Set canvas to Screen Space - Overlay
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // Get CanvasScaler and configure it
        CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
        if (scaler != null)
        {
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 1920); // Portrait resolution
            scaler.matchWidthOrHeight = 0.5f;
        }

        // Build UI hierarchy
        CreateBackground();
        CreateBackButton(); // NEW: Back button to MainMenu
        CreateContentContainer();
        CreateARIcon();
        CreateTitle();
        CreateSubtitle();
        CreateStartButton();
        CreateInstructionsPanel();

        Debug.Log("✅ Home UI built successfully!");
    }

    void CreateBackground()
    {
        backgroundPanel = new GameObject("Background");
        backgroundPanel.transform.SetParent(canvas.transform, false);

        RectTransform rt = backgroundPanel.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;
        rt.anchoredPosition = Vector2.zero;

        Image img = backgroundPanel.AddComponent<Image>();
        // Create gradient texture
        Texture2D gradientTexture = CreateGradientTexture();
        img.sprite = Sprite.Create(gradientTexture, new Rect(0, 0, gradientTexture.width, gradientTexture.height), new Vector2(0.5f, 0.5f));

        Debug.Log("✅ Background created");
    }

    void CreateBackButton()
    {
        GameObject backButton = new GameObject("BackButton");
        backButton.transform.SetParent(canvas.transform, false);

        RectTransform rt = backButton.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0, 1);
        rt.sizeDelta = new Vector2(120, 120);
        rt.anchoredPosition = new Vector2(40, -40);

        // Background circle
        Image img = backButton.AddComponent<Image>();
        img.color = new Color(1f, 1f, 1f, 0.2f); // Semi-transparent white
        img.sprite = CreateCircleSprite();

        Button btn = backButton.AddComponent<Button>();
        btn.targetGraphic = img;

        // Back arrow text
        GameObject arrowText = new GameObject("ArrowText");
        arrowText.transform.SetParent(backButton.transform, false);

        RectTransform arrowRt = arrowText.AddComponent<RectTransform>();
        arrowRt.anchorMin = Vector2.zero;
        arrowRt.anchorMax = Vector2.one;
        arrowRt.sizeDelta = Vector2.zero;

        TextMeshProUGUI tmp = arrowText.AddComponent<TextMeshProUGUI>();
        tmp.text = "←";
        tmp.fontSize = 48;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontStyle = FontStyles.Bold;

        // Wire up button click to return to MainMenu
        btn.onClick.AddListener(() =>
        {
            Debug.Log("Back button clicked - returning to MainMenu");
            SceneTransitionManager.LoadMainMenu();
        });

        Debug.Log("✅ Back button created");
    }

    Sprite CreateCircleSprite()
    {
        Texture2D texture = new Texture2D(128, 128);
        Color[] pixels = new Color[128 * 128];

        Vector2 center = new Vector2(64, 64);
        float radius = 64;

        for (int y = 0; y < 128; y++)
        {
            for (int x = 0; x < 128; x++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), center);
                pixels[y * 128 + x] = distance <= radius ? Color.white : Color.clear;
            }
        }

        texture.SetPixels(pixels);
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, 128, 128), new Vector2(0.5f, 0.5f));
    }

    Texture2D CreateGradientTexture()
    {
        int height = 256;
        Texture2D texture = new Texture2D(1, height);

        Color topColor = new Color(0.4f, 0.6f, 1f); // Blue
        Color bottomColor = new Color(0.6f, 0.2f, 0.8f); // Purple

        for (int y = 0; y < height; y++)
        {
            float t = y / (float)height;
            Color color = Color.Lerp(bottomColor, topColor, t);
            texture.SetPixel(0, y, color);
        }

        texture.Apply();
        return texture;
    }

    void CreateContentContainer()
    {
        contentContainer = new GameObject("ContentContainer");
        contentContainer.transform.SetParent(canvas.transform, false);

        RectTransform rt = contentContainer.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(900, 1600);
        rt.anchoredPosition = Vector2.zero;

        // Add VerticalLayoutGroup for automatic spacing
        VerticalLayoutGroup vlg = contentContainer.AddComponent<VerticalLayoutGroup>();
        vlg.childAlignment = TextAnchor.MiddleCenter;
        vlg.spacing = 40;
        vlg.childControlWidth = false;
        vlg.childControlHeight = false;
        vlg.childForceExpandWidth = false;
        vlg.childForceExpandHeight = false;

        Debug.Log("✅ Content container created");
    }

    void CreateARIcon()
    {
        GameObject iconContainer = new GameObject("ARIconContainer");
        iconContainer.transform.SetParent(contentContainer.transform, false);

        RectTransform rt = iconContainer.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(240, 240);

        Image img = iconContainer.AddComponent<Image>();
        img.color = Color.white;

        // Add rounded corners effect (simplified - Unity doesn't have built-in rounded corners)
        // In production, you'd use a rounded sprite or shader

        // Add AR icon text (placeholder - in production use an icon sprite)
        GameObject iconText = new GameObject("IconText");
        iconText.transform.SetParent(iconContainer.transform, false);

        RectTransform iconTextRt = iconText.AddComponent<RectTransform>();
        iconTextRt.anchorMin = Vector2.zero;
        iconTextRt.anchorMax = Vector2.one;
        iconTextRt.sizeDelta = Vector2.zero;

        TextMeshProUGUI tmp = iconText.AddComponent<TextMeshProUGUI>();
        tmp.text = "AR";
        tmp.fontSize = 80;
        tmp.color = new Color(0.4f, 0.6f, 1f); // Blue
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontStyle = FontStyles.Bold;

        Debug.Log("✅ AR Icon created");
    }

    void CreateTitle()
    {
        GameObject title = new GameObject("Title");
        title.transform.SetParent(contentContainer.transform, false);

        RectTransform rt = title.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(800, 100);

        TextMeshProUGUI tmp = title.AddComponent<TextMeshProUGUI>();
        tmp.text = "AR Card Scanner";
        tmp.fontSize = 72;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontStyle = FontStyles.Bold;

        Debug.Log("✅ Title created");
    }

    void CreateSubtitle()
    {
        GameObject subtitle = new GameObject("Subtitle");
        subtitle.transform.SetParent(contentContainer.transform, false);

        RectTransform rt = subtitle.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(800, 120);

        TextMeshProUGUI tmp = subtitle.AddComponent<TextMeshProUGUI>();
        tmp.text = "Quét thẻ bài để xem video AR trong không gian thực";
        tmp.fontSize = 32;
        tmp.color = new Color(1f, 1f, 1f, 0.7f); // White with 70% opacity
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.enableWordWrapping = true;

        Debug.Log("✅ Subtitle created");
    }

    void CreateStartButton()
    {
        GameObject buttonObj = new GameObject("StartButton");
        buttonObj.transform.SetParent(contentContainer.transform, false);

        RectTransform rt = buttonObj.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(500, 120);

        Image img = buttonObj.AddComponent<Image>();
        img.color = Color.white;

        Button btn = buttonObj.AddComponent<Button>();

        // Add button text
        GameObject buttonText = new GameObject("ButtonText");
        buttonText.transform.SetParent(buttonObj.transform, false);

        RectTransform textRt = buttonText.AddComponent<RectTransform>();
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.sizeDelta = Vector2.zero;

        TextMeshProUGUI tmp = buttonText.AddComponent<TextMeshProUGUI>();
        tmp.text = "📷 Bắt đầu quét";
        tmp.fontSize = 40;
        tmp.color = new Color(0.4f, 0.6f, 1f); // Blue
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontStyle = FontStyles.Bold;

        // Wire up button click
        btn.onClick.AddListener(() =>
        {
            Debug.Log("Start button clicked!");
            SceneTransitionManager.LoadAR();
        });

        Debug.Log("✅ Start button created");
    }

    void CreateInstructionsPanel()
    {
        GameObject panel = new GameObject("InstructionsPanel");
        panel.transform.SetParent(contentContainer.transform, false);

        RectTransform rt = panel.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(800, 300);

        Image img = panel.AddComponent<Image>();
        img.color = new Color(1f, 1f, 1f, 0.15f); // White with 15% opacity

        // Add instructions text
        GameObject instructionsText = new GameObject("InstructionsText");
        instructionsText.transform.SetParent(panel.transform, false);

        RectTransform textRt = instructionsText.AddComponent<RectTransform>();
        textRt.anchorMin = new Vector2(0.1f, 0.1f);
        textRt.anchorMax = new Vector2(0.9f, 0.9f);
        textRt.sizeDelta = Vector2.zero;

        TextMeshProUGUI tmp = instructionsText.AddComponent<TextMeshProUGUI>();
        tmp.text = "ℹ️ Hướng dẫn sử dụng:\n\n" +
                   "1. Hướng camera vào thẻ bài\n" +
                   "2. Giữ điện thoại ổn định\n" +
                   "3. Video sẽ hiển thị tự động\n" +
                   "4. Xoay điện thoại để xem hiệu ứng AR";
        tmp.fontSize = 28;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.TopLeft;
        tmp.enableWordWrapping = true;

        Debug.Log("✅ Instructions panel created");
    }
}