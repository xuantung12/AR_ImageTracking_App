using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages the Home Screen UI
/// Builds UI programmatically to match Flutter design
/// </summary>
public class HomeUIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private Button startButton;
    
    [Header("Colors")]
    [SerializeField] private Color topGradientColor = new Color(0.4f, 0.6f, 1f); // Blue
    [SerializeField] private Color bottomGradientColor = new Color(0.6f, 0.2f, 0.8f); // Purple

    void Start()
    {
        Debug.Log("🏠 HomeUIManager Started");
        
        // If canvas not assigned, find it
        if (canvas == null)
        {
            canvas = FindObjectOfType<Canvas>();
        }
        
        // Build UI if not already built
        if (startButton == null)
        {
            BuildHomeUI();
        }
    }

    /// <summary>
    /// Build the complete Home UI
    /// </summary>
    void BuildHomeUI()
    {
        Debug.Log("Building Home UI...");
        
        // This method will be called to set up UI elements
        // For now, we'll create a simple structure
        // The actual UI will be built in the Unity Editor for better control
    }

    /// <summary>
    /// Called when Start button is clicked
    /// </summary>
    public void OnStartButtonClicked()
    {
        Debug.Log("Start button clicked - Loading AR Scene");
        SceneTransitionManager.LoadAR();
    }

    /// <summary>
    /// Quit application
    /// </summary>
    public void OnQuitButtonClicked()
    {
        Debug.Log("Quit button clicked");
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}