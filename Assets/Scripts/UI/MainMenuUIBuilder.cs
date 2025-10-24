using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

/// <summary>
/// Builds the Main Menu UI for ar_kid app
/// Replicates the Flutter HomePage design with Quiz, Stories, and AR buttons
/// </summary>


[ExecuteAlways]
public class MainMenuUIBuilder : MonoBehaviour
{
    private Canvas canvas;
    private GameObject mainContainer;

    void Start()
    {
        Debug.Log("🏠 MainMenuUIBuilder Started - Building ar_kid Main Menu...");
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

        // Create gradient background
        CreateGradientBackground();

        // Create main container
        CreateMainContainer();

        // Create main icon
        CreateMainIcon();

        // Create title
        CreateTitle();

        // Create subtitle
        CreateSubtitle();

        // Create buttons
        CreateQuizButton();
        CreateStoryButton();
        CreateARButton();

        // Create info panel
        CreateInfoPanel();

        Debug.Log("✅ ar_kid Main Menu UI built successfully!");
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

        // Create gradient texture (Purple → Blue → Pink)
        Image img = bgObj.AddComponent<Image>();
        Texture2D gradientTexture = new Texture2D(1, 256);

        for (int i = 0; i < 256; i++)
        {
            float t = i / 255f;
            Color color;

            if (t < 0.5f)
            {
                // Purple to Blue
                color = Color.Lerp(
                    new Color(0.73f, 0.41f, 0.78f), // Purple (#BA68C8)
                    new Color(0.39f, 0.71f, 0.96f), // Blue (#64B5F6)
                    t * 2f
                );
            }
            else
            {
                // Blue to Pink
                color = Color.Lerp(
                    new Color(0.39f, 0.71f, 0.96f), // Blue (#64B5F6)
                    new Color(0.96f, 0.56f, 0.69f), // Pink (#F48FB1)
                    (t - 0.5f) * 2f
                );
            }

            gradientTexture.SetPixel(0, i, color);
        }

        gradientTexture.Apply();
        img.sprite = Sprite.Create(gradientTexture, new Rect(0, 0, 1, 256), new Vector2(0.5f, 0.5f));

        // Set to back
        bgObj.transform.SetAsFirstSibling();
    }

    void CreateMainContainer()
    {
        mainContainer = new GameObject("MainContainer");
        mainContainer.transform.SetParent(canvas.transform, false);

        RectTransform rt = mainContainer.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;
        rt.anchoredPosition = Vector2.zero;

        // Add VerticalLayoutGroup for auto-layout
        VerticalLayoutGroup vlg = mainContainer.AddComponent<VerticalLayoutGroup>();
        vlg.childAlignment = TextAnchor.MiddleCenter;
        vlg.childControlHeight = false;
        vlg.childControlWidth = false;
        vlg.childForceExpandHeight = false;
        vlg.childForceExpandWidth = false;
        vlg.spacing = 40;
        vlg.padding = new RectOffset(80, 80, 200, 200);
    }

    void CreateMainIcon()
    {
        GameObject iconContainer = new GameObject("IconContainer");
        iconContainer.transform.SetParent(mainContainer.transform, false);

        RectTransform containerRt = iconContainer.AddComponent<RectTransform>();
        containerRt.sizeDelta = new Vector2(260, 260);

        // White circle background
        Image bgImage = iconContainer.AddComponent<Image>();
        bgImage.color = Color.white;
        bgImage.sprite = CreateCircleSprite();

        // Add shadow effect
        Shadow shadow = iconContainer.AddComponent<Shadow>();
        shadow.effectColor = new Color(0, 0, 0, 0.2f);
        shadow.effectDistance = new Vector2(0, 10);

        // Quiz icon (using TextMeshPro as icon placeholder)
        GameObject iconObj = new GameObject("Icon");
        iconObj.transform.SetParent(iconContainer.transform, false);

        RectTransform iconRt = iconObj.AddComponent<RectTransform>();
        iconRt.anchorMin = Vector2.zero;
        iconRt.anchorMax = Vector2.one;
        iconRt.sizeDelta = Vector2.zero;

        TextMeshProUGUI iconText = iconObj.AddComponent<TextMeshProUGUI>();
        iconText.text = "?";
        iconText.fontSize = 120;
        iconText.color = new Color(0.73f, 0.41f, 0.78f); // Purple
        iconText.alignment = TextAlignmentOptions.Center;
        iconText.fontStyle = FontStyles.Bold;

        // Add scale animation
        StartCoroutine(AnimateScale(iconContainer.transform, 0.8f, 1.0f, 1.5f));
    }

    void CreateTitle()
    {
        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(mainContainer.transform, false);

        RectTransform rt = titleObj.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(800, 120);

        TextMeshProUGUI tmp = titleObj.AddComponent<TextMeshProUGUI>();
        tmp.text = "Quiz Vui Nhộn";
        tmp.fontSize = 72;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontStyle = FontStyles.Bold;

        // Add shadow
        Shadow shadow = titleObj.AddComponent<Shadow>();
        shadow.effectColor = new Color(0, 0, 0, 0.26f);
        shadow.effectDistance = new Vector2(2, 2);
    }

