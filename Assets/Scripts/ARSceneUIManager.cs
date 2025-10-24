using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages UI elements in the AR Scene
/// Creates a back button if one doesn't exist and handles navigation
/// </summary>
public class ARSceneUIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button backButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Canvas canvas;

    void Start()
    {
        Debug.Log("🎮 ARSceneUIManager Started");

        // Find canvas if not assigned
        if (canvas == null)
        {
            canvas = FindObjectOfType<Canvas>();
        }

        // Find and configure back button
        if (backButton == null)
        {
            FindAndConfigureBackButton();
        }
        else
        {
            ConfigureBackButton();
        }

        // Find and configure exit button
        if (exitButton == null)
        {
            FindAndConfigureExitButton();
        }
        else
        {
            ConfigureExitButton();
        }
    }

    void FindAndConfigureBackButton()
    {
        // First, try to find existing button
        Button[] buttons = FindObjectsOfType<Button>();

        foreach (Button btn in buttons)
        {
            // Look for any button that might be the back button
            if (btn != null &&
                (btn.name.ToLower().Contains("back") ||
                 btn.GetComponentInChildren<TextMeshProUGUI>()?.text.Contains("←") == true ||
                 btn.GetComponentInChildren<TextMeshProUGUI>()?.text.Contains("Quay lại") == true))
            {
                backButton = btn;
                ConfigureBackButton();
                Debug.Log("✅ Found existing back button: " + btn.name);
                return;
            }
        }

        // If no back button found, create one
        Debug.Log("⚠️ Back button not found - creating new one");
        CreateBackButton();
    }

    void CreateBackButton()
    {
        if (canvas == null)
        {
            Debug.LogError("❌ Cannot create back button - Canvas not found");
            return;
        }

        // Create back button container
        GameObject buttonContainer = new GameObject("BackButtonContainer");
        buttonContainer.transform.SetParent(canvas.transform, false);

        RectTransform containerRt = buttonContainer.AddComponent<RectTransform>();
        containerRt.anchorMin = new Vector2(0, 1);
        containerRt.anchorMax = new Vector2(0, 1);
        containerRt.pivot = new Vector2(0, 1);
        containerRt.anchoredPosition = new Vector2(32, -32);
        containerRt.sizeDelta = new Vector2(100, 100);

        // Add Image component for background
        Image bgImage = buttonContainer.AddComponent<Image>();
        bgImage.color = new Color(0, 0, 0, 0.5f); // Semi-transparent black
        bgImage.raycastTarget = true; // Enable raycast for clicking

        // Add Button component
        backButton = buttonContainer.AddComponent<Button>();

        // Set button target graphic
        backButton.targetGraphic = bgImage;

        // Create button text
        GameObject buttonText = new GameObject("ButtonText");
        buttonText.transform.SetParent(buttonContainer.transform, false);

        RectTransform textRt = buttonText.AddComponent<RectTransform>();
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.sizeDelta = Vector2.zero;

        TextMeshProUGUI tmp = buttonText.AddComponent<TextMeshProUGUI>();
        tmp.text = "←";
        tmp.fontSize = 48;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.Center;

        ConfigureBackButton();
        Debug.Log("✅ Created new back button");
    }

    void ConfigureBackButton()
    {
        if (backButton != null)
        {
            // Clear existing listeners
            backButton.onClick.RemoveAllListeners();

            // Add new listener to go back to Home scene
            backButton.onClick.AddListener(OnBackButtonClicked);

            Debug.Log("✅ Back button configured to return to Home scene");
        }
    }

    /// <summary>
    /// Find and configure exit button
    /// </summary>
    void FindAndConfigureExitButton()
    {
        // Try to find existing exit button
        Button[] buttons = FindObjectsOfType<Button>();

        foreach (Button btn in buttons)
        {
            if (btn != null &&
                (btn.name.ToLower().Contains("exit") ||
                 btn.name.ToLower().Contains("thoat") ||
                 btn.GetComponentInChildren<TextMeshProUGUI>()?.text.Contains("Thoát") == true ||
                 btn.GetComponentInChildren<TextMeshProUGUI>()?.text.Contains("X") == true))
            {
                exitButton = btn;
                ConfigureExitButton();
                Debug.Log("✅ Found existing exit button: " + btn.name);
                return;
            }
        }

        Debug.Log("⚠️ Exit button not found in scene - This is OK, back button will still work");
    }

    /// <summary>
    /// Configure exit button to return to HomeScene
    /// </summary>
    void ConfigureExitButton()
    {
        if (exitButton != null)
        {
            // Clear existing listeners
            exitButton.onClick.RemoveAllListeners();

            // Add listener to go back to HomeScene
            exitButton.onClick.AddListener(OnExitButtonClicked);

            Debug.Log("✅ Exit button configured to return to HomeScene");
        }
    }

    /// <summary>
    /// Called when back button is clicked - Returns to AR Home Scene
    /// </summary>
    public void OnBackButtonClicked()
    {
        Debug.Log("Back button clicked - Returning to AR Home Scene");
        SceneTransitionManager.LoadARHome();
    }

    /// <summary>
    /// Called when exit button is clicked - Returns to HomeScene
    /// </summary>
    public void OnExitButtonClicked()
    {
        Debug.Log("Exit button clicked - Returning to HomeScene");
        SceneTransitionManager.LoadARHome();
    }
}