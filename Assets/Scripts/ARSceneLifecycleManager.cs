using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages AR Scene lifecycle to prevent camera destruction issues
/// Properly cleans up AR components when leaving AR scene
/// </summary>
public class ARSceneLifecycleManager : MonoBehaviour
{
    [Header("AR Components")]
    [SerializeField] private ARSession arSession;
    [SerializeField] private ARCameraManager arCameraManager;
    [SerializeField] private GameObject xrOrigin;
    
    [Header("Debug")]
    [SerializeField] private bool showDebugLogs = true;
    
    private static bool isARSceneActive = false;
    
    void Awake()
    {
        if (showDebugLogs)
        {
            Debug.Log("🎬 ARSceneLifecycleManager Awake");
        }
        
        // Find AR components if not assigned
        FindARComponents();
        
        // Mark AR scene as active
        isARSceneActive = true;
    }
    
    void Start()
    {
        if (showDebugLogs)
        {
            Debug.Log("🚀 ARSceneLifecycleManager Started");
            LogARComponents();
        }
        
        // Subscribe to scene unload event
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
    
    void FindARComponents()
    {
        // Find AR Session
        if (arSession == null)
        {
            arSession = FindObjectOfType<ARSession>();
            if (arSession != null && showDebugLogs)
            {
                Debug.Log($"✅ Found ARSession: {arSession.gameObject.name}");
            }
        }
        
        // Find AR Camera Manager
        if (arCameraManager == null)
        {
            arCameraManager = FindObjectOfType<ARCameraManager>();
            if (arCameraManager != null && showDebugLogs)
            {
                Debug.Log($"✅ Found ARCameraManager: {arCameraManager.gameObject.name}");
            }
        }
        
        // Find XR Origin
        if (xrOrigin == null)
        {
            // Try to find by name
            GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (GameObject obj in rootObjects)
            {
                if (obj.name.Contains("XR Origin") || obj.name.Contains("AR Session Origin"))
                {
                    xrOrigin = obj;
                    if (showDebugLogs)
                    {
                        Debug.Log($"✅ Found XR Origin: {xrOrigin.name}");
                    }
                    break;
                }
            }
        }
    }
    
    void LogARComponents()
    {
        Debug.Log("📋 AR Components Status:");
        Debug.Log($"  - ARSession: {(arSession != null ? arSession.gameObject.name : "NULL")}");
        Debug.Log($"  - ARCameraManager: {(arCameraManager != null ? arCameraManager.gameObject.name : "NULL")}");
        Debug.Log($"  - XR Origin: {(xrOrigin != null ? xrOrigin.name : "NULL")}");
        
        if (arCameraManager != null)
        {
            Camera cam = arCameraManager.GetComponent<Camera>();
            if (cam != null)
            {
                Debug.Log($"  - Camera: {cam.name} (Enabled: {cam.enabled})");
            }
        }
    }
    
    void OnSceneUnloaded(Scene scene)
    {
        // Only cleanup if this is the AR scene being unloaded
        if (scene.name == "ARScene" && isARSceneActive)
        {
            if (showDebugLogs)
            {
                Debug.Log($"🧹 ARScene unloaded, cleaning up AR components...");
            }
            
            CleanupARComponents();
            isARSceneActive = false;
        }
    }
    
    void CleanupARComponents()
    {
        if (showDebugLogs)
        {
            Debug.Log("🧹 Cleaning up AR components...");
        }
        
        // Disable AR Session first
        if (arSession != null)
        {
            arSession.enabled = false;
            if (showDebugLogs)
            {
                Debug.Log("  ✅ ARSession disabled");
            }
        }
        
        // Disable AR Camera Manager
        if (arCameraManager != null)
        {
            arCameraManager.enabled = false;
            if (showDebugLogs)
            {
                Debug.Log("  ✅ ARCameraManager disabled");
            }
        }
        
        // Note: We don't destroy objects here because Unity will handle that
        // when the scene is unloaded. We just disable them to prevent issues.
        
        if (showDebugLogs)
        {
            Debug.Log("✅ AR cleanup complete");
        }
    }
    
    void OnDestroy()
    {
        // Unsubscribe from events
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        
        if (showDebugLogs)
        {
            Debug.Log("🛑 ARSceneLifecycleManager destroyed");
        }
        
        // Final cleanup
        if (isARSceneActive)
        {
            CleanupARComponents();
            isARSceneActive = false;
        }
    }
    
    void OnApplicationPause(bool pauseStatus)
    {
        // Handle app pause/resume
        if (pauseStatus)
        {
            // App is pausing
            if (arSession != null && arSession.enabled)
            {
                arSession.enabled = false;
                if (showDebugLogs)
                {
                    Debug.Log("⏸️ AR Session paused (app pause)");
                }
            }
        }
        else
        {
            // App is resuming
            if (arSession != null && isARSceneActive)
            {
                arSession.enabled = true;
                if (showDebugLogs)
                {
                    Debug.Log("▶️ AR Session resumed (app resume)");
                }
            }
        }
    }
    
    /// <summary>
    /// Public method to manually cleanup AR (can be called before scene transition)
    /// </summary>
    public void ManualCleanup()
    {
        if (showDebugLogs)
        {
            Debug.Log("🧹 Manual AR cleanup requested");
        }
        CleanupARComponents();
    }
}