    void CreateSubtitle()
    {
        GameObject subtitleObj = new GameObject("Subtitle");
        subtitleObj.transform.SetParent(mainContainer.transform, false);

        RectTransform rt = subtitleObj.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(800, 100);

        TextMeshProUGUI tmp = subtitleObj.AddComponent<TextMeshProUGUI>();
        tmp.text = "Hãy kiểm tra kiến thức của bạn!\nBạn trả lời đúng được bao nhiêu câu?";
        tmp.fontSize = 28;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontStyle = FontStyles.Normal;
    }

    void CreateQuizButton()
    {
        GameObject buttonObj = CreateButton(
            name: "QuizButton",
            text: "▶ Bắt Đầu Quiz",
            bgColor: Color.white,
            textColor: new Color(0.73f, 0.41f, 0.78f), // Purple
            width: 600,
            height: 120
        );

        Button btn = buttonObj.GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            Debug.Log("Quiz button clicked!");
            SceneTransitionManager.LoadQuiz();
        });

        // Add scale animation
        StartCoroutine(AnimateScale(buttonObj.transform, 0.8f, 1.0f, 1.5f));
    }

    void CreateStoryButton()
    {
        GameObject buttonObj = CreateButton(
            name: "StoryButton",
            text: "📖 Đọc Truyện",
            bgColor: new Color(1f, 0.6f, 0.2f), // Orange
            textColor: Color.white,
            width: 550,
            height: 110
        );

        Button btn = buttonObj.GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            Debug.Log("Story button clicked!");
            SceneTransitionManager.LoadStoryList();
        });

        // Add scale animation with delay
        StartCoroutine(AnimateScale(buttonObj.transform, 0.8f, 1.0f, 1.5f, 0.1f));
    }

    void CreateARButton()
    {
        GameObject buttonObj = CreateButton(
            name: "ARButton",
            text: "📷 AR Quét Thẻ",
            bgColor: new Color(0.2f, 0.7f, 0.9f), // Light Blue
            textColor: Color.white,
            width: 550,
            height: 110
        );

        Button btn = buttonObj.GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            Debug.Log("AR button clicked!");
            SceneTransitionManager.LoadARHome();
        });

        // Add scale animation with delay
        StartCoroutine(AnimateScale(buttonObj.transform, 0.8f, 1.0f, 1.5f, 0.2f));
    }

    GameObject CreateButton(string name, string text, Color bgColor, Color textColor, float width, float height)
    {
        GameObject buttonObj = new GameObject(name);
        buttonObj.transform.SetParent(mainContainer.transform, false);

        RectTransform rt = buttonObj.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(width, height);

        // Background image
        Image img = buttonObj.AddComponent<Image>();
        img.color = bgColor;
        img.sprite = CreateRoundedRectSprite();

        // Button component
        Button btn = buttonObj.AddComponent<Button>();
        btn.targetGraphic = img;

        // Add shadow
        Shadow shadow = buttonObj.AddComponent<Shadow>();
        shadow.effectColor = new Color(0, 0, 0, 0.38f);
        shadow.effectDistance = new Vector2(0, 8);

        // Button text
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);

        RectTransform textRt = textObj.AddComponent<RectTransform>();
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.sizeDelta = Vector2.zero;

        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 36;
        tmp.color = textColor;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontStyle = FontStyles.Bold;

        return buttonObj;
    }

    void CreateInfoPanel()
    {
        GameObject panelObj = new GameObject("InfoPanel");
        panelObj.transform.SetParent(mainContainer.transform, false);

        RectTransform rt = panelObj.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(700, 150);

        // Semi-transparent white background
        Image img = panelObj.AddComponent<Image>();
        img.color = new Color(1, 1, 1, 0.2f);
        img.sprite = CreateRoundedRectSprite();

        // Vertical layout for info items
        VerticalLayoutGroup vlg = panelObj.AddComponent<VerticalLayoutGroup>();
        vlg.childAlignment = TextAnchor.MiddleCenter;
        vlg.spacing = 20;
        vlg.padding = new RectOffset(40, 40, 30, 30);

        // Info item 1: Questions count
        CreateInfoItem(panelObj.transform, "❓ 10 Câu Hỏi");

        // Info item 2: Points
        CreateInfoItem(panelObj.transform, "⭐ Mỗi câu đúng: 10 điểm");
    }

    void CreateInfoItem(Transform parent, string text)
    {
        GameObject itemObj = new GameObject("InfoItem");
        itemObj.transform.SetParent(parent, false);

        RectTransform rt = itemObj.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(600, 40);

        TextMeshProUGUI tmp = itemObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 26;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontStyle = FontStyles.Bold;
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

    IEnumerator AnimateScale(Transform target, float startScale, float endScale, float duration, float delay = 0f)
    {
        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }

        float elapsed = 0f;
        Vector3 start = Vector3.one * startScale;
        Vector3 end = Vector3.one * endScale;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Ease out back curve
            t = 1f - Mathf.Pow(1f - t, 3f);

            target.localScale = Vector3.Lerp(start, end, t);
            yield return null;
        }

        target.localScale = end;
    }
}
