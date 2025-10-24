using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Builds the Story Detail UI for ar_kid app
/// Replicates the Flutter StoryDetailPage design with scrollable content
/// </summary>
public class StoryDetailUIBuilder : MonoBehaviour
{
    private Canvas canvas;
    private Story currentStory;
    private float fontSize = 28f;
    private TextMeshProUGUI contentText;
    
    void Start()
    {
        Debug.Log("📖 StoryDetailUIBuilder Started - Building Story Detail...");
        
        // Get selected story from StoryManager
        currentStory = StoryManager.Instance.SelectedStory;
        
        if (currentStory == null)
        {
            Debug.LogError("❌ No story selected! Returning to story list...");
            SceneTransitionManager.LoadStoryList();
            return;
        }
        
        Debug.Log($"📖 Displaying story: {currentStory.title}");
        BuildUI();
    }

    void BuildUI()
    {
        // Find or create Canvas
        canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasObj.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1080, 1920);
            canvasObj.AddComponent<GraphicRaycaster>();
            
            // Add EventSystem if not exists
            if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            }
        }

        // Create gradient background (Orange → White)
        CreateGradientBackground();
        
        // Create header
        CreateHeader();
        
        // Create story info panel
        CreateStoryInfoPanel();
        
        // Create font size control
        CreateFontSizeControl();
        
        // Create story content (scrollable)
        CreateStoryContent();
        
        Debug.Log("✅ Story Detail UI built successfully!");
    }

    void CreateGradientBackground()
    {
        GameObject bgObj = new GameObject("Background");
        bgObj.transform.SetParent(canvas.transform, false);
        
        RectTransform rt = bgObj.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;
        rt.anchoredPosition = Vector2.zero;
        
        // Create gradient texture (Orange → White)
        Image img = bgObj.AddComponent<Image>();
        Texture2D gradientTexture = new Texture2D(1, 256);
        
        for (int i = 0; i < 256; i++)
        {
            float t = i / 255f;
            Color color = Color.Lerp(
                new Color(1f, 0.98f, 0.94f), // Light orange (#FFF9F0)
                Color.white,
                t
            );
            gradientTexture.SetPixel(0, i, color);
        }
        
        gradientTexture.Apply();
        img.sprite = Sprite.Create(gradientTexture, new Rect(0, 0, 1, 256), new Vector2(0.5f, 0.5f));
        
        bgObj.transform.SetAsFirstSibling();
    }

    void CreateHeader()
    {
        GameObject headerObj = new GameObject("Header");
        headerObj.transform.SetParent(canvas.transform, false);
        
        RectTransform rt = headerObj.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(1, 1);
        rt.pivot = new Vector2(0.5f, 1);
        rt.sizeDelta = new Vector2(0, 180);
        rt.anchoredPosition = new Vector2(0, 0);
        
        // Orange background
        Image bgImg = headerObj.AddComponent<Image>();
        bgImg.color = new Color(1f, 0.6f, 0.2f); // Orange
        
        // Shadow
        Shadow shadow = headerObj.AddComponent<Shadow>();
        shadow.effectColor = new Color(0, 0, 0, 0.1f);
        shadow.effectDistance = new Vector2(0, 2);
        
        // Content container
        GameObject contentObj = new GameObject("Content");
        contentObj.transform.SetParent(headerObj.transform, false);
        
        RectTransform contentRt = contentObj.AddComponent<RectTransform>();
        contentRt.anchorMin = Vector2.zero;
        contentRt.anchorMax = Vector2.one;
        contentRt.sizeDelta = Vector2.zero;
        
        HorizontalLayoutGroup hlg = contentObj.AddComponent<HorizontalLayoutGroup>();
        hlg.childAlignment = TextAnchor.MiddleLeft;
        hlg.padding = new RectOffset(30, 30, 30, 30);
        hlg.spacing = 20;
        hlg.childControlWidth = false;
        hlg.childControlHeight = false;
        
        // Back button
        GameObject backBtn = CreateIconButton("BackButton", "←", 80, 80);
        backBtn.transform.SetParent(contentObj.transform, false);
        backBtn.GetComponent<Button>().onClick.AddListener(() => {
            Debug.Log("Back button clicked - returning to StoryList");
            SceneTransitionManager.LoadStoryList();
        });
        
        // Title and author
        CreateHeaderText(contentObj.transform);
    }

    void CreateHeaderText(Transform parent)
    {
        GameObject textContainer = new GameObject("TextContainer");
        textContainer.transform.SetParent(parent, false);
        
        RectTransform rt = textContainer.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(850, 120);
        
        VerticalLayoutGroup vlg = textContainer.AddComponent<VerticalLayoutGroup>();
        vlg.childAlignment = TextAnchor.MiddleLeft;
        vlg.spacing = 5;
        vlg.childControlWidth = true;
        vlg.childControlHeight = false;
        
        // Title
        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(textContainer.transform, false);
        
        TextMeshProUGUI titleText = titleObj.AddComponent<TextMeshProUGUI>();
        titleText.text = currentStory.title;
        titleText.fontSize = 34;
        titleText.color = Color.white;
        titleText.fontStyle = FontStyles.Bold;
        titleText.alignment = TextAlignmentOptions.Left;
        
        // Author
        GameObject authorObj = new GameObject("Author");
        authorObj.transform.SetParent(textContainer.transform, false);
        
        TextMeshProUGUI authorText = authorObj.AddComponent<TextMeshProUGUI>();
        authorText.text = currentStory.author;
        authorText.fontSize = 22;
        authorText.color = new Color(1, 1, 1, 0.7f); // White 70% opacity
        authorText.alignment = TextAlignmentOptions.Left;
    }

    void CreateStoryInfoPanel()
    {
        GameObject panelObj = new GameObject("InfoPanel");
        panelObj.transform.SetParent(canvas.transform, false);
        
        RectTransform rt = panelObj.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(1, 1);
        rt.pivot = new Vector2(0.5f, 1);
        rt.sizeDelta = new Vector2(-80, 140);
        rt.anchoredPosition = new Vector2(0, -200);
        
        // White background
        Image bgImg = panelObj.AddComponent<Image>();
        bgImg.color = Color.white;
        bgImg.sprite = CreateRoundedRectSprite();
        
        // Shadow
        Shadow shadow = panelObj.AddComponent<Shadow>();
        shadow.effectColor = new Color(0, 0, 0, 0.05f);
        shadow.effectDistance = new Vector2(0, 5);
        
        // Horizontal layout for info items
        HorizontalLayoutGroup hlg = panelObj.AddComponent<HorizontalLayoutGroup>();
        hlg.childAlignment = TextAnchor.MiddleCenter;
        hlg.spacing = 0;
        hlg.padding = new RectOffset(40, 40, 30, 30);
        hlg.childControlWidth = true;
        hlg.childControlHeight = false;
        hlg.childForceExpandWidth = true;
        
        // Category
        CreateInfoItem(panelObj.transform, "📚", currentStory.category, new Color(1f, 0.6f, 0.2f));
        
        // Divider
        CreateDivider(panelObj.transform);
        
        // Age range
        CreateInfoItem(panelObj.transform, "👶", $"{currentStory.ageRange}+ tuổi", new Color(0.2f, 0.6f, 1f));
        
        // Divider
        CreateDivider(panelObj.transform);
        
        // Reading time
        int readingTime = EstimateReadingTime();
        CreateInfoItem(panelObj.transform, "⏱", $"{readingTime} phút", new Color(0.3f, 0.8f, 0.3f));
    }

    void CreateInfoItem(Transform parent, string icon, string text, Color color)
    {
        GameObject itemObj = new GameObject("InfoItem");
        itemObj.transform.SetParent(parent, false);
        
        RectTransform rt = itemObj.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(250, 80);
        
        HorizontalLayoutGroup hlg = itemObj.AddComponent<HorizontalLayoutGroup>();
        hlg.childAlignment = TextAnchor.MiddleCenter;
        hlg.spacing = 12;
        hlg.childControlWidth = false;
        hlg.childControlHeight = false;
        
        // Icon
        GameObject iconObj = new GameObject("Icon");
        iconObj.transform.SetParent(itemObj.transform, false);
        
        RectTransform iconRt = iconObj.AddComponent<RectTransform>();
        iconRt.sizeDelta = new Vector2(32, 32);
        
        TextMeshProUGUI iconText = iconObj.AddComponent<TextMeshProUGUI>();
        iconText.text = icon;
        iconText.fontSize = 28;
        iconText.alignment = TextAlignmentOptions.Center;
        
        // Text
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(itemObj.transform, false);
        
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 22;
        tmp.color = Color.black;
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Left;
    }

    void CreateDivider(Transform parent)
    {
        GameObject dividerObj = new GameObject("Divider");
        dividerObj.transform.SetParent(parent, false);
        
        RectTransform rt = dividerObj.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(2, 60);
        
        Image img = dividerObj.AddComponent<Image>();
        img.color = new Color(0.85f, 0.85f, 0.85f); // Light gray
    }

    void CreateFontSizeControl()
    {
        GameObject controlObj = new GameObject("FontSizeControl");
        controlObj.transform.SetParent(canvas.transform, false);
        
        RectTransform rt = controlObj.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(1, 1);
        rt.anchorMax = new Vector2(1, 1);
        rt.pivot = new Vector2(1, 1);
        rt.sizeDelta = new Vector2(350, 80);
        rt.anchoredPosition = new Vector2(-40, -360);
        
        HorizontalLayoutGroup hlg = controlObj.AddComponent<HorizontalLayoutGroup>();
        hlg.childAlignment = TextAnchor.MiddleRight;
        hlg.spacing = 15;
        hlg.childControlWidth = false;
        hlg.childControlHeight = false;
        
        // Label
        GameObject labelObj = new GameObject("Label");
        labelObj.transform.SetParent(controlObj.transform, false);
        
        TextMeshProUGUI labelText = labelObj.AddComponent<TextMeshProUGUI>();
        labelText.text = "Cỡ chữ: ";
        labelText.fontSize = 22;
        labelText.color = new Color(0.5f, 0.5f, 0.5f);
        labelText.alignment = TextAlignmentOptions.Center;
        
        // Decrease button
        GameObject decreaseBtn = CreateFontButton("-", () => {
            if (fontSize > 20)
            {
                fontSize -= 2;
                UpdateContentFontSize();
            }
        });
        decreaseBtn.transform.SetParent(controlObj.transform, false);
        
        // Font size display
        GameObject sizeObj = new GameObject("FontSize");
        sizeObj.transform.SetParent(controlObj.transform, false);
        
        RectTransform sizeRt = sizeObj.AddComponent<RectTransform>();
        sizeRt.sizeDelta = new Vector2(60, 60);
        
        TextMeshProUGUI sizeText = sizeObj.AddComponent<TextMeshProUGUI>();
        sizeText.text = fontSize.ToString("0");
        sizeText.fontSize = 24;
        sizeText.color = Color.black;
        sizeText.fontStyle = FontStyles.Bold;
        sizeText.alignment = TextAlignmentOptions.Center;
        sizeText.name = "FontSizeText";
        
        // Increase button
        GameObject increaseBtn = CreateFontButton("+", () => {
            if (fontSize < 40)
            {
                fontSize += 2;
                UpdateContentFontSize();
            }
        });
        increaseBtn.transform.SetParent(controlObj.transform, false);
    }

    GameObject CreateFontButton(string text, UnityEngine.Events.UnityAction onClick)
    {
        GameObject btnObj = new GameObject($"FontButton_{text}");
        
        RectTransform rt = btnObj.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(60, 60);
        
        Image img = btnObj.AddComponent<Image>();
        img.color = new Color(1f, 0.6f, 0.2f); // Orange
        img.sprite = CreateCircleSprite();
        
        Button btn = btnObj.AddComponent<Button>();
        btn.targetGraphic = img;
        btn.onClick.AddListener(onClick);
        
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(btnObj.transform, false);
        
        RectTransform textRt = textObj.AddComponent<RectTransform>();
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.sizeDelta = Vector2.zero;
        
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 32;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontStyle = FontStyles.Bold;
        
        return btnObj;
    }

    void CreateStoryContent()
    {
        GameObject contentContainer = new GameObject("ContentContainer");
        contentContainer.transform.SetParent(canvas.transform, false);
        
        RectTransform rt = contentContainer.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(1, 1);
        rt.pivot = new Vector2(0.5f, 1);
        rt.offsetMin = new Vector2(40, 40);
        rt.offsetMax = new Vector2(-40, -460);
        
        // White background
        Image bgImg = contentContainer.AddComponent<Image>();
        bgImg.color = Color.white;
        bgImg.sprite = CreateRoundedRectSprite();
        
        // Shadow
        Shadow shadow = contentContainer.AddComponent<Shadow>();
        shadow.effectColor = new Color(0, 0, 0, 0.05f);
        shadow.effectDistance = new Vector2(0, 5);
        
        // Scroll view
        GameObject scrollView = new GameObject("ScrollView");
        scrollView.transform.SetParent(contentContainer.transform, false);
        
        RectTransform scrollRt = scrollView.AddComponent<RectTransform>();
        scrollRt.anchorMin = Vector2.zero;
        scrollRt.anchorMax = Vector2.one;
        scrollRt.sizeDelta = new Vector2(-40, -40);
        scrollRt.anchoredPosition = Vector2.zero;
        
        ScrollRect scrollRect = scrollView.AddComponent<ScrollRect>();
        scrollRect.horizontal = false;
        scrollRect.vertical = true;
        scrollRect.movementType = ScrollRect.MovementType.Elastic;
        
        // Content
        GameObject content = new GameObject("Content");
        content.transform.SetParent(scrollView.transform, false);
        
        RectTransform contentRt = content.AddComponent<RectTransform>();
        contentRt.anchorMin = new Vector2(0, 1);
        contentRt.anchorMax = new Vector2(1, 1);
        contentRt.pivot = new Vector2(0.5f, 1);
        contentRt.sizeDelta = new Vector2(-40, 0);
        
        VerticalLayoutGroup vlg = content.AddComponent<VerticalLayoutGroup>();
        vlg.childAlignment = TextAnchor.UpperLeft;
        vlg.padding = new RectOffset(40, 40, 40, 40);
        vlg.childControlWidth = true;
        vlg.childControlHeight = false;
        
        ContentSizeFitter csf = content.AddComponent<ContentSizeFitter>();
        csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        
        scrollRect.content = contentRt;
        
        // Story text
        GameObject textObj = new GameObject("StoryText");
        textObj.transform.SetParent(content.transform, false);
        
        contentText = textObj.AddComponent<TextMeshProUGUI>();
        contentText.text = currentStory.content;
        contentText.fontSize = fontSize;
        contentText.color = new Color(0.13f, 0.13f, 0.13f); // Dark gray
        contentText.alignment = TextAlignmentOptions.TopLeft;
        contentText.lineSpacing = 15;
        contentText.enableWordWrapping = true;
    }

    void UpdateContentFontSize()
    {
        if (contentText != null)
        {
            contentText.fontSize = fontSize;
        }
        
        // Update font size display
        Transform fontSizeText = canvas.transform.Find("FontSizeControl/FontSize");
        if (fontSizeText != null)
        {
            TextMeshProUGUI tmp = fontSizeText.GetComponent<TextMeshProUGUI>();
            if (tmp != null)
            {
                tmp.text = fontSize.ToString("0");
            }
        }
    }

    int EstimateReadingTime()
    {
        // Estimate reading time: ~200 words/minute
        int wordCount = currentStory.content.Split(' ').Length;
        return Mathf.Max(1, Mathf.CeilToInt(wordCount / 200f));
    }

    GameObject CreateIconButton(string name, string icon, float width, float height)
    {
        GameObject btnObj = new GameObject(name);
        
        RectTransform rt = btnObj.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(width, height);
        
        Image img = btnObj.AddComponent<Image>();
        img.color = new Color(1, 1, 1, 0.2f);
        img.sprite = CreateCircleSprite();
        
        Button btn = btnObj.AddComponent<Button>();
        btn.targetGraphic = img;
        
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(btnObj.transform, false);
        
        RectTransform textRt = textObj.AddComponent<RectTransform>();
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.sizeDelta = Vector2.zero;
        
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = icon;
        tmp.fontSize = 36;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontStyle = FontStyles.Bold;
        
        return btnObj;
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

    Sprite CreateRoundedRectSprite()
    {
        Texture2D texture = new Texture2D(128, 128);
        Color[] pixels = new Color[128 * 128];
        
        for (int y = 0; y < 128; y++)
        {
            for (int x = 0; x < 128; x++)
            {
                pixels[y * 128 + x] = Color.white;
            }
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        
        return Sprite.Create(texture, new Rect(0, 0, 128, 128), new Vector2(0.5f, 0.5f));
    }
}
