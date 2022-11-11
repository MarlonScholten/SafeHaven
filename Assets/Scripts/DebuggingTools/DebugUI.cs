using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DebuggingTools
{
    public class ToggleEvent : UnityEvent<bool>
    {
    }

    public class DebugUI : MonoBehaviour
    {
        public ToggleEvent ToggleDebuggingTools;
    
        [SerializeField] 
        private GameObject debugContainer;

        [SerializeField] 
        private GameObject debugGameObjectWidget;

        [SerializeField]
        private List<GameObject> debugGameObjects;
    
        private bool _debuggerEnabled;
        private Canvas _canvasComponent;

        public void AddDebugGameObject(GameObject gameObjectToAdd)
        {
            debugGameObjects.Add(gameObjectToAdd);
        
            Instantiate(debugGameObjectWidget, debugContainer.transform)
                .GetComponent<DebugGameObjectWidget>()
                .Initialize(gameObjectToAdd);
        }
        
        private void Awake()
        {
            ToggleDebuggingTools = new ToggleEvent();
        }

        private void Start()
        {
            ToggleDebuggingTools.Invoke(_debuggerEnabled);
        
            _canvasComponent = GetComponent<Canvas>();
            _canvasComponent.enabled = _debuggerEnabled;
        
            ClearWidgets();

            foreach (GameObject debugGameObject in debugGameObjects)
            {
                Instantiate(debugGameObjectWidget, debugContainer.transform)
                    .GetComponent<DebugGameObjectWidget>()
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
        
            ToggleDebuggingTools.Invoke(_debuggerEnabled);

            _canvasComponent.enabled = _debuggerEnabled;
        }
    }
}