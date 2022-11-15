using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractableBehaviour : MonoBehaviour
{
    // Public.

    // Private.

    private void Awake()
    {
        foreach (var test in GetComponentsInChildren<WaypointBehaviour>())
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
