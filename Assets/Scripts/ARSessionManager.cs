using UnityEngine;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// Quản lý AR Session cho standalone Unity app
/// GameObject này phải có tên "ARSessionManager" trong Unity scene
/// </summary>
public class ARSessionManager : MonoBehaviour
{
    [Header("AR Components")]
    [SerializeField] private ARSession arSession;
    [SerializeField] private ARCameraManager arCameraManager;
    
    private bool isARStarted = false;
    private float logTimer = 0f;
    private const float LOG_INTERVAL = 2f; // Log mỗi 2 giây

    void Start()
    {
        Debug.Log("🚀 ARSessionManager Started");
        
        // Tự động tìm AR components nếu chưa assign
        if (arSession == null)
        {
            arSession = FindObjectOfType<ARSession>();
            if (arSession != null)
            {
                Debug.Log("✅ Found ARSession component");
            }
            else
            {
                Debug.LogError("❌ ARSession component not found in scene!");
            }
        }
        
        if (arCameraManager == null)
        {
            arCameraManager = FindObjectOfType<ARCameraManager>();
            if (arCameraManager != null)
            {
                Debug.Log("✅ Found ARCameraManager component");
            }
        }
        
        // Tự động start AR session
        StartARSession();
    }

    void Update()
    {
        // Log trạng thái AR session định kỳ
        logTimer += Time.deltaTime;
        if (logTimer >= LOG_INTERVAL)
        {
            logTimer = 0f;
            LogARStatus();
        }
    }

    /// <summary>
    /// Start the AR Session
    /// </summary>
    public void StartARSession()
    {
        Debug.Log("📞 StartARSession called");

        if (arSession == null)
        {
            Debug.LogError("❌ Cannot start AR: ARSession is null");
            return;
        }

        // Enable AR Session
        arSession.enabled = true;
        isARStarted = true;

        Debug.Log("✅ AR Session enabled");
    }

    /// <summary>
    /// Log trạng thái AR session
    /// </summary>
    private void LogARStatus()
    {
        if (!isARStarted) return;

        string status = $"📊 AR Status:\n";
        status += $"  - Session State: {ARSession.state}\n";
        status += $"  - Session Enabled: {(arSession != null ? arSession.enabled : false)}\n";

        if (arCameraManager != null)
        {
            status += $"  - Camera Enabled: {arCameraManager.enabled}\n";
            status += $"  - Camera Descriptor Valid: {arCameraManager.descriptor != null}\n";
        }

        Debug.Log(status);
    }

    /// <summary>
    /// Pause AR session
    /// </summary>
    public void PauseARSession()
    {
        if (arSession != null)
        {
            arSession.enabled = false;
            Debug.Log("⏸️ AR Session paused");
        }
    }

    /// <summary>
    /// Resume AR session
    /// </summary>
    public void ResumeARSession()
    {
        if (arSession != null)
        {
            arSession.enabled = true;
            Debug.Log("▶️ AR Session resumed");
        }
    }

    void OnDestroy()
    {
        Debug.Log("🛑 ARSessionManager destroyed");
    }
}

