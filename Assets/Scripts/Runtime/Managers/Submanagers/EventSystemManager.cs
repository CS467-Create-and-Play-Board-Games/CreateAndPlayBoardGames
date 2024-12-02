using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.Managers.Submanagers
{
    public class EventSystemManager : MonoBehaviour
    {
        // Singleton instance
        public static EventSystemManager Instance { get; private set; }

        // The EventSystem reference
        private EventSystem _eventSystem;

        public void Initialize()
        {
            EnsureSingleEventSystem();
        }

        private void EnsureSingleEventSystem()
        {
            // Check if an EventSystem already exists
            _eventSystem = FindObjectOfType<EventSystem>();

            if (_eventSystem == null)
            {
                // No EventSystem found, create a new one
                _eventSystem = new GameObject("EventSystem").AddComponent<EventSystem>();

                // Optionally, add the StandaloneInputModule if not present
                if (_eventSystem.GetComponent<StandaloneInputModule>() == null)
                {
                    _eventSystem.gameObject.AddComponent<StandaloneInputModule>();
                }

                _eventSystem.transform.SetParent(transform);
                Debug.Log("[EventSystemManager] Created a new EventSystem.");
            }
            else
            {
                Debug.Log("[EventSystemManager] An EventSystem already exists.");
            }
        }
    }
}
