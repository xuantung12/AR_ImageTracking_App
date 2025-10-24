using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

/// <summary>
/// Builds the Quiz UI for ar_kid app
/// Replicates the Flutter QuizPage design with questions, answers, and result screen
/// </summary>
public class QuizUIBuilder : MonoBehaviour
{
    private Canvas canvas;
    private QuizManager quizManager;
    private GameObject quizContainer;
    private GameObject resultContainer;
    private GameObject[] answerButtons;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI questionCounterText;
    private TextMeshProUGUI progressPercentageText;
    private Image progressBar;
    private TextMeshProUGUI questionText;
    
    void Start()
    {
        Debug.Log("🎯 QuizUIBuilder Started - Building Quiz UI...");

        // Get or create QuizManager
        quizManager = FindObjectOfType<QuizManager>();
        if (quizManager == null)
        {
            GameObject managerObj = new GameObject("QuizManager");
            quizManager = managerObj.AddComponent<QuizManager>();
        }

        // Subscribe to quiz events
        quizManager.OnQuizStateChanged += RefreshUI;
        quizManager.OnAnswerSelected += OnAnswerSelected;
        quizManager.OnQuizCompleted += OnQuizCompleted;

        // Wait one frame for QuizManager.Start() to complete
        StartCoroutine(BuildUINextFrame());
    }

    IEnumerator BuildUINextFrame()
    {
        yield return null; // Wait one frame
        BuildUI();
    }

    void OnDestroy()
    {
        // Unsubscribe from events
        if (quizManager != null)
        {
            quizManager.OnQuizStateChanged -= RefreshUI;
            quizManager.OnAnswerSelected -= OnAnswerSelected;
            quizManager.OnQuizCompleted -= OnQuizCompleted;
        }
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
        
        // Create quiz container (for questions)
        CreateQuizContainer();
        
        // Create result container (hidden initially)
        CreateResultContainer();
        
        // Initial refresh
        RefreshUI();
        
        Debug.Log("✅ Quiz UI built successfully!");
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
        
        // Create gradient texture (Purple → Light Purple)
        Image img = bgObj.AddComponent<Image>();
        Texture2D gradientTexture = new Texture2D(1, 256);
        
        for (int i = 0; i < 256; i++)
        {
            float t = i / 255f;
            Color color = Color.Lerp(
                new Color(0.61f, 0.35f, 0.71f), // Purple (#9C59B6)
                new Color(0.73f, 0.41f, 0.78f), // Light Purple (#BA68C8)
                t
            );
            gradientTexture.SetPixel(0, i, color);
        }
        
        gradientTexture.Apply();
        img.sprite = Sprite.Create(gradientTexture, new Rect(0, 0, 1, 256), new Vector2(0.5f, 0.5f));
        
        bgObj.transform.SetAsFirstSibling();
    }

    void CreateQuizContainer()
    {
        quizContainer = new GameObject("QuizContainer");
        quizContainer.transform.SetParent(canvas.transform, false);
        
        RectTransform rt = quizContainer.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;
        
        // Create header with score
        CreateQuizHeader();
        
        // Create progress section
        CreateProgressSection();
        
        // Create question card
        CreateQuestionCard();
        
        // Create answer buttons
        CreateAnswerButtons();
    }

    void CreateQuizHeader()
    {
        GameObject headerObj = new GameObject("Header");
        headerObj.transform.SetParent(quizContainer.transform, false);
        
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
        titleRt.sizeDelta = new Vector2(500, 80);
        
        TextMeshProUGUI titleText = titleObj.AddComponent<TextMeshProUGUI>();
        titleText.text = "Quiz Vui Nhộn";
        titleText.fontSize = 38;
        titleText.color = Color.white;
        titleText.fontStyle = FontStyles.Bold;
        titleText.alignment = TextAlignmentOptions.Left;
        
        // Spacer
        GameObject spacer = new GameObject("Spacer");
        spacer.transform.SetParent(headerObj.transform, false);
        LayoutElement spacerLayout = spacer.AddComponent<LayoutElement>();
        spacerLayout.flexibleWidth = 1;
        
        // Score display
        CreateScoreDisplay(headerObj.transform);
    }

