using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTransform : MonoBehaviour, IDebuggableObject
{
    [SerializeField]
    private bool autoActivate = true;
    
    private void Start()
    {
        if (!autoActivate) return;
        
        FindObjectOfType<DebugUI>().AddDebugGameObject(gameObject);
    }

    public Dictionary<string, string> GetDebugValues()
    {
        return new Dictionary<string, string>
        {
            { "Position", transform.position.ToString() },
            { "Rotation", transform.rotation.eulerAngles.ToString() },
            { "Scale", transform.localScale.ToString() } 
        };
    }
}
