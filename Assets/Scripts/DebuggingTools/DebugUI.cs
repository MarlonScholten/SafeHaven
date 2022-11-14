using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DebuggingTools
{
    /// <summary>
    /// Class <c>DebugUI</c> is the main class handling the debugging tool
    /// </summary>
    public class DebugUI : MonoBehaviour
    {
        [SerializeField] private GameObject debugContainer;

        [SerializeField] private GameObject debugGameObjectWidget;

        private void Start()
        {
            FindObjectOfType<DebugToggle>().ToggleDebuggingToolsEvent.AddListener(ToggleDebuggingTools);
            FindObjectOfType<DebugData>().DebugDataChangedEvent.AddListener(RebuildUI);

            gameObject.SetActive(false);
        }

        private void ClearWidgets()
        {
            for (int i = debugContainer.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(debugContainer.transform.GetChild(i).gameObject);
            }
        }

        private void RebuildUI(List<GameObject> debugGameObjects)
        {
            if (!gameObject.activeSelf) return;
            
            ClearWidgets();

            foreach (GameObject debugGameObject in debugGameObjects)
            {
                Instantiate(debugGameObjectWidget, debugContainer.transform)
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