    void CreateScoreDisplay(Transform parent)
    {
        GameObject scoreContainer = new GameObject("ScoreContainer");
        scoreContainer.transform.SetParent(parent, false);
        
        RectTransform rt = scoreContainer.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(180, 70);
        
        Image bgImg = scoreContainer.AddComponent<Image>();
        bgImg.color = Color.white;
        bgImg.sprite = CreateRoundedRectSprite();
        
        HorizontalLayoutGroup hlg = scoreContainer.AddComponent<HorizontalLayoutGroup>();
        hlg.childAlignment = TextAnchor.MiddleCenter;
        hlg.spacing = 10;
        hlg.padding = new RectOffset(20, 20, 15, 15);
        
        // Star icon
        GameObject starObj = new GameObject("Star");
        starObj.transform.SetParent(scoreContainer.transform, false);
        
        TextMeshProUGUI starText = starObj.AddComponent<TextMeshProUGUI>();
        starText.text = "⭐";
        starText.fontSize = 28;
        starText.alignment = TextAlignmentOptions.Center;
        
        // Score text
        GameObject scoreObj = new GameObject("Score");
        scoreObj.transform.SetParent(scoreContainer.transform, false);
        
        scoreText = scoreObj.AddComponent<TextMeshProUGUI>();
        scoreText.text = "0";
        scoreText.fontSize = 28;
        scoreText.color = new Color(0.61f, 0.35f, 0.71f); // Purple
        scoreText.fontStyle = FontStyles.Bold;
        scoreText.alignment = TextAlignmentOptions.Center;
    }

    void CreateProgressSection()
    {
        GameObject progressObj = new GameObject("ProgressSection");
        progressObj.transform.SetParent(quizContainer.transform, false);
        
        RectTransform rt = progressObj.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(1, 1);
        rt.pivot = new Vector2(0.5f, 1);
        rt.sizeDelta = new Vector2(-80, 120);
        rt.anchoredPosition = new Vector2(0, -170);
        
        VerticalLayoutGroup vlg = progressObj.AddComponent<VerticalLayoutGroup>();
        vlg.childAlignment = TextAnchor.UpperCenter;
        vlg.spacing = 15;
        vlg.childControlWidth = true;
        vlg.childControlHeight = false;
        
        // Progress text (Câu X/10 - XX%)
        GameObject textRow = new GameObject("ProgressText");
        textRow.transform.SetParent(progressObj.transform, false);
        
        RectTransform textRt = textRow.AddComponent<RectTransform>();
        textRt.sizeDelta = new Vector2(0, 40);
        
        HorizontalLayoutGroup textHlg = textRow.AddComponent<HorizontalLayoutGroup>();
        textHlg.childAlignment = TextAnchor.MiddleLeft;
        textHlg.childControlWidth = false;
        
        // Question counter
        GameObject counterObj = new GameObject("Counter");
        counterObj.transform.SetParent(textRow.transform, false);
        
        questionCounterText = counterObj.AddComponent<TextMeshProUGUI>();
        questionCounterText.text = "Câu 1/10";
        questionCounterText.fontSize = 28;
        questionCounterText.color = Color.white;
        questionCounterText.fontStyle = FontStyles.Bold;
        questionCounterText.alignment = TextAlignmentOptions.Left;
        
        // Spacer
        GameObject spacer = new GameObject("Spacer");
        spacer.transform.SetParent(textRow.transform, false);
        LayoutElement spacerLayout = spacer.AddComponent<LayoutElement>();
        spacerLayout.flexibleWidth = 1;
        
        // Percentage
        GameObject percentObj = new GameObject("Percentage");
        percentObj.transform.SetParent(textRow.transform, false);
        
        progressPercentageText = percentObj.AddComponent<TextMeshProUGUI>();
        progressPercentageText.text = "10%";
        progressPercentageText.fontSize = 28;
        progressPercentageText.color = Color.white;
        progressPercentageText.fontStyle = FontStyles.Bold;
        progressPercentageText.alignment = TextAlignmentOptions.Right;
        
        // Progress bar
        GameObject barContainer = new GameObject("ProgressBar");
        barContainer.transform.SetParent(progressObj.transform, false);
        
        RectTransform barRt = barContainer.AddComponent<RectTransform>();
        barRt.sizeDelta = new Vector2(0, 20);
        
        Image barBg = barContainer.AddComponent<Image>();
        barBg.color = new Color(1, 1, 1, 0.3f);
        barBg.sprite = CreateRoundedRectSprite();
        
        // Progress fill
        GameObject fillObj = new GameObject("Fill");
        fillObj.transform.SetParent(barContainer.transform, false);
        
        RectTransform fillRt = fillObj.AddComponent<RectTransform>();
        fillRt.anchorMin = new Vector2(0, 0);
        fillRt.anchorMax = new Vector2(0, 1);
        fillRt.pivot = new Vector2(0, 0.5f);
        fillRt.sizeDelta = new Vector2(100, 0);
        fillRt.anchoredPosition = Vector2.zero;
        
        progressBar = fillObj.AddComponent<Image>();
        progressBar.color = Color.white;
        progressBar.sprite = CreateRoundedRectSprite();
        progressBar.type = Image.Type.Filled;
        progressBar.fillMethod = Image.FillMethod.Horizontal;
        progressBar.fillAmount = 0.1f;
    }

