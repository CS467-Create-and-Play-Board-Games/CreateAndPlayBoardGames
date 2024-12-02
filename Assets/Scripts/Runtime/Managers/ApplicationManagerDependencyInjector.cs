using Runtime.Controllers.System;
using Runtime.Managers.Submanagers;
using Runtime.Shared.SceneManagement;
using UnityEngine;

namespace Runtime.Managers
{
    /// <summary>
    /// Manages the dependency injection for the ApplicationManager by creating and injecting
    /// various manager components.
    /// </summary>
    public class ApplicationManagerDependencyInjector : MonoBehaviour
    {
        private ApplicationManager _applicationManager;

        public AudioManager audioManager;
        public CameraManager cameraManager;
        public EventSystemManager eventManager;
        public GameDataPersistenceManager gameDataPersistenceManager;
        public GlobalGameStateManager globalGameStateManager;
        public InputManager inputManager;
        public SceneController sceneController;
        public SettingsManager settingsManager;
        public UserInterfaceManager userInterfaceManager;

        /// <summary>
        /// Called when the script instance is being loaded.
        /// Initializes and injects dependencies into the ApplicationManager.
        /// </summary>
        private void Awake()
        {
            // Create and add the AudioManager component.
            GameObject audioManagerGO = new GameObject("AudioManager");
            audioManager = audioManagerGO.AddComponent<AudioManager>();

            // Create and add the CameraManager component.
            GameObject cameraManagerGO = new GameObject("CameraManager");
            cameraManager = cameraManagerGO.AddComponent<CameraManager>();

            // Create and add the EventManager component.
            GameObject eventSystemManagerGO = new GameObject("EventSystemManager");
            eventManager = eventSystemManagerGO.AddComponent<EventSystemManager>();

            // Create and add the GameDataPersistenceManager component.
            GameObject gameDataPersistenceManagerGO = new GameObject("GameDataPersistenceManager");
            gameDataPersistenceManager = gameDataPersistenceManagerGO.AddComponent<GameDataPersistenceManager>();

            // Create and add the GlobalGameStateManager component.
            GameObject globalGameStateManagerGO = new GameObject("GlobalGameStateManager");
            globalGameStateManager = globalGameStateManagerGO.AddComponent<GlobalGameStateManager>();

            // Create and add the InputManager component.
            GameObject inputManagerGO = new GameObject("InputManager");
            inputManager = inputManagerGO.AddComponent<InputManager>();

            // Create and add the SceneController component.
            GameObject sceneControllerGO = new GameObject("SceneController");
            sceneController = sceneControllerGO.AddComponent<SceneController>();

            // Create and add the SettingsManager component.
            GameObject settingsManagerGO = new GameObject("SettingsManager");
            settingsManager = settingsManagerGO.AddComponent<SettingsManager>();

            // Create and add the UserInterfaceManager component.
            GameObject userInterfaceManagerGO = new GameObject("UserInterfaceManager");
            userInterfaceManager = userInterfaceManagerGO.AddComponent<UserInterfaceManager>();

            // Find the existing ApplicationManager and inject the dependencies.
            _applicationManager = FindObjectOfType<ApplicationManager>();
            if (_applicationManager != null)
            {
                // Inject the components into the ApplicationManager.
                _applicationManager.InjectDependencies(
                    cameraManager,
                    audioManager,
                    eventManager,
                    gameDataPersistenceManager,
                    globalGameStateManager,
                    inputManager,
                    sceneController,
                    settingsManager,
                    userInterfaceManager
                );

                // Initialize the dependencies.
                _applicationManager.InitializeDependencies();
            }
            else
            {
                Debug.LogError("ApplicationManager not found in the scene.");
            }
        }
    }
}
