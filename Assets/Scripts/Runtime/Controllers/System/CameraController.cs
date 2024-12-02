using System;
using Runtime.Shared.DataModels;
using UnityEngine;

namespace Runtime.Controllers.System
{
    /// <summary>
    /// Controls the camera based on the current scene mode.
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        private enum CameraMode
        {
            Menu,
            Editor,
            Gameplay,
        }

        private Action _updateCameraBehavior;
        // private CameraMode _currentCameraMode;

        // Speed at which the camera drags.
        public float dragSpeed = 2;

        // Speed at which the camera zooms.
        public float zoomSpeed = 2;

        // Minimum zoom level.
        public float minZoom = 1f;

        // Maximum zoom level.
        public float maxZoom = 15f;

        // Current scene mode.
        private SceneMode _currentSceneMode;

        // Origin point for dragging.
        private Vector3 _dragOrigin;

        // Reference to the main camera.
        private Camera _mainCamera;

        /// <summary>
        /// Initializes the main camera.
        /// </summary>
        private void Awake()
        {
            _mainCamera = Camera.main;
            if (_mainCamera == null)
            {
                Debug.LogWarning("Main Camera is not assigned.");
            }
        }

        /// <summary>
        /// Updates the camera behavior based on the current scene mode.
        /// </summary>
        public void UpdateCamera()
        {
            _updateCameraBehavior?.Invoke();
        }

        /// <summary>
        /// Sets the camera to gameplay mode.
        /// </summary>
        public void SetGameplayCamera()
        {
            // _currentCameraMode = CameraMode.Gameplay;
            _updateCameraBehavior = UpdateGameplayCamera;
            ConfigureGameplayCamera();
        }

        /// <summary>
        /// Sets the camera to editor mode.
        /// </summary>
        public void SetEditorCamera()
        {
            // _currentCameraMode = CameraMode.Editor;
            _updateCameraBehavior = UpdateEditorCamera;
            ConfigureEditorCamera();
        }

        /// <summary>
        /// Sets the camera to menu mode.
        /// </summary>
        public void SetMenuCamera()
        {
            // _currentCameraMode = CameraMode.Menu;
            _updateCameraBehavior = UpdateMenuCamera;
            ConfigureMenuCamera();
        }

        /// <summary>
        /// Configures the camera for gameplay mode.
        /// </summary>
        private void ConfigureGameplayCamera()
        {
            Debug.Log("Camera configured for gameplay mode.");
            // _mainCamera.backgroundColor = Color.green;
        }

        /// <summary>
        /// Configures the camera for editor mode.
        /// </summary>
        private void ConfigureEditorCamera()
        {
            Debug.Log("Camera configured for editor mode.");
            // _mainCamera.backgroundColor = Color.blue;
        }

        /// <summary>
        /// Configures the camera for menu mode.
        /// </summary>
        private void ConfigureMenuCamera()
        {
            Debug.Log("Camera configured for menu mode.");
            // _mainCamera.backgroundColor = Color.red;
        }

        /// <summary>
        /// Handles the camera behavior for a menu scene.
        /// </summary>
        private void UpdateMenuCamera()
        {
        }

        /// <summary>
        /// Handles the camera behavior for an editor scene.
        /// </summary>
        private void UpdateEditorCamera()
        {
            HandleDragView();
            HandleZoomView();
        }

        /// <summary>
        /// Handles the camera behavior for an in-game scene.
        /// </summary>
        private void UpdateGameplayCamera()
        {
            HandleDragView();
            HandleZoomView();
        }

        /// <summary>
        /// Handles the camera dragging behavior.
        /// </summary>
        private void HandleDragView()
        {
            if (Input.GetMouseButtonDown(2))
            {
                // Debug.Log("Drag origin set.");
                _dragOrigin = Input.mousePosition;
                return;
            }

            if (!Input.GetMouseButton(2))
            {
                // Debug.Log("Not dragging view.");
                return;
            }

            // Debug.Log("Dragging View.");
            var difference = _mainCamera.ScreenToWorldPoint(
                                 Input.mousePosition) -
                             _mainCamera.ScreenToWorldPoint(_dragOrigin);
            difference.z = 0;

            transform.position -= difference * dragSpeed;
            _dragOrigin = Input.mousePosition;
        }

        /// <summary>
        /// Handles the camera zooming behavior.
        /// </summary>
        private void HandleZoomView()
        {
            var scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (scrollInput == 0) return;
            var newSize = _mainCamera.orthographicSize - scrollInput *
                zoomSpeed;
            newSize = Mathf.Clamp(newSize, minZoom, maxZoom);
            _mainCamera.orthographicSize = newSize;
        }
    }
}
