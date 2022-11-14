using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace DebuggingTools
{
    /// <summary>
    /// Class <c>DebugUI</c> is the main class handling the debugging tool
    /// </summary>
    public class DebugUI : MonoBehaviour
    {
        [SerializeField] private GameObject _debugContainer;

        [SerializeField] private GameObject _debugGameObjectWidget;

        private void Start()
        {
            FindObjectOfType<DebugToggle>().ToggleDebuggingToolsEvent.AddListener(ToggleDebuggingTools);
            FindObjectOfType<DebugData>().DebugDataChangedEvent.AddListener(RebuildUI);

            gameObject.SetActive(false);
        }

        private void ClearWidgets()
        {
            for (int i = _debugContainer.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(_debugContainer.transform.GetChild(i).gameObject);
            }
        }

        private void RebuildUI(List<GameObject> debugGameObjects)
        {
            if (!gameObject.activeSelf) return;
            
            ClearWidgets();

            foreach (GameObject debugGameObject in debugGameObjects)
            {
                Instantiate(_debugGameObjectWidget, _debugContainer.transform)
                    .GetComponent<DebugGameObjectUI>()
                    .Initialize(debugGameObject);
            }
        }

        private void ToggleDebuggingTools(bool isActivated)
        {
            gameObject.SetActive(isActivated);

            if (!isActivated) return;
            
            RebuildUI(FindObjectOfType<DebugData>().GetDebugGameObjects());
        }
    }
}