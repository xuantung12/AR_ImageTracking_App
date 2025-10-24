using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ImageTrackingManager : MonoBehaviour
{
    [Header("AR Components")]
    [SerializeField] private ARTrackedImageManager trackedImageManager;
    [SerializeField] private ARSession arSession;  // ✅ Thêm reference đến AR Session

    [Header("Video Prefabs")]
    [SerializeField] private GameObject[] videoPrefabs; // 3 prefabs tương ứng card_1, 2, 3

    [Header("UI Elements")]
    [SerializeField] private GameObject scanFrame;
    [SerializeField] private GameObject instructionText;

    private Dictionary<string, GameObject> spawnedVideos = new Dictionary<string, GameObject>();
    private Dictionary<string, bool> trackedStates = new Dictionary<string, bool>();
    private float logTimer = 0f;
 
    void Start()
    {
        Debug.Log("🚀 ImageTrackingManager Started");

        // Tự động tìm AR Session nếu chưa assign
        if (arSession == null)
        {
            arSession = FindObjectOfType<ARSession>();
            if (arSession != null)
            {
                Debug.Log("✅ Found ARSession component");
                arSession.enabled = true;  // ✅ Đảm bảo AR Session được bật
                Debug.Log("✅ AR Session enabled");
            }
            else
            {
                Debug.LogError("❌ ARSession not found in scene!");
            }
        }
        else
        {
            arSession.enabled = true;
            Debug.Log("✅ AR Session enabled from assigned reference");
        }

        Debug.Log("📤 Scene loaded");
    }

    void OnEnable()
    {
        if (trackedImageManager != null)
        {
            trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        }
    }

    void OnDisable()
    {
        if (trackedImageManager != null)
        {
            trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // Thêm ảnh mới được phát hiện
        foreach (var trackedImage in eventArgs.added)
        {
            UpdateTrackedImage(trackedImage);
        }

        // Cập nhật ảnh đang tracking
        foreach (var trackedImage in eventArgs.updated)
        {
            UpdateTrackedImage(trackedImage);
        }

        // Xóa ảnh không còn tracking
        foreach (var trackedImage in eventArgs.removed)
        {
            string imageName = trackedImage.referenceImage.name;
            if (spawnedVideos.ContainsKey(imageName))
            {
                Destroy(spawnedVideos[imageName]);
                spawnedVideos.Remove(imageName);
                trackedStates[imageName] = false;
                Debug.Log($"Card lost: {imageName}");
            }
        }
    }

    void UpdateTrackedImage(ARTrackedImage trackedImage)
    {
        // Null check để tránh lỗi trong XR Simulation
        if (trackedImage == null || trackedImage.referenceImage == null)
        {
            return;
        }

        string imageName = trackedImage.referenceImage.name;

        // Check nếu imageName null hoặc empty
        if (string.IsNullOrEmpty(imageName))
        {
            return;
        }

        // Tạo video nếu chưa có
        if (!spawnedVideos.ContainsKey(imageName))
        {
            GameObject prefab = GetPrefabForImage(imageName);
            if (prefab != null)
            {
                GameObject videoObject = Instantiate(prefab, trackedImage.transform);
                spawnedVideos[imageName] = videoObject;
                trackedStates[imageName] = true;

                // Hide UI when card detected
                HideUI();
                Debug.Log($"Card detected: {imageName}");
            }
        }

        // Cập nhật vị trí và hiển thị
        if (spawnedVideos.ContainsKey(imageName))
        {
            GameObject videoObject = spawnedVideos[imageName];
            videoObject.transform.position = trackedImage.transform.position;
            videoObject.transform.rotation = trackedImage.transform.rotation;

            bool isTracking = trackedImage.trackingState == TrackingState.Tracking;
            videoObject.SetActive(isTracking);

            // Update tracking state
            if (trackedStates.ContainsKey(imageName))
            {
                if (isTracking && !trackedStates[imageName])
                {
                    trackedStates[imageName] = true;
                    Debug.Log($"Card detected: {imageName}");
                }
                else if (!isTracking && trackedStates[imageName])
                {
                    trackedStates[imageName] = false;
                    Debug.Log($"Card lost: {imageName}");
                }
            }
        }
    }

    GameObject GetPrefabForImage(string imageName)
    {
        switch (imageName)
        {
            case "card_1": return videoPrefabs[0];
            case "card_2": return videoPrefabs[1];
            case "card_3": return videoPrefabs[2];
            default: return null;
        }
    }

    void HideUI()
    {
        if (scanFrame != null) scanFrame.SetActive(false);
        if (instructionText != null) instructionText.SetActive(false);
    }

    void ShowUI()
    {
        if (scanFrame != null) scanFrame.SetActive(true);
        if (instructionText != null) instructionText.SetActive(true);
    }

    void Update()
    {
        // Log AR Session state mỗi 2 giây
        logTimer += Time.deltaTime;
        if (logTimer >= 2f)
        {
            logTimer = 0f;
            if (arSession != null)
            {
                Debug.Log($"📊 AR Session State: {ARSession.state}, Enabled: {arSession.enabled}");
            }
        }

        // Show UI if no cards are being tracked
        bool anyTracking = false;
        foreach (var kvp in spawnedVideos)
        {
            if (kvp.Value != null && kvp.Value.activeSelf)
            {
                anyTracking = true;
                break;
            }
        }

        if (!anyTracking && spawnedVideos.Count > 0)
        {
            ShowUI();
        }
    }
}