using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;

/// <summary>
/// Helper script to configure build settings for AR Kid integrated app (Quiz + Stories + AR)
/// Run from Unity Editor: Tools > AR Kid > Configure Build Settings
/// </summary>
public class BuildConfigurationHelper : MonoBehaviour
{
    [MenuItem("Tools/AR Kid/Configure Build Settings")]
    public static void ConfigureBuildSettings()
    {
        Debug.Log("🔧 Configuring Build Settings for AR Kid integrated app...");

        // Configure scenes in build
        ConfigureScenes();

        // Configure player settings
        ConfigurePlayerSettings();

        // Configure Android settings
        ConfigureAndroidSettings();

        Debug.Log("✅ Build configuration complete!");
        Debug.Log("📋 Next steps:");
        Debug.Log("   1. Verify XR Plugin Management is enabled (Edit > Project Settings > XR Plugin Management)");
        Debug.Log("   2. Ensure ARCore is enabled for Android");
        Debug.Log("   3. Build and test on device");
        Debug.Log("   4. App will launch with MainMenuScene (ar_kid home)");
    }

    static void ConfigureScenes()
    {
        Debug.Log("📋 Configuring scenes in Build Settings...");

        // Configure all 6 scenes for integrated ar_kid + AR Card Scanner app
        EditorBuildSettingsScene[] scenes = new EditorBuildSettingsScene[6];

        // ar_kid scenes (primary app features)
        scenes[0] = new EditorBuildSettingsScene("Assets/Scenes/MainMenuScene.unity", true);      // ar_kid home (primary entry)
        scenes[1] = new EditorBuildSettingsScene("Assets/Scenes/QuizScene.unity", true);          // Quiz feature
        scenes[2] = new EditorBuildSettingsScene("Assets/Scenes/StoryListScene.unity", true);     // Story browsing
        scenes[3] = new EditorBuildSettingsScene("Assets/Scenes/StoryDetailScene.unity", true);   // Story reading

        // AR Card Scanner scenes (secondary features)
        scenes[4] = new EditorBuildSettingsScene("Assets/Scenes/HomeScene.unity", true);          // AR landing page
        scenes[5] = new EditorBuildSettingsScene("Assets/Scenes/ARScene.unity", true);            // AR scanning

        // Set scenes in build settings
        EditorBuildSettings.scenes = scenes;

        Debug.Log("✅ Scenes configured:");
        Debug.Log("   [0] MainMenuScene (ar_kid home - PRIMARY ENTRY)");
        Debug.Log("   [1] QuizScene (Quiz feature)");
        Debug.Log("   [2] StoryListScene (Story browsing)");
        Debug.Log("   [3] StoryDetailScene (Story reading)");
        Debug.Log("   [4] HomeScene (AR Card Scanner landing)");
        Debug.Log("   [5] ARScene (AR scanning)");
    }

    static void ConfigurePlayerSettings()
    {
        Debug.Log("🎮 Configuring Player Settings...");

        // Set company name
        PlayerSettings.companyName = "AR Kid";

        // Set product name (integrated app)
        PlayerSettings.productName = "AR Kid - Quiz & Stories";

        // Set version
        PlayerSettings.bundleVersion = "2.0.0";

        // Set default orientation to Portrait
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;

        // Allow only portrait orientations
        PlayerSettings.allowedAutorotateToPortrait = true;
        PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
        PlayerSettings.allowedAutorotateToLandscapeLeft = false;
        PlayerSettings.allowedAutorotateToLandscapeRight = false;

        Debug.Log("✅ Player Settings configured");
        Debug.Log("   Product: AR Kid - Quiz & Stories");
        Debug.Log("   Version: 2.0.0 (integrated app)");
    }

    static void ConfigureAndroidSettings()
    {
        Debug.Log("🤖 Configuring Android Settings...");

        // Set package name (updated for integrated app)
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.arkid.quizstories");

        // Set minimum API level (Android 7.0 - API 24 for ARCore)
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel24;

        // Set target API level
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;

        // Set scripting backend to IL2CPP for better performance
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);

        // Set ARM64 architecture
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;

        Debug.Log("✅ Android Settings configured");
        Debug.Log("   Package: com.arkid.quizstories");
        Debug.Log("   Min SDK: Android 7.0 (API 24)");
        Debug.Log("   Target SDK: Auto");
        Debug.Log("   Backend: IL2CPP");
        Debug.Log("   Architecture: ARM64");
    }

    [MenuItem("Tools/AR Card Scanner/Verify Configuration")]
    public static void VerifyConfiguration()
    {
        Debug.Log("🔍 Verifying Build Configuration...");
        
        // Check scenes
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        Debug.Log($"📋 Scenes in build: {scenes.Length}");
        foreach (var scene in scenes)
        {
            Debug.Log($"   - {scene.path} (enabled: {scene.enabled})");
        }
        
        // Check player settings
        Debug.Log($"🎮 Product Name: {PlayerSettings.productName}");
        Debug.Log($"🎮 Bundle Version: {PlayerSettings.bundleVersion}");
        Debug.Log($"🎮 Orientation: {PlayerSettings.defaultInterfaceOrientation}");
        
        // Check Android settings
        Debug.Log($"🤖 Android Package: {PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android)}");
        Debug.Log($"🤖 Min SDK: {PlayerSettings.Android.minSdkVersion}");
        Debug.Log($"🤖 Target SDK: {PlayerSettings.Android.targetSdkVersion}");
        Debug.Log($"🤖 Scripting Backend: {PlayerSettings.GetScriptingBackend(BuildTargetGroup.Android)}");
        
        Debug.Log("✅ Verification complete!");
    }

    [MenuItem("Tools/AR Card Scanner/Build Android APK")]
    public static void BuildAndroid()
    {
        Debug.Log("🏗️ Building Android APK...");

        // Configure first
        ConfigureBuildSettings();

        // Build options
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] {
            "Assets/Scenes/MainMenuScene.unity",      // ar_kid home (primary entry)
            "Assets/Scenes/QuizScene.unity",          // Quiz feature
            "Assets/Scenes/StoryListScene.unity",     // Story browsing
            "Assets/Scenes/StoryDetailScene.unity",   // Story reading
            "Assets/Scenes/HomeScene.unity",          // AR landing page
            "Assets/Scenes/ARScene.unity"             // AR scanning
        };
        buildPlayerOptions.locationPathName = "Builds/ARKidIntegrated.apk";
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = BuildOptions.None;

        // Build
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log($"✅ Build succeeded: {summary.totalSize} bytes");
            Debug.Log($"📦 APK location: {buildPlayerOptions.locationPathName}");
        }
        else
        {
            Debug.LogError($"❌ Build failed: {summary.result}");
        }
    }
}