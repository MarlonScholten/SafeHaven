using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DebuggingTools
{
    public class ToggleEvent : UnityEvent<bool>
    {
    }

    /// <summary>
    /// Class <c>DebugUI</c> is the main class handling the debugging tool
    /// </summary>
    public class DebugUI : MonoBehaviour
    {
        /// <summary>
        /// To this event can be subscribed to receive a event when the debugging tool is enabled or disabled
        /// <returns>A boolean that resembles if the debugging tool is enabled or disabled</returns>
        /// </summary>
        public ToggleEvent ToggleDebuggingToolsEvent;
    
        [SerializeField] 
        private GameObject debugContainer;

        [SerializeField] 
        private GameObject debugGameObjectWidget;

        [SerializeField]
        private List<GameObject> debugGameObjects;
    
        private bool _debuggerEnabled;
        private Canvas _canvasComponent;

        /// <summary>
        /// This method adds a <c>GameObject</c> to be displayed in the debugging tool.
        /// </summary>
        /// <param name="gameObjectToAdd">The <c>GameObject</c> to add</param>
        public void AddDebugGameObject(GameObject gameObjectToAdd)
        {
            debugGameObjects.Add(gameObjectToAdd);
        
            Instantiate(debugGameObjectWidget, debugContainer.transform)
                .GetComponent<DebugGameObjectUI>()
                .Initialize(gameObjectToAdd);
        }
        
        private void Awake()
        {
            ToggleDebuggingToolsEvent = new ToggleEvent();
        }

        private void Start()
        {
            ToggleDebuggingToolsEvent.Invoke(_debuggerEnabled);
        
            _canvasComponent = GetComponent<Canvas>();
            _canvasComponent.enabled = _debuggerEnabled;
        
            ClearWidgets();

            foreach (GameObject debugGameObject in debugGameObjects)
            {
                Instantiate(debugGameObjectWidget, debugContainer.transform)
                    .GetComponent<DebugGameObjectUI>()
                    .Initialize(debugGameObject);
            }
        }

        private void ClearWidgets()
        {
            for (int i = debugContainer.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(debugContainer.transform.GetChild(i).gameObject);
            }
        }

        private void OnShowDebuggingTools()
        {
            _debuggerEnabled = !_debuggerEnabled;
        
            ToggleDebuggingToolsEvent.Invoke(_debuggerEnabled);

            _canvasComponent.enabled = _debuggerEnabled;
        }
    }
}