    void CreateQuestionCard()
    {
        GameObject cardObj = new GameObject("QuestionCard");
        cardObj.transform.SetParent(quizContainer.transform, false);
        
        RectTransform rt = cardObj.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 1);
        rt.anchorMax = new Vector2(0.5f, 1);
        rt.pivot = new Vector2(0.5f, 1);
        rt.sizeDelta = new Vector2(1000, 300);
        rt.anchoredPosition = new Vector2(0, -330);
        
        // White background
        Image bgImg = cardObj.AddComponent<Image>();
        bgImg.color = Color.white;
        bgImg.sprite = CreateRoundedRectSprite();
        
        // Shadow
        Shadow shadow = cardObj.AddComponent<Shadow>();
        shadow.effectColor = new Color(0, 0, 0, 0.1f);
        shadow.effectDistance = new Vector2(0, 5);
        
        // Question text
        GameObject textObj = new GameObject("QuestionText");
        textObj.transform.SetParent(cardObj.transform, false);
        
        RectTransform textRt = textObj.AddComponent<RectTransform>();
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.sizeDelta = new Vector2(-80, -80);
        textRt.anchoredPosition = Vector2.zero;
        
        questionText = textObj.AddComponent<TextMeshProUGUI>();
        questionText.text = "Loading question...";
        questionText.fontSize = 36;
        questionText.color = new Color(0.61f, 0.35f, 0.71f); // Purple
        questionText.fontStyle = FontStyles.Bold;
        questionText.alignment = TextAlignmentOptions.Center;
        questionText.enableWordWrapping = true;
    }

    void CreateAnswerButtons()
    {
        GameObject answersContainer = new GameObject("AnswersContainer");
        answersContainer.transform.SetParent(quizContainer.transform, false);
        
        RectTransform rt = answersContainer.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(1, 1);
        rt.pivot = new Vector2(0.5f, 1);
        rt.offsetMin = new Vector2(40, 40);
        rt.offsetMax = new Vector2(-40, -670);
        
        VerticalLayoutGroup vlg = answersContainer.AddComponent<VerticalLayoutGroup>();
        vlg.childAlignment = TextAnchor.UpperCenter;
        vlg.spacing = 30;
        vlg.childControlWidth = true;
        vlg.childControlHeight = false;
        
        // Create 4 answer buttons
        answerButtons = new GameObject[4];
        for (int i = 0; i < 4; i++)
        {
            answerButtons[i] = CreateAnswerButton(answersContainer.transform, i);
        }
    }

    GameObject CreateAnswerButton(Transform parent, int index)
    {
        GameObject btnObj = new GameObject($"AnswerButton_{index}");
        btnObj.transform.SetParent(parent, false);
        
        RectTransform rt = btnObj.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(0, 140);
        
        Image bgImg = btnObj.AddComponent<Image>();
        bgImg.color = Color.white;
        bgImg.sprite = CreateRoundedRectSprite();
        
        Button btn = btnObj.AddComponent<Button>();
        btn.targetGraphic = bgImg;
        
        int answerIndex = index;
        btn.onClick.AddListener(() => {
            quizManager.SelectAnswer(answerIndex);
        });
        
        // Shadow
        Shadow shadow = btnObj.AddComponent<Shadow>();
        shadow.effectColor = new Color(0, 0, 0, 0.1f);
        shadow.effectDistance = new Vector2(0, 3);
        
        // Content container
        GameObject contentObj = new GameObject("Content");
        contentObj.transform.SetParent(btnObj.transform, false);
        
        RectTransform contentRt = contentObj.AddComponent<RectTransform>();
        contentRt.anchorMin = Vector2.zero;
        contentRt.anchorMax = Vector2.one;
        contentRt.sizeDelta = new Vector2(-40, -40);
        
        HorizontalLayoutGroup hlg = contentObj.AddComponent<HorizontalLayoutGroup>();
        hlg.childAlignment = TextAnchor.MiddleLeft;
        hlg.spacing = 30;
        hlg.childControlWidth = false;
        hlg.childControlHeight = false;
        
        // Letter circle (A, B, C, D)
        GameObject letterObj = new GameObject("Letter");
        letterObj.transform.SetParent(contentObj.transform, false);
        
        RectTransform letterRt = letterObj.AddComponent<RectTransform>();
        letterRt.sizeDelta = new Vector2(80, 80);
        
        Image letterBg = letterObj.AddComponent<Image>();
        letterBg.color = new Color(0.61f, 0.35f, 0.71f, 0.1f); // Light purple
        letterBg.sprite = CreateCircleSprite();
        
        GameObject letterTextObj = new GameObject("LetterText");
        letterTextObj.transform.SetParent(letterObj.transform, false);
        
        RectTransform letterTextRt = letterTextObj.AddComponent<RectTransform>();
        letterTextRt.anchorMin = Vector2.zero;
        letterTextRt.anchorMax = Vector2.one;
        letterTextRt.sizeDelta = Vector2.zero;
        
        TextMeshProUGUI letterText = letterTextObj.AddComponent<TextMeshProUGUI>();
        letterText.text = ((char)('A' + index)).ToString();
        letterText.fontSize = 32;
        letterText.color = new Color(0.61f, 0.35f, 0.71f); // Purple
        letterText.fontStyle = FontStyles.Bold;
        letterText.alignment = TextAlignmentOptions.Center;
        
        // Answer text
        GameObject answerTextObj = new GameObject("AnswerText");
        answerTextObj.transform.SetParent(contentObj.transform, false);
        
        RectTransform answerTextRt = answerTextObj.AddComponent<RectTransform>();
        answerTextRt.sizeDelta = new Vector2(750, 80);
        
        TextMeshProUGUI answerText = answerTextObj.AddComponent<TextMeshProUGUI>();
        answerText.text = "Answer option";
        answerText.fontSize = 28;
        answerText.color = new Color(0.61f, 0.35f, 0.71f); // Purple
        answerText.fontStyle = FontStyles.Bold;
        answerText.alignment = TextAlignmentOptions.Left;
        answerText.enableWordWrapping = true;
        
        // Icon (check/cross) - hidden initially
        GameObject iconObj = new GameObject("Icon");
        iconObj.transform.SetParent(contentObj.transform, false);
        iconObj.SetActive(false);
        
        TextMeshProUGUI iconText = iconObj.AddComponent<TextMeshProUGUI>();
        iconText.text = "✓";
        iconText.fontSize = 48;
        iconText.color = Color.white;
        iconText.alignment = TextAlignmentOptions.Center;
        
        return btnObj;
    }

    void CreateResultContainer()
    {
        resultContainer = new GameObject("ResultContainer");
        resultContainer.transform.SetParent(canvas.transform, false);
        resultContainer.SetActive(false); // Hidden initially
        
        RectTransform rt = resultContainer.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;
        
        VerticalLayoutGroup vlg = resultContainer.AddComponent<VerticalLayoutGroup>();
        vlg.childAlignment = TextAnchor.MiddleCenter;
        vlg.spacing = 40;
        vlg.padding = new RectOffset(80, 80, 200, 200);
        vlg.childControlWidth = false;
        vlg.childControlHeight = false;
        
        // Result icon (will be set dynamically)
        GameObject iconObj = new GameObject("ResultIcon");
        iconObj.transform.SetParent(resultContainer.transform, false);
        
        RectTransform iconRt = iconObj.AddComponent<RectTransform>();
        iconRt.sizeDelta = new Vector2(200, 200);
        
        TextMeshProUGUI iconText = iconObj.AddComponent<TextMeshProUGUI>();
        iconText.name = "IconText";
        iconText.text = "🏆";
        iconText.fontSize = 120;
        iconText.alignment = TextAlignmentOptions.Center;
        
        // Result message
        GameObject msgObj = new GameObject("ResultMessage");
        msgObj.transform.SetParent(resultContainer.transform, false);
        
        TextMeshProUGUI msgText = msgObj.AddComponent<TextMeshProUGUI>();
        msgText.name = "MessageText";
        msgText.text = "Xuất sắc!";
        msgText.fontSize = 60;
        msgText.color = Color.white;
        msgText.fontStyle = FontStyles.Bold;
        msgText.alignment = TextAlignmentOptions.Center;
        
        // Score card
        CreateResultScoreCard(resultContainer.transform);
        
        // Retry button
        CreateResultButton(resultContainer.transform, "🔄 Làm Lại", Color.white, new Color(0.61f, 0.35f, 0.71f), () => {
            quizManager.RestartQuiz();
        });
        
        // Home button
        CreateResultButton(resultContainer.transform, "🏠 Về Trang Chủ", new Color(0.61f, 0.35f, 0.71f), Color.white, () => {
            quizManager.ReturnToMainMenu();
        });
    }

    void CreateResultScoreCard(Transform parent)
    {
        GameObject cardObj = new GameObject("ScoreCard");
        cardObj.transform.SetParent(parent, false);
        
        RectTransform rt = cardObj.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(700, 400);
        
        Image bgImg = cardObj.AddComponent<Image>();
        bgImg.color = Color.white;
        bgImg.sprite = CreateRoundedRectSprite();
        
        Shadow shadow = cardObj.AddComponent<Shadow>();
        shadow.effectColor = new Color(0, 0, 0, 0.2f);
        shadow.effectDistance = new Vector2(0, 8);
        
        VerticalLayoutGroup vlg = cardObj.AddComponent<VerticalLayoutGroup>();
        vlg.childAlignment = TextAnchor.MiddleCenter;
        vlg.spacing = 20;
        vlg.padding = new RectOffset(60, 60, 60, 60);
        vlg.childControlWidth = false;
        vlg.childControlHeight = false;
        
        // "Điểm số của bạn"
        GameObject labelObj = new GameObject("Label");
        labelObj.transform.SetParent(cardObj.transform, false);
        
        TextMeshProUGUI labelText = labelObj.AddComponent<TextMeshProUGUI>();
        labelText.text = "Điểm số của bạn";
        labelText.fontSize = 32;
        labelText.color = new Color(0.61f, 0.35f, 0.71f);
        labelText.fontStyle = FontStyles.Bold;
        labelText.alignment = TextAlignmentOptions.Center;
        
        // Score display
        GameObject scoreRow = new GameObject("ScoreRow");
        scoreRow.transform.SetParent(cardObj.transform, false);
        
        HorizontalLayoutGroup scoreHlg = scoreRow.AddComponent<HorizontalLayoutGroup>();
        scoreHlg.childAlignment = TextAnchor.MiddleCenter;
        scoreHlg.spacing = 0;
        
        GameObject scoreObj = new GameObject("Score");
        scoreObj.transform.SetParent(scoreRow.transform, false);
        
        TextMeshProUGUI scoreText = scoreObj.AddComponent<TextMeshProUGUI>();
        scoreText.name = "ScoreText";
        scoreText.text = "100";
        scoreText.fontSize = 90;
        scoreText.color = new Color(0.61f, 0.35f, 0.71f);
        scoreText.fontStyle = FontStyles.Bold;
        scoreText.alignment = TextAlignmentOptions.Center;
        
        GameObject maxScoreObj = new GameObject("MaxScore");
        maxScoreObj.transform.SetParent(scoreRow.transform, false);
        
        TextMeshProUGUI maxScoreText = maxScoreObj.AddComponent<TextMeshProUGUI>();
        maxScoreText.text = "/100";
        maxScoreText.fontSize = 45;
        maxScoreText.color = new Color(0.61f, 0.35f, 0.71f);
        maxScoreText.fontStyle = FontStyles.Bold;
        maxScoreText.alignment = TextAlignmentOptions.Center;
        
        // Percentage
        GameObject percentObj = new GameObject("Percentage");
        percentObj.transform.SetParent(cardObj.transform, false);
        
        TextMeshProUGUI percentText = percentObj.AddComponent<TextMeshProUGUI>();
        percentText.name = "PercentageText";
        percentText.text = "100%";
        percentText.fontSize = 36;
        percentText.color = new Color(1f, 0.76f, 0.03f); // Gold
        percentText.fontStyle = FontStyles.Bold;
        percentText.alignment = TextAlignmentOptions.Center;
    }

    void CreateResultButton(Transform parent, string text, Color bgColor, Color textColor, UnityEngine.Events.UnityAction onClick)
    {
        GameObject btnObj = new GameObject("ResultButton");
        btnObj.transform.SetParent(parent, false);
        
        RectTransform rt = btnObj.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(600, 120);
        
        Image bgImg = btnObj.AddComponent<Image>();
        bgImg.color = bgColor;
        bgImg.sprite = CreateRoundedRectSprite();
        
        Button btn = btnObj.AddComponent<Button>();
        btn.targetGraphic = bgImg;
        btn.onClick.AddListener(onClick);
        
        Shadow shadow = btnObj.AddComponent<Shadow>();
        shadow.effectColor = new Color(0, 0, 0, 0.3f);
        shadow.effectDistance = new Vector2(0, 5);
        
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(btnObj.transform, false);
        
        RectTransform textRt = textObj.AddComponent<RectTransform>();
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.sizeDelta = Vector2.zero;
        
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 32;
        tmp.color = textColor;
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
    }

    // ========== UI UPDATE METHODS ==========
    
    void RefreshUI()
    {
        // Check if UI is built yet
        if (quizContainer == null || resultContainer == null)
        {
            return; // UI not ready yet
        }

        if (quizManager.IsQuizCompleted())
        {
            ShowResultScreen();
        }
        else
        {
            ShowQuizScreen();
            UpdateQuizUI();
        }
    }

    void ShowQuizScreen()
    {
        if (quizContainer != null) quizContainer.SetActive(true);
        if (resultContainer != null) resultContainer.SetActive(false);
    }

    void ShowResultScreen()
    {
        if (quizContainer != null) quizContainer.SetActive(false);
        if (resultContainer != null) resultContainer.SetActive(true);
        UpdateResultUI();
    }

    void UpdateQuizUI()
    {
        // Update score
        scoreText.text = quizManager.GetScore().ToString();
        
        // Update progress
        int currentIndex = quizManager.GetCurrentQuestionIndex();
        int totalQuestions = quizManager.GetTotalQuestions();
        questionCounterText.text = $"Câu {currentIndex + 1}/{totalQuestions}";
        progressPercentageText.text = $"{quizManager.GetProgressPercentage()}%";
        progressBar.fillAmount = (float)(currentIndex + 1) / totalQuestions;
        
        // Update question
        QuizQuestion currentQuestion = quizManager.GetCurrentQuestion();
        if (currentQuestion != null)
        {
            questionText.text = currentQuestion.question;
            
            // Update answer buttons
            for (int i = 0; i < 4; i++)
            {
                if (i < currentQuestion.options.Length)
                {
                    UpdateAnswerButton(answerButtons[i], currentQuestion.options[i], i, currentQuestion.correctAnswer);
                }
            }
        }
    }

    void UpdateAnswerButton(GameObject button, string answerText, int index, int correctIndex)
    {
        // Update answer text
        Transform answerTextTransform = button.transform.Find("Content/AnswerText");
        if (answerTextTransform != null)
        {
            TextMeshProUGUI tmp = answerTextTransform.GetComponent<TextMeshProUGUI>();
            if (tmp != null)
            {
                tmp.text = answerText;
            }
        }
        
        // Reset button appearance if not answered yet
        if (!quizManager.HasAnswered())
        {
            Image bgImg = button.GetComponent<Image>();
            if (bgImg != null)
            {
                bgImg.color = Color.white;
            }
            
            // Hide icon
            Transform iconTransform = button.transform.Find("Content/Icon");
            if (iconTransform != null)
            {
                iconTransform.gameObject.SetActive(false);
            }
            
            // Reset text colors
            Transform letterTextTransform = button.transform.Find("Content/Letter/LetterText");
            if (letterTextTransform != null)
            {
                TextMeshProUGUI tmp = letterTextTransform.GetComponent<TextMeshProUGUI>();
                if (tmp != null)
                {
                    tmp.color = new Color(0.61f, 0.35f, 0.71f);
                }
            }
            
            if (answerTextTransform != null)
            {
                TextMeshProUGUI tmp = answerTextTransform.GetComponent<TextMeshProUGUI>();
                if (tmp != null)
                {
                    tmp.color = new Color(0.61f, 0.35f, 0.71f);
                }
            }
        }
    }

    void OnAnswerSelected(int selectedIndex, bool isCorrect)
    {
        QuizQuestion currentQuestion = quizManager.GetCurrentQuestion();
        if (currentQuestion == null) return;
        
        // Update all answer buttons to show correct/incorrect
        for (int i = 0; i < answerButtons.Length; i++)
        {
            GameObject button = answerButtons[i];
            Image bgImg = button.GetComponent<Image>();
            Transform iconTransform = button.transform.Find("Content/Icon");
            Transform letterTextTransform = button.transform.Find("Content/Letter/LetterText");
            Transform answerTextTransform = button.transform.Find("Content/AnswerText");
            
            if (i == currentQuestion.correctAnswer)
            {
                // Correct answer - green
                if (bgImg != null) bgImg.color = new Color(0.3f, 0.8f, 0.3f); // Green
                
                if (iconTransform != null)
                {
                    iconTransform.gameObject.SetActive(true);
                    TextMeshProUGUI iconText = iconTransform.GetComponent<TextMeshProUGUI>();
                    if (iconText != null)
                    {
                        iconText.text = "✓";
                        iconText.color = Color.white;
                    }
                }
                
                // White text
                if (letterTextTransform != null)
                {
                    letterTextTransform.GetComponent<TextMeshProUGUI>().color = Color.white;
                }
                if (answerTextTransform != null)
                {
                    answerTextTransform.GetComponent<TextMeshProUGUI>().color = Color.white;
                }
            }
            else if (i == selectedIndex)
            {
                // Selected wrong answer - red
                if (bgImg != null) bgImg.color = new Color(0.9f, 0.3f, 0.3f); // Red
                
                if (iconTransform != null)
                {
                    iconTransform.gameObject.SetActive(true);
                    TextMeshProUGUI iconText = iconTransform.GetComponent<TextMeshProUGUI>();
                    if (iconText != null)
                    {
                        iconText.text = "✗";
                        iconText.color = Color.white;
                    }
                }
                
                // White text
                if (letterTextTransform != null)
                {
                    letterTextTransform.GetComponent<TextMeshProUGUI>().color = Color.white;
                }
                if (answerTextTransform != null)
                {
                    answerTextTransform.GetComponent<TextMeshProUGUI>().color = Color.white;
                }
            }
        }
    }

    void OnQuizCompleted(int finalScore, int totalQuestions)
    {
        Debug.Log($"🎉 Quiz completed! Final score: {finalScore}");
    }

    void UpdateResultUI()
    {
        // Update icon
        Transform iconTransform = resultContainer.transform.Find("ResultIcon/IconText");
        if (iconTransform != null)
        {
            TextMeshProUGUI iconText = iconTransform.GetComponent<TextMeshProUGUI>();
            if (iconText != null)
            {
                iconText.text = quizManager.GetResultIcon();
            }
        }
        
        // Update message
        Transform msgTransform = resultContainer.transform.Find("ResultMessage/MessageText");
        if (msgTransform != null)
        {
            TextMeshProUGUI msgText = msgTransform.GetComponent<TextMeshProUGUI>();
            if (msgText != null)
            {
                msgText.text = quizManager.GetResultMessage();
            }
        }
        
        // Update score
        Transform scoreTransform = resultContainer.transform.Find("ScoreCard/ScoreRow/Score/ScoreText");
        if (scoreTransform != null)
        {
            TextMeshProUGUI scoreText = scoreTransform.GetComponent<TextMeshProUGUI>();
            if (scoreText != null)
            {
                scoreText.text = quizManager.GetScore().ToString();
            }
        }
        
        // Update percentage
        Transform percentTransform = resultContainer.transform.Find("ScoreCard/Percentage/PercentageText");
        if (percentTransform != null)
        {
            TextMeshProUGUI percentText = percentTransform.GetComponent<TextMeshProUGUI>();
            if (percentText != null)
            {
                percentText.text = $"{quizManager.GetScorePercentage()}%";
                percentText.color = quizManager.GetResultColor();
            }
        }
    }

    // ========== SPRITE HELPERS ==========
    
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
