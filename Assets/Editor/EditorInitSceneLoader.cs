#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    /// <summary>
    /// This script is used to load a specific scene additively when the game is played in the editor.
    /// It is a modified version of the scripts provided in this resource:
    /// https://stackoverflow.com/questions/35586103/unity3d-load-a-specific-scene-on-play-mode
    ///
    /// Final builds will not be affected by this script.
    ///
    /// This script is only intended for use in the Unity Editor, and it ensures that
    /// the Init scene, which contains the ApplicationInitializer is loaded additively
    /// when the game is played in the editor.
    /// </summary>
    [InitializeOnLoad]
    public class EditorInitSceneLoader
    {
        private const string InitSceneName = "Init";

        static EditorInitSceneLoader()
        {
            // Subscribe to the PlayModeStateChanged event to trigger when Play mode starts.
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        /// <summary>
        /// Handles the PlayModeStateChanged event to load the Init scene additively when entering Play mode.
        /// </summary>
        /// <param name="state">The current play mode state.</param>
        static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            // When entering Play mode, load the active scene and load the Init scene additively.
            if (state != PlayModeStateChange.EnteredPlayMode) return;

            // Ensure the currently active scene is the one being used in Play mode.
            Scene activeScene = SceneManager.GetActiveScene();
            Debug.Log($"[EditorInit] - Active scene: {activeScene.name}");

            if (activeScene.name == InitSceneName)
            {
                Debug.Log(
                    $"[EditorInit] - The Init scene is already active. No need to load it additively.");
                return;
            }

            // Ensure the Init scene is loaded additively if it's not already loaded
            Scene initScene = SceneManager.GetSceneByName(InitSceneName);
            if (initScene.isLoaded) return;

            Debug.Log($"[EditorInit] - Loading Init scene additively.");
            SceneManager.LoadScene(InitSceneName, LoadSceneMode.Additive);
        }
    }
}
#endif
