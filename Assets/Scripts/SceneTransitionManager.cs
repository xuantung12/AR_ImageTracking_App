using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages scene transitions for the integrated AR Kid + AR Card Scanner app
/// Updated to support ar_kid features (Quiz, Stories) + AR features
/// </summary>
public class SceneTransitionManager : MonoBehaviour
{
    // ========== SCENE NAMES ==========

    // ar_kid scenes (primary app features)
    public const string MAIN_MENU_SCENE = "MainMenuScene";  // ar_kid HomePage (primary entry point)
    public const string QUIZ_SCENE = "QuizScene";           // Quiz feature
    public const string STORY_LIST_SCENE = "StoryListScene"; // Story browsing
    public const string STORY_DETAIL_SCENE = "StoryDetailScene"; // Story reading

    // AR Card Scanner scenes (secondary features)
    public const string AR_HOME_SCENE = "HomeScene";        // AR Card Scanner landing page
    public const string AR_SCENE = "ARScene";               // AR card scanning

    // Legacy constants (for backward compatibility)
    public const string HOME_SCENE = AR_HOME_SCENE;         // Deprecated: Use AR_HOME_SCENE

    // ========== INSTANCE METHODS ==========

    /// <summary>
    /// Load the Main Menu (ar_kid HomePage) - Primary home screen
    /// </summary>
    public void LoadMainMenuScene()
    {
        Debug.Log("🔄 Loading Main Menu (ar_kid HomePage)...");
        SceneManager.LoadScene(MAIN_MENU_SCENE);
    }

    /// <summary>
    /// Load the Quiz Scene
    /// </summary>
    public void LoadQuizScene()
    {
        Debug.Log("🔄 Loading Quiz Scene...");
        SceneManager.LoadScene(QUIZ_SCENE);
    }

    /// <summary>
    /// Load the Story List Scene
    /// </summary>
    public void LoadStoryListScene()
    {
        Debug.Log("🔄 Loading Story List Scene...");
        SceneManager.LoadScene(STORY_LIST_SCENE);
    }

    /// <summary>
    /// Load the Story Detail Scene
    /// </summary>
    public void LoadStoryDetailScene()
    {
        Debug.Log("🔄 Loading Story Detail Scene...");
        SceneManager.LoadScene(STORY_DETAIL_SCENE);
    }

    /// <summary>
    /// Load the AR Home Scene (AR Card Scanner landing page)
    /// </summary>
    public void LoadARHomeScene()
    {
        Debug.Log("🔄 Loading AR Home Scene (AR Card Scanner landing)...");
        SceneManager.LoadScene(AR_HOME_SCENE);
    }

    /// <summary>
    /// Load the AR Scene (AR card scanning)
    /// </summary>
    public void LoadARScene()
    {
        Debug.Log("🔄 Loading AR Scene (card scanning)...");
        SceneManager.LoadScene(AR_SCENE);
    }

    /// <summary>
    /// [DEPRECATED] Load the Home Scene - Use LoadMainMenuScene() or LoadARHomeScene()
    /// </summary>
    public void LoadHomeScene()
    {
        Debug.LogWarning("⚠️ LoadHomeScene() is deprecated. Use LoadMainMenuScene() for ar_kid home or LoadARHomeScene() for AR landing page.");
        LoadMainMenuScene();
    }

    /// <summary>
    /// Quit the application
    /// </summary>
    public void QuitApplication()
    {
        Debug.Log("Quitting application...");
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    // ========== STATIC METHODS (for calling from anywhere) ==========

    /// <summary>
    /// Load the Main Menu (ar_kid HomePage) - Primary home screen
    /// </summary>
    public static void LoadMainMenu()
    {
        Debug.Log("🔄 Loading Main Menu (ar_kid HomePage)...");
        SceneManager.LoadScene(MAIN_MENU_SCENE);
    }

    /// <summary>
    /// Load the Quiz Scene
    /// </summary>
    public static void LoadQuiz()
    {
        Debug.Log("🔄 Loading Quiz Scene...");
        SceneManager.LoadScene(QUIZ_SCENE);
    }

    /// <summary>
    /// Load the Story List Scene
    /// </summary>
    public static void LoadStoryList()
    {
        Debug.Log("🔄 Loading Story List Scene...");
        SceneManager.LoadScene(STORY_LIST_SCENE);
    }

    /// <summary>
    /// Load the Story Detail Scene
    /// </summary>
    public static void LoadStoryDetail()
    {
        Debug.Log("🔄 Loading Story Detail Scene...");
        SceneManager.LoadScene(STORY_DETAIL_SCENE);
    }

    /// <summary>
    /// Load the AR Home Scene (AR Card Scanner landing page)
    /// </summary>
    public static void LoadARHome()
    {
        Debug.Log("🔄 Loading AR Home Scene (AR Card Scanner landing)...");
        SceneManager.LoadScene(AR_HOME_SCENE);
    }

    /// <summary>
    /// Load the AR Scene (AR card scanning)
    /// </summary>
    public static void LoadAR()
    {
        Debug.Log("🔄 Loading AR Scene (card scanning)...");
        SceneManager.LoadScene(AR_SCENE);
    }

    /// <summary>
    /// [DEPRECATED] Load the Home Scene - Use LoadMainMenu() or LoadARHome()
    /// This now loads MainMenuScene (ar_kid home) instead of AR HomeScene
    /// </summary>
    public static void LoadHome()
    {
        Debug.LogWarning("⚠️ LoadHome() is deprecated. Use LoadMainMenu() for ar_kid home or LoadARHome() for AR landing page.");
        LoadMainMenu();
    }
}