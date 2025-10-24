using UnityEngine;

/// <summary>
/// Singleton manager to pass selected story data between scenes
/// Used for StoryListScene → StoryDetailScene navigation
/// </summary>
public class StoryManager : MonoBehaviour
{
    private static StoryManager _instance;
    
    public static StoryManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("StoryManager");
                _instance = go.AddComponent<StoryManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    // Currently selected story
    private Story _selectedStory;
    
    public Story SelectedStory
    {
        get { return _selectedStory; }
        set { _selectedStory = value; }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Set the selected story and navigate to detail scene
    /// </summary>
    public void ViewStoryDetail(Story story)
    {
        _selectedStory = story;
        Debug.Log($"📖 Viewing story: {story.title}");
        SceneTransitionManager.LoadStoryDetail();
    }

    /// <summary>
    /// Clear selected story (optional cleanup)
    /// </summary>
    public void ClearSelection()
    {
        _selectedStory = null;
    }
}
