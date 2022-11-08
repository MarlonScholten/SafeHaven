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

    void Start()
    {
        foreach (GameObject debugGameObject in debugGameObjects)
        {
            Instantiate(debugGameObjectWidget, debugContainer.transform)
                .GetComponent<DebugGameObjectWidget>()
                .Initialize(debugGameObject);
        }
    }
}
