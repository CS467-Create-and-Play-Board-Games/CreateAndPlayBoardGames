using Cinemachine;
using Runtime.Controllers.System;
using UnityEngine;

namespace Runtime.Managers.Submanagers
{
    /// <summary>
    /// Manages the main camera and its behavior based on the current scene mode.
    /// </summary>
    public class CameraManager : MonoBehaviour
    {
        // Reference to the main camera.
        public Camera mainCamera;

        // Reference to the CameraController component.
        private CameraController _cameraController;

        /// <summary>
        /// Initializes the camera manager, sets up the main camera, and
        /// configures the camera behavior.
        /// </summary>
        public void Initialize()
        {
            mainCamera = Camera.main;

            if (CheckIfMainCameraAlreadyExists())
            {
                Debug.Log("Main camera already exists and is being used.");
                mainCamera = Camera.main;
            }
            else
            {
                Debug.Log("Main camera does not exist. Creating a new one.");
                CreateCamera();
            }

            if (CheckIfUsingCinemachine())
            {
                Debug.Log("Cinemachine detected. Using Cinemachine camera.");
            }
            else
            {
                EnsureCameraControllerExists();
                SetCameraBehaviorBasedOnCurrentSceneMode();
            }
        }

        private static bool CheckIfUsingCinemachine()
        {
            return FindObjectOfType<CinemachineBrain>() != null || FindObjectOfType<CinemachineVirtualCamera>() != null;
        }

        /// <summary>
        /// Checks if the main camera already exists in the scene.
        /// </summary>
        /// <returns>True if the main camera exists, false otherwise.</returns>
        private static bool CheckIfMainCameraAlreadyExists()
        {
            return Camera.main != null;
        }

        /// <summary>
        /// Creates a new main camera with default settings.
        /// </summary>
        private void CreateCamera()
        {
            mainCamera = new GameObject("Main Camera").AddComponent<Camera>();
            mainCamera.tag = "MainCamera";
            mainCamera.transform.position = new Vector3(0, 0, -15);

            mainCamera.orthographic = true;
            mainCamera.orthographicSize = 5;
            mainCamera.nearClipPlane = 0.3f;
            mainCamera.farClipPlane = 1000;

            mainCamera.clearFlags = CameraClearFlags.SolidColor;
            mainCamera.backgroundColor = Color.black;

            mainCamera.transform.SetParent(transform);
        }

        /// <summary>
        /// Ensures that a CameraController component exists on the main camera.
        /// </summary>
        private void EnsureCameraControllerExists()
        {
            _cameraController = mainCamera.GetComponent<CameraController>();
            if (_cameraController != null) return;

            Debug.Log("CameraController not found. Creating a new one.");

            _cameraController = mainCamera.gameObject.AddComponent<CameraController>();
        }

        /// <summary>
        /// Sets the camera behavior based on the current scene mode.
        /// </summary>
        private void SetCameraBehaviorBasedOnCurrentSceneMode()
        {
            // Todo: Implement a more flexible and scalable method of determining the current scene mode.
            // For testing, the scene name can be used to determine the mode.
            string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            if (currentSceneName.Contains("Menu"))
            {
                Debug.Log("Menu scene detected");
                _cameraController.SetMenuCamera();
            }
            else if (currentSceneName.Contains("Editor"))
            {
                Debug.Log("Editor scene detected");
                _cameraController.SetEditorCamera();
            }
            else if (currentSceneName.Contains("Game"))
            {
                Debug.Log("Game scene detected");
                _cameraController.SetGameplayCamera();
            }
            else
            {
                Debug.LogWarning("Scene mode not detected");
                _cameraController.SetGameplayCamera();
            }
        }

        /// <summary>
        /// Updates the camera logic each frame.
        /// </summary>
        public void UpdateLogic()
        {
            _cameraController?.UpdateCamera();
        }
    }
}
