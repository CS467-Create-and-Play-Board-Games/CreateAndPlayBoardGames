using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime.Initializers
{
    /// <summary>
    /// Initializes the application by ensuring a single instance of the initializer,
    /// logging the current scene, and managing the loading and unloading of scenes.
    /// </summary>
    public class ApplicationInitializer : MonoBehaviour
    {
        // Singleton instance
        private static ApplicationInitializer Instance { get; set; }

        private Scene CurrentScene { get; set; }

        /// <summary>
        /// Called when the script instance is being loaded.
        /// Ensures a single instance, logs the current scene, and initializes scenes.
        /// </summary>
        private void Awake()
        {
            EnsureSingleInstance();
            LogCurrentScene();

            // Perform setup and initialization tasks here.
            InitializeApplicationManager();

            // Use the ApplicationManager to initialize to load the main game scene.
            // ...
            if (CurrentScene.name == "Init")
            {
                StartCoroutine(LoadMainMenuSceneCoroutine());
            }

            StartCoroutine(UnloadInitSceneCoroutine());
        }

        /// <summary>
        /// Called when the script instance is being destroyed.
        /// Cleans up references if necessary.
        /// </summary>
        private void OnDestroy()
        {
            Debug.Log("[ApplicationInitializer] - Cleaning up references.");
            if (Instance == this)
            {
                Instance = null;
            }

            Destroy(gameObject);
        }

        /// <summary>
        /// Ensures that only one instance of the ApplicationInitializer exists.
        /// Destroys duplicate instances.
        /// </summary>
        private void EnsureSingleInstance()
        {
            if (Instance == null)
            {
                Instance = this;
                // DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Logs the name of the current active scene and provides context-specific messages.
        /// </summary>
        private void LogCurrentScene()
        {
            CurrentScene = SceneManager.GetActiveScene();

            switch (CurrentScene.name)
            {
                case "Init" or "ApplicationInitializer":
                    Debug.Log(
                        "[ApplicationInitializer] - Starting from the Init scene.");
                    Debug.Log(
                        "[ApplicationInitializer] - This means the game is starting.");
                    break;
                case "ApplicationManager":
                    Debug.LogWarning(
                        "[ApplicationInitializer] - [WARNING]: Starting from the ApplicationManager scene.");
                    Debug.LogWarning(
                        "[ApplicationInitializer] - [WARNING]: This is not recommended and may create duplicate game objects.");
                    Debug.LogWarning(
                        "[ApplicationInitializer] - [WARNING]: The ApplicationInitializer already loads the ApplicationManager scene additively.");
                    break;
                default:
                    Debug.Log(
                        "[ApplicationInitializer] - Starting from current scene: " +
                        CurrentScene.name);
                    break;
            }
        }

        private void InitializeApplicationManager()
        {
            // Load the ApplicationManager scene additively.
            // This scene should contain the ApplicationManager game object and other objects
            // for setting it up.
            LoadApplicationManagerScene();

            // The ApplicationManager game object should become persistent across scenes after
            // the ApplicationManager scene is loaded additively,
            // so the ApplicationManager Scene itself should be unloaded to remove other objects.
            UnloadApplicationManagerScene();
        }

        /// <summary>
        /// Loads the ApplicationManager scene additively.
        /// </summary>
        private void LoadApplicationManagerScene()
        {
            SceneManager.LoadScene("ApplicationManager",
                LoadSceneMode.Additive);
            Debug.Log(
                "[ApplicationInitializer] - [LOADING]: ApplicationManager scene loaded additively.");
        }

        /// <summary>
        /// Initiates the unloading of the ApplicationManager scene.
        /// </summary>
        private void UnloadApplicationManagerScene()
        {
            Debug.Log(
                "[ApplicationInitializer] - [UNLOADING]: Called UnloadApplicationManagerScene() to unload ApplicationManager scene.");
            StartCoroutine(UnloadApplicationManagerSceneCoroutine());
        }

        /// <summary>
        /// Coroutine to unload the ApplicationManager scene once it is fully loaded.
        /// </summary>
        private IEnumerator UnloadApplicationManagerSceneCoroutine()
        {
            while (!SceneManager.GetSceneByName("ApplicationManager").isLoaded)
            {
                yield return null;
            }

            Debug.Log(
                "[ApplicationInitializer] - [UNLOADING]: Unloading ApplicationManager scene.");
            SceneManager.UnloadSceneAsync("ApplicationManager");
            Debug.Log(
                "[ApplicationInitializer] - [UNLOADING]: ApplicationManager scene unloaded.");
        }

        /// <summary>
        /// Initiates the unloading of the Init scene.
        /// </summary>
        private void UnloadInitScene()
        {
            Debug.Log(
                "[ApplicationInitializer] - [UNLOADING]: Called UnloadInitScene() to unload Init scene.");
            StartCoroutine(UnloadInitSceneCoroutine());
        }

        /// <summary>
        /// Coroutine to unload the Init scene once it is fully loaded.
        /// </summary>
        private IEnumerator UnloadInitSceneCoroutine()
        {
            while (!SceneManager.GetSceneByName("Init").isLoaded ||
                   SceneManager.GetActiveScene().name == "Init")
            {
                yield return null;
            }

            // SceneManager.LoadScene("LoadingScreen", LoadSceneMode.Additive);
            // yield return null;

            Debug.Log(
                "[ApplicationInitializer] - Ensuring ApplicationInitializer is destroyed.");
            Destroy(gameObject);

            Debug.Log(
                "[ApplicationInitializer] - [UNLOADING]: Unloading Init scene.");
            SceneManager.UnloadSceneAsync("Init");
            Debug.Log("[ApplicationInitializer] - [UNLOADING]: Init scene unloaded.");
            // SceneManager.UnloadSceneAsync("LoadingScreen");
        }

        private IEnumerator LoadMainMenuSceneCoroutine()
        {
            // Load the MainMenu scene.
            AsyncOperation loadMainMenu = SceneManager.LoadSceneAsync
                ("MainMenu", LoadSceneMode.Single);

            // Wait until it is fully loaded.
            yield return new WaitUntil(() => loadMainMenu is { isDone: true });

            // Set the MainMenu scene as the active scene.
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainMenu"));
        }
    }
}
