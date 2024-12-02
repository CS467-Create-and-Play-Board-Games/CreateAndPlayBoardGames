using Runtime.Controllers.System;
using Runtime.Managers.Submanagers;
using UnityEngine;

namespace Runtime.Managers
{
    /// <summary>
    /// Manages the overall application by handling the initialization and dependency injection
    /// of various manager components.
    /// </summary>
    public class ApplicationManager : MonoBehaviour
    {
        // Singleton instance of the ApplicationManager
        public static ApplicationManager Instance { get; private set; }

        public AudioManager audioManager;
        public CameraManager cameraManager;
        public EventSystemManager eventSystemManager;
        public GameDataPersistenceManager gameDataPersistenceManager;
        public GlobalGameStateManager globalGameStateManager;
        public InputManager inputManager;
        public SceneController sceneController;
        public SettingsManager settingsManager;
        public UserInterfaceManager userInterfaceManager;

        /// <summary>
        /// Injects dependencies into the ApplicationManager.
        /// </summary>
        /// <param name="injectedCameraManager">The CameraManager to inject.</param>
        /// <param name="injectedAudioManager">The AudioManager to inject.</param>
        /// <param name="injectedEventManager">The EventManager to inject.</param>
        /// <param name="injectedGameDataPersistenceManager">The GameDataPersistenceManager to inject.</param>
        /// <param name="injectedGlobalGameStateManager">The GlobalGameStateManager to inject.</param>
        /// <param name="injectedInputManager">The InputManager to inject.</param>
        /// <param name="injectedSceneController">The SceneController to inject.</param>
        /// <param name="injectedSettingsManager">The SettingsManager to inject.</param>
        /// <param name="injectedUserInterfaceManager">The UserInterfaceManager to inject.</param>
        public void InjectDependencies(
            CameraManager injectedCameraManager,
            AudioManager injectedAudioManager,
            EventSystemManager injectedEventManager,
            GameDataPersistenceManager injectedGameDataPersistenceManager,
            GlobalGameStateManager injectedGlobalGameStateManager,
            InputManager injectedInputManager,
            SceneController injectedSceneController,
            SettingsManager injectedSettingsManager,
            UserInterfaceManager injectedUserInterfaceManager)
        {
            cameraManager = injectedCameraManager;
            audioManager = injectedAudioManager;
            eventSystemManager = injectedEventManager;
            gameDataPersistenceManager = injectedGameDataPersistenceManager;
            globalGameStateManager = injectedGlobalGameStateManager;
            inputManager = injectedInputManager;
            sceneController = injectedSceneController;
            settingsManager = injectedSettingsManager;
            userInterfaceManager = injectedUserInterfaceManager;
        }

        /// <summary>
        /// Called when the script instance is being loaded.
        /// Ensures a single instance and initializes dependencies.
        /// </summary>
        private void Awake()
        {
            // Ensure Singleton Instance.
            if (Instance == null)
            {
                Instance = this;
                // Keep the ApplicationManager persistent across scenes.
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            // Call Initialize after dependencies are injected.
            InitializeDependencies();
        }

        /// <summary>
        /// Called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// Ensures managers are set as children of the ApplicationManager.
        /// </summary>
        private void Start()
        {
            // Ensure managers are set as children of the ApplicationManager.
            SetManagersAsChildren();
        }

        /// <summary>
        /// Initializes all injected dependencies.
        /// </summary>
        public void InitializeDependencies()
        {
            // Initialize all managers
            cameraManager.Initialize();
            // audioManager.Initialize();
            eventSystemManager.Initialize();
            // gameDataPersistenceManager.Initialize();
            // globalGameStateManager.Initialize();
            // inputManager.Initialize();
            sceneController.Initialize();
            // settingsManager.Initialize();
            // userInterfaceManager.Initialize();
        }

        /// <summary>
        /// Sets all submanagers (and controllers, if applicable) as
        /// children of the ApplicationManager.
        /// This should ensure that they are not destroyed when
        /// loading new scenes.
        /// </summary>
        void SetManagersAsChildren()
        {
            cameraManager.transform.SetParent(transform);
            audioManager.transform.SetParent(transform);
            eventSystemManager.transform.SetParent(transform);
            gameDataPersistenceManager.transform.SetParent(transform);
            globalGameStateManager.transform.SetParent(transform);
            inputManager.transform.SetParent(transform);
            sceneController.transform.SetParent(transform);
            settingsManager.transform.SetParent(transform);
            userInterfaceManager.transform.SetParent(transform);
        }

        /// <summary>
        /// Called every frame, if the MonoBehaviour is enabled.
        /// Updates the logic of the CameraManager.
        /// </summary>
        private void Update()
        {
            cameraManager.UpdateLogic();
        }
    }
}
