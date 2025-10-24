using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Builds the Story List UI for ar_kid app
/// Replicates the Flutter StoryListPage design with category filter and story cards
/// </summary>
public class StoryListUIBuilder : MonoBehaviour
{
    private Canvas canvas;
    private string selectedCategory = "Tất Cả";
    private List<Story> allStories;
    private GameObject storyListContainer;
    
    void Start()
    {
        Debug.Log("📚 StoryListUIBuilder Started - Building Story List...");
        allStories = StoryData.GetAllStories();
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

        // Create gradient background (Orange → Pink → Purple)
        CreateGradientBackground();
        
        // Create header
        CreateHeader();
        
        // Create category filter
        CreateCategoryFilter();
        
        // Create story list
        CreateStoryList();
        
        Debug.Log("✅ Story List UI built successfully!");
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
        
        // Create gradient texture (Orange → Pink → Purple)
        Image img = bgObj.AddComponent<Image>();
        Texture2D gradientTexture = new Texture2D(1, 256);
        
        for (int i = 0; i < 256; i++)
        {
            float t = i / 255f;
            Color color;
            
            if (t < 0.5f)
            {
                // Orange to Pink
                color = Color.Lerp(
                    new Color(1f, 0.72f, 0.3f),    // Orange (#FFB74D)
                    new Color(0.96f, 0.56f, 0.69f), // Pink (#F48FB1)
                    t * 2f
                );
            }
            else
            {
                // Pink to Purple
                color = Color.Lerp(
                    new Color(0.96f, 0.56f, 0.69f), // Pink (#F48FB1)
                    new Color(0.81f, 0.58f, 0.85f), // Purple (#CE93D8)
                    (t - 0.5f) * 2f
                );
            }
            
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
        rt.sizeDelta = new Vector2(0, 150);
        rt.anchoredPosition = new Vector2(0, 0);
        
        HorizontalLayoutGroup hlg = headerObj.AddComponent<HorizontalLayoutGroup>();
        hlg.childAlignment = TextAnchor.MiddleLeft;
        hlg.padding = new RectOffset(40, 40, 40, 40);
        hlg.spacing = 20;
        hlg.childControlWidth = false;
        hlg.childControlHeight = false;
        
        // Back button
        GameObject backBtn = CreateIconButton("BackButton", "←", 80, 80);
        backBtn.transform.SetParent(headerObj.transform, false);
        backBtn.GetComponent<Button>().onClick.AddListener(() => {
            Debug.Log("Back button clicked - returning to MainMenu");
            SceneTransitionManager.LoadMainMenu();
        });
        
        // Title
        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(headerObj.transform, false);
        
        RectTransform titleRt = titleObj.AddComponent<RectTransform>();
        titleRt.sizeDelta = new Vector2(600, 80);
        
        TextMeshProUGUI titleText = titleObj.AddComponent<TextMeshProUGUI>();
        titleText.text = "Kho Truyện";
        titleText.fontSize = 42;
        titleText.color = Color.white;
        titleText.fontStyle = FontStyles.Bold;
        titleText.alignment = TextAlignmentOptions.Left;
        
        // Spacer
        GameObject spacer = new GameObject("Spacer");
        spacer.transform.SetParent(headerObj.transform, false);
        LayoutElement spacerLayout = spacer.AddComponent<LayoutElement>();
        spacerLayout.flexibleWidth = 1;
        
        // Book icon
        GameObject iconObj = CreateIconButton("BookIcon", "📖", 80, 80);
        iconObj.transform.SetParent(headerObj.transform, false);
        iconObj.GetComponent<Button>().interactable = false; // Just decoration
    }

    void CreateCategoryFilter()
    {
        GameObject filterContainer = new GameObject("CategoryFilter");
        filterContainer.transform.SetParent(canvas.transform, false);
        
        RectTransform rt = filterContainer.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(1, 1);
        rt.pivot = new Vector2(0.5f, 1);
        rt.sizeDelta = new Vector2(0, 120);
        rt.anchoredPosition = new Vector2(0, -150);
        
        // Horizontal scroll view
        GameObject scrollView = new GameObject("ScrollView");
        scrollView.transform.SetParent(filterContainer.transform, false);
        
        RectTransform scrollRt = scrollView.AddComponent<RectTransform>();
        scrollRt.anchorMin = Vector2.zero;
        scrollRt.anchorMax = Vector2.one;
        scrollRt.sizeDelta = Vector2.zero;
        
        ScrollRect scrollRect = scrollView.AddComponent<ScrollRect>();
        scrollRect.horizontal = true;
        scrollRect.vertical = false;
        scrollRect.movementType = ScrollRect.MovementType.Elastic;
        
        // Content container
        GameObject content = new GameObject("Content");
        content.transform.SetParent(scrollView.transform, false);
        
        RectTransform contentRt = content.AddComponent<RectTransform>();
        contentRt.anchorMin = new Vector2(0, 0);
        contentRt.anchorMax = new Vector2(0, 1);
        contentRt.pivot = new Vector2(0, 0.5f);
        
        HorizontalLayoutGroup hlg = content.AddComponent<HorizontalLayoutGroup>();
        hlg.childAlignment = TextAnchor.MiddleLeft;
        hlg.spacing = 20;
        hlg.padding = new RectOffset(40, 40, 20, 20);
        hlg.childControlWidth = false;
        hlg.childControlHeight = false;
        
        ContentSizeFitter csf = content.AddComponent<ContentSizeFitter>();
        csf.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        
        scrollRect.content = contentRt;
        
        // Create category chips
        List<string> categories = StoryData.GetCategories();
        foreach (string category in categories)
        {
            CreateCategoryChip(content.transform, category);
        }
    }

    void CreateCategoryChip(Transform parent, string category)
    {
        GameObject chipObj = new GameObject($"Chip_{category}");
        chipObj.transform.SetParent(parent, false);
        
        RectTransform rt = chipObj.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(200, 70);
        
        bool isSelected = category == selectedCategory;
        
        Image img = chipObj.AddComponent<Image>();
        img.color = isSelected ? Color.white : new Color(1, 1, 1, 0.2f);
        img.sprite = CreateRoundedRectSprite();
        
        Button btn = chipObj.AddComponent<Button>();
        btn.targetGraphic = img;
        btn.onClick.AddListener(() => {
            selectedCategory = category;
            RefreshStoryList();
            RefreshCategoryChips();
        });
        
        // Shadow
        Shadow shadow = chipObj.AddComponent<Shadow>();
        shadow.effectColor = new Color(0, 0, 0, 0.2f);
        shadow.effectDistance = new Vector2(0, 5);
        
        // Text
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(chipObj.transform, false);
        
        RectTransform textRt = textObj.AddComponent<RectTransform>();
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.sizeDelta = Vector2.zero;
        
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = category;
        tmp.fontSize = 24;
        tmp.color = isSelected ? new Color(1f, 0.6f, 0.2f) : Color.white; // Orange if selected
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
    }

    void CreateStoryList()
    {
        GameObject listContainer = new GameObject("StoryListContainer");
        listContainer.transform.SetParent(canvas.transform, false);
        
        RectTransform rt = listContainer.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(1, 1);
        rt.pivot = new Vector2(0.5f, 1);
        rt.offsetMin = new Vector2(0, 0);
        rt.offsetMax = new Vector2(0, -270); // Below header and filter
        
        // Scroll view
        GameObject scrollView = new GameObject("ScrollView");
        scrollView.transform.SetParent(listContainer.transform, false);
        
        RectTransform scrollRt = scrollView.AddComponent<RectTransform>();
        scrollRt.anchorMin = Vector2.zero;
        scrollRt.anchorMax = Vector2.one;
        scrollRt.sizeDelta = Vector2.zero;
        
        ScrollRect scrollRect = scrollView.AddComponent<ScrollRect>();
        scrollRect.horizontal = false;
        scrollRect.vertical = true;
        scrollRect.movementType = ScrollRect.MovementType.Elastic;
        
        // Content
        storyListContainer = new GameObject("Content");
        storyListContainer.transform.SetParent(scrollView.transform, false);
        
        RectTransform contentRt = storyListContainer.AddComponent<RectTransform>();
        contentRt.anchorMin = new Vector2(0, 1);
        contentRt.anchorMax = new Vector2(1, 1);
        contentRt.pivot = new Vector2(0.5f, 1);
        
        VerticalLayoutGroup vlg = storyListContainer.AddComponent<VerticalLayoutGroup>();
        vlg.childAlignment = TextAnchor.UpperCenter;
        vlg.spacing = 30;
        vlg.padding = new RectOffset(40, 40, 40, 40);
        vlg.childControlWidth = true;
        vlg.childControlHeight = false;
        
        ContentSizeFitter csf = storyListContainer.AddComponent<ContentSizeFitter>();
        csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        
        scrollRect.content = contentRt;
        
        // Populate with stories
        RefreshStoryList();
    }

    void RefreshStoryList()
    {
        // Clear existing story cards
        foreach (Transform child in storyListContainer.transform)
        {
            Destroy(child.gameObject);
        }
        
        // Get filtered stories
        List<Story> filteredStories = GetFilteredStories();
        
        if (filteredStories.Count == 0)
        {
            CreateEmptyMessage();
            return;
        }
        
        // Create story cards
        foreach (Story story in filteredStories)
        {
            CreateStoryCard(story);
        }
    }

    List<Story> GetFilteredStories()
    {
        if (selectedCategory == "Tất Cả")
        {
            return allStories;
        }
        return allStories.Where(s => s.category == selectedCategory).ToList();
    }

    void CreateEmptyMessage()
    {
        GameObject msgObj = new GameObject("EmptyMessage");
        msgObj.transform.SetParent(storyListContainer.transform, false);
        
        RectTransform rt = msgObj.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(800, 100);
        
        TextMeshProUGUI tmp = msgObj.AddComponent<TextMeshProUGUI>();
        tmp.text = "Chưa có truyện nào";
        tmp.fontSize = 28;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.Center;
    }

    void CreateStoryCard(Story story)
    {
        GameObject cardObj = new GameObject($"StoryCard_{story.id}");
        cardObj.transform.SetParent(storyListContainer.transform, false);
        
        RectTransform rt = cardObj.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(1000, 280);
        
        // White background
        Image bgImg = cardObj.AddComponent<Image>();
        bgImg.color = Color.white;
        bgImg.sprite = CreateRoundedRectSprite();
        
        // Button for click
        Button btn = cardObj.AddComponent<Button>();
        btn.targetGraphic = bgImg;
        btn.onClick.AddListener(() => {
            Debug.Log($"Story card clicked: {story.title}");
            StoryManager.Instance.ViewStoryDetail(story);
        });
        
        // Shadow
        Shadow shadow = cardObj.AddComponent<Shadow>();
        shadow.effectColor = new Color(0, 0, 0, 0.1f);
        shadow.effectDistance = new Vector2(0, 5);
        
        // Horizontal layout
        GameObject contentObj = new GameObject("Content");
        contentObj.transform.SetParent(cardObj.transform, false);
        
        RectTransform contentRt = contentObj.AddComponent<RectTransform>();
        contentRt.anchorMin = Vector2.zero;
        contentRt.anchorMax = Vector2.one;
        contentRt.sizeDelta = Vector2.zero;
        
        HorizontalLayoutGroup hlg = contentObj.AddComponent<HorizontalLayoutGroup>();
        hlg.childAlignment = TextAnchor.MiddleLeft;
        hlg.padding = new RectOffset(30, 30, 30, 30);
        hlg.spacing = 30;
        hlg.childControlWidth = false;
        hlg.childControlHeight = false;
        
        // Book icon
        CreateBookIcon(contentObj.transform);
        
        // Story info
        CreateStoryInfo(contentObj.transform, story);
        
        // Arrow icon
        CreateArrowIcon(contentObj.transform);
    }

    void CreateBookIcon(Transform parent)
    {
        GameObject iconContainer = new GameObject("IconContainer");
        iconContainer.transform.SetParent(parent, false);
        
        RectTransform rt = iconContainer.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(140, 140);
        
        Image img = iconContainer.AddComponent<Image>();
        img.color = new Color(1f, 0.93f, 0.8f); // Light orange
        img.sprite = CreateRoundedRectSprite();
        
        // Book emoji/icon
        GameObject iconObj = new GameObject("Icon");
        iconObj.transform.SetParent(iconContainer.transform, false);
        
        RectTransform iconRt = iconObj.AddComponent<RectTransform>();
        iconRt.anchorMin = Vector2.zero;
        iconRt.anchorMax = Vector2.one;
        iconRt.sizeDelta = Vector2.zero;
        
        TextMeshProUGUI iconText = iconObj.AddComponent<TextMeshProUGUI>();
        iconText.text = "📖";
        iconText.fontSize = 60;
        iconText.alignment = TextAlignmentOptions.Center;
    }

    void CreateStoryInfo(Transform parent, Story story)
    {
        GameObject infoObj = new GameObject("StoryInfo");
        infoObj.transform.SetParent(parent, false);
        
        RectTransform rt = infoObj.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(700, 240);
        
        VerticalLayoutGroup vlg = infoObj.AddComponent<VerticalLayoutGroup>();
        vlg.childAlignment = TextAnchor.UpperLeft;
        vlg.spacing = 10;
        vlg.childControlWidth = true;
        vlg.childControlHeight = false;
        
        // Title
        CreateStoryTitle(infoObj.transform, story.title);
        
        // Author
        CreateStoryAuthor(infoObj.transform, story.author);
        
        // Summary
        CreateStorySummary(infoObj.transform, story.summary);
        
        // Metadata (category + age)
        CreateStoryMetadata(infoObj.transform, story);
    }

    void CreateStoryTitle(Transform parent, string title)
    {
        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(parent, false);
        
        TextMeshProUGUI tmp = titleObj.AddComponent<TextMeshProUGUI>();
        tmp.text = title;
        tmp.fontSize = 32;
        tmp.color = new Color(0.13f, 0.13f, 0.13f); // Dark gray
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.TopLeft;
    }

    void CreateStoryAuthor(Transform parent, string author)
    {
        GameObject authorObj = new GameObject("Author");
        authorObj.transform.SetParent(parent, false);
        
        TextMeshProUGUI tmp = authorObj.AddComponent<TextMeshProUGUI>();
        tmp.text = author;
        tmp.fontSize = 22;
        tmp.color = new Color(0.4f, 0.4f, 0.4f); // Gray
        tmp.alignment = TextAlignmentOptions.TopLeft;
    }

    void CreateStorySummary(Transform parent, string summary)
    {
        GameObject summaryObj = new GameObject("Summary");
        summaryObj.transform.SetParent(parent, false);
        
        RectTransform rt = summaryObj.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(0, 60);
        
        TextMeshProUGUI tmp = summaryObj.AddComponent<TextMeshProUGUI>();
        tmp.text = summary;
        tmp.fontSize = 22;
        tmp.color = new Color(0.3f, 0.3f, 0.3f);
        tmp.alignment = TextAlignmentOptions.TopLeft;
        tmp.overflowMode = TextOverflowModes.Ellipsis;
        tmp.enableWordWrapping = true;
    }

    void CreateStoryMetadata(Transform parent, Story story)
    {
        GameObject metaObj = new GameObject("Metadata");
        metaObj.transform.SetParent(parent, false);
        
        RectTransform rt = metaObj.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(0, 50);
        
        HorizontalLayoutGroup hlg = metaObj.AddComponent<HorizontalLayoutGroup>();
        hlg.childAlignment = TextAnchor.MiddleLeft;
        hlg.spacing = 15;
        hlg.childControlWidth = false;
        hlg.childControlHeight = false;
        
        // Category badge
        CreateCategoryBadge(metaObj.transform, story.category);
        
        // Age info
        CreateAgeInfo(metaObj.transform, story.ageRange);
    }

    void CreateCategoryBadge(Transform parent, string category)
    {
        GameObject badgeObj = new GameObject("CategoryBadge");
        badgeObj.transform.SetParent(parent, false);
        
        RectTransform rt = badgeObj.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(150, 40);
        
        Image img = badgeObj.AddComponent<Image>();
        img.color = new Color(1f, 0.98f, 0.94f); // Very light orange
        img.sprite = CreateRoundedRectSprite();
        
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(badgeObj.transform, false);
        
        RectTransform textRt = textObj.AddComponent<RectTransform>();
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.sizeDelta = Vector2.zero;
        
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = category;
        tmp.fontSize = 18;
        tmp.color = new Color(1f, 0.6f, 0.2f); // Orange
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
    }

    void CreateAgeInfo(Transform parent, int ageRange)
    {
        GameObject ageObj = new GameObject("AgeInfo");
        ageObj.transform.SetParent(parent, false);
        
        RectTransform rt = ageObj.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(150, 40);
        
        HorizontalLayoutGroup hlg = ageObj.AddComponent<HorizontalLayoutGroup>();
        hlg.childAlignment = TextAnchor.MiddleCenter;
        hlg.spacing = 8;
        
        // Icon
        GameObject iconObj = new GameObject("Icon");
        iconObj.transform.SetParent(ageObj.transform, false);
        
        RectTransform iconRt = iconObj.AddComponent<RectTransform>();
        iconRt.sizeDelta = new Vector2(24, 24);
        
        TextMeshProUGUI iconText = iconObj.AddComponent<TextMeshProUGUI>();
        iconText.text = "👶";
        iconText.fontSize = 20;
        iconText.alignment = TextAlignmentOptions.Center;
        
        // Text
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(ageObj.transform, false);
        
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = $"{ageRange}+ tuổi";
        tmp.fontSize = 18;
        tmp.color = new Color(0.4f, 0.4f, 0.4f);
        tmp.alignment = TextAlignmentOptions.Left;
    }

    void CreateArrowIcon(Transform parent)
    {
        GameObject arrowObj = new GameObject("Arrow");
        arrowObj.transform.SetParent(parent, false);
        
        RectTransform rt = arrowObj.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(40, 40);
        
        TextMeshProUGUI tmp = arrowObj.AddComponent<TextMeshProUGUI>();
        tmp.text = "→";
        tmp.fontSize = 32;
        tmp.color = new Color(0.7f, 0.7f, 0.7f);
        tmp.alignment = TextAlignmentOptions.Center;
    }

    void RefreshCategoryChips()
    {
        // Find category filter and rebuild chips
        Transform filterTransform = canvas.transform.Find("CategoryFilter/ScrollView/Content");
        if (filterTransform != null)
        {
            foreach (Transform child in filterTransform)
            {
                Destroy(child.gameObject);
            }
            
            List<string> categories = StoryData.GetCategories();
            foreach (string category in categories)
            {
                CreateCategoryChip(filterTransform, category);
            }
        }
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
