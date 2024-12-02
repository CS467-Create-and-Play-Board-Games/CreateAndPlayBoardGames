using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime.Controllers.System
{
    public class SceneController : MonoBehaviour
    {
        public void LoadScene(string sceneName)
        {
            LoadSceneSingle(sceneName);
        }

        public void LoadSceneSingle(string sceneName)
        {
            if (SceneManager.GetSceneByName(sceneName).isLoaded) return;
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            Debug.Log($"[SceneController] - Loading single scene: {sceneName}");
        }

        public void LoadSceneAdditive(string sceneName)
        {
            if (SceneManager.GetSceneByName(sceneName).isLoaded) return;
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            Debug.Log($"[SceneController] - Loading scene additively: {sceneName}");
        }

        public void UnloadScene(string sceneName)
        {
            if (!SceneManager.GetSceneByName(sceneName).isLoaded) return;
            SceneManager.UnloadSceneAsync(sceneName);
            Debug.Log($"[SceneController] - Unloading scene: {sceneName}");
        }

        public void SetActiveScene(string sceneName)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            Debug.Log($"[SceneController] - Scene {sceneName} set as active.");
        }

        public void Initialize()
        {
            // Additional initialization logic for SceneController
        }

        // New method to load and unload scenes in one operation
        public IEnumerator LoadAndUnloadScenes(string sceneToLoad, string sceneToUnload)
        {
            // Load the new scene additively
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);

            // Wait for the new scene to fully load
            while (!loadOperation.isDone)
            {
                yield return null;
            }

            Debug.Log($"[SceneController] - Scene {sceneToLoad} loaded.");

            // Disable the previous scene's root UI container if it's still active
            var oldScene = SceneManager.GetSceneByName(sceneToUnload);
            if (oldScene.isLoaded)
            {
                foreach (GameObject rootObject in oldScene.GetRootGameObjects())
                {
                    if (rootObject.CompareTag("UIRoot")) // Use a tag like "UIRoot" for UI containers
                    {
                        rootObject.SetActive(false);
                    }
                }
            }

            // Set the newly loaded scene as active
            SetActiveScene(sceneToLoad);

            // Unload the old scene (optional if you want to keep it in memory)
            UnloadScene(sceneToUnload);
        }

    }
}
