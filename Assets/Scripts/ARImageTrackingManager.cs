using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ARImageTrackingManager : MonoBehaviour
{
    [Header("AR Components")]
    [SerializeField] private ARTrackedImageManager trackedImageManager;

    [Header("Video Prefabs - Phải đúng thứ tự với Image Library")]
    [SerializeField] private GameObject[] videoPrefabs = new GameObject[10];

    [Header("UI Elements")]
    [SerializeField] private GameObject scanFrame;
    [SerializeField] private TextMeshProUGUI instructionText;
    [SerializeField] private Button backButton;
    [SerializeField] private Button exitButton;

    private Dictionary<string, GameObject> spawnedVideos = new Dictionary<string, GameObject>();
    private GameObject currentActiveVideo;
    private bool isPlayingVideo = false;

    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;

        // Setup buttons
        backButton.onClick.AddListener(OnBackButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);

        // Ẩn nút Back ban đầu
        backButton.gameObject.SetActive(false);
    }

    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        backButton.onClick.RemoveListener(OnBackButtonClicked);
        exitButton.onClick.RemoveListener(OnExitButtonClicked);
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // Xử lý ảnh mới được track
        foreach (var trackedImage in eventArgs.added)
        {
            UpdateTrackedImage(trackedImage);
        }

        // Xử lý ảnh đang được track (cập nhật vị trí)
        foreach (var trackedImage in eventArgs.updated)
        {
            UpdateTrackedImage(trackedImage);
        }

        // Xử lý ảnh bị mất track
        foreach (var trackedImage in eventArgs.removed)
        {
            if (spawnedVideos.ContainsKey(trackedImage.referenceImage.name))
            {
                spawnedVideos[trackedImage.referenceImage.name].SetActive(false);
            }
        }
    }

    void UpdateTrackedImage(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;

        // Nếu đang phát video, không track ảnh mới
        if (isPlayingVideo && currentActiveVideo != null)
        {
            return;
        }

        // Kiểm tra xem ảnh có đang được track tốt không
        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            // Nếu chưa tạo video cho ảnh này
            if (!spawnedVideos.ContainsKey(imageName))
            {
                int imageIndex = GetImageIndex(imageName);
                if (imageIndex >= 0 && imageIndex < videoPrefabs.Length && videoPrefabs[imageIndex] != null)
                {
                    GameObject videoObj = Instantiate(videoPrefabs[imageIndex], trackedImage.transform);
                    videoObj.transform.localPosition = Vector3.zero;
                    videoObj.transform.localRotation = Quaternion.identity;
                    spawnedVideos[imageName] = videoObj;

                    // Phát video
                    PlayVideo(videoObj);
                }
            }
            else
            {
                // Cập nhật vị trí và hiển thị
                GameObject videoObj = spawnedVideos[imageName];
                videoObj.SetActive(true);
                videoObj.transform.position = trackedImage.transform.position;
                videoObj.transform.rotation = trackedImage.transform.rotation;

                // Phát video nếu chưa phát
                PlayVideo(videoObj);
            }
        }
        else
        {
            // Ẩn video nếu tracking kém
            if (spawnedVideos.ContainsKey(imageName))
            {
                spawnedVideos[imageName].SetActive(false);
            }
        }
    }

    void PlayVideo(GameObject videoObj)
    {
        if (isPlayingVideo) return;

        VideoPlayer videoPlayer = videoObj.GetComponentInChildren<VideoPlayer>();
        if (videoPlayer != null)
        {
            if (!videoPlayer.isPlaying)
            {
                videoPlayer.Play();
                isPlayingVideo = true;
                currentActiveVideo = videoObj;

                // Ẩn UI quét, hiện nút Back
                scanFrame.SetActive(false);
                instructionText.gameObject.SetActive(false);
                backButton.gameObject.SetActive(true);
            }
        }
    }

    int GetImageIndex(string imageName)
    {
        // Parse tên ảnh để lấy index
        // Ví dụ: "Image_01" -> return 0, "Image_02" -> return 1
        if (imageName.Contains("_"))
        {
            string[] parts = imageName.Split('_');
            if (parts.Length > 1)
            {
                if (int.TryParse(parts[1], out int index))
                {
                    return index - 1; // Trả về index từ 0
                }
            }
        }
        return -1;
    }

    // void OnBackButtonClicked()
    // {
    //     if (currentActiveVideo != null)
    //     {
    //         VideoPlayer videoPlayer = currentActiveVideo.GetComponentInChildren<VideoPlayer>();
    //         if (videoPlayer != null)
    //         {
    //             videoPlayer.Stop();
    //         }
    //         currentActiveVideo.SetActive(false);
    //         currentActiveVideo = null;
    //     }

    //     isPlayingVideo = false;

    //     scanFrame.SetActive(true);
    //     instructionText.gameObject.SetActive(true);
    //     backButton.gameObject.SetActive(false);
    // }


    /// <summary>
    /// Called when back button is clicked - Returns to AR Home Scene
    /// </summary>
    public void OnBackButtonClicked()
    {
        // Dừng video hiện tại
        if (currentActiveVideo != null)
        {
            VideoPlayer videoPlayer = currentActiveVideo.GetComponentInChildren<VideoPlayer>();
            if (videoPlayer != null)
            {
                videoPlayer.Stop();
            }
            currentActiveVideo.SetActive(false);
            currentActiveVideo = null;
        }

        isPlayingVideo = false;


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