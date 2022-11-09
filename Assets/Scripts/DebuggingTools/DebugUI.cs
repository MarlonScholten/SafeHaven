using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    [SerializeField] 
    private GameObject debugContainer;

    [SerializeField] 
    private GameObject debugGameObjectWidget;

    [SerializeField]
    private List<GameObject> debugGameObjects;

    [SerializeField] 
    private List<DebugHighlight> highlightGameObjects;

    private bool _debuggerEnabled;
    private Canvas _canvasComponent;

    void Start()
    {
        _canvasComponent = GetComponent<Canvas>();
        _canvasComponent.enabled = _debuggerEnabled;
        
        foreach (GameObject debugGameObject in debugGameObjects)
        {
            Instantiate(debugGameObjectWidget, debugContainer.transform)
                .GetComponent<DebugGameObjectWidget>()
                .Initialize(debugGameObject);
        }
    }

    public void AddHighlightGameObject(DebugHighlight gameObjectToAdd)
    {
        highlightGameObjects.Add(gameObjectToAdd);
    }

    public void AddDebugGameObject(GameObject gameObjectToAdd)
    {
        debugGameObjects.Add(gameObjectToAdd);
        
        Instantiate(debugGameObjectWidget, debugContainer.transform)
            .GetComponent<DebugGameObjectWidget>()
            .Initialize(gameObjectToAdd);
    }

    private void OnShowDebuggingTools()
    {
        _debuggerEnabled = !_debuggerEnabled;

        _canvasComponent.enabled = _debuggerEnabled;
        
        foreach (DebugHighlight highlightGameObject in highlightGameObjects)
        {
            if (_debuggerEnabled) highlightGameObject.Highlight();
            else highlightGameObject.UnHighlight();
        }
    }
}
