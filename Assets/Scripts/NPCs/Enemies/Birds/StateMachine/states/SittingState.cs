using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SittingState : MonoBehaviour
{
    private BirdStateManager _birdStateManager;
    private IEnumerator _sittingCoroutine;
    

    public void Awake()
    {
        _birdStateManager = GetComponent<BirdStateManager>();
    }

    public void ENTER_SITTING_STATE()
    {
        transform.position = _birdStateManager.restPoint;
        _sittingCoroutine = _birdStateManager.CallFunctionAfterSeconds(_birdStateManager.birdScriptableObject.TimeAtRestPoint,
            () => CustomEvent.Trigger(gameObject, "Flying"));
    }
    
    public void UPDATE_SITTING_STATE()
    {
        StartCoroutine(_sittingCoroutine);
    }
    
    public void FIXED_UPDATE_SITTING_STATE()
    {
        // not implemented yet
    }
    
    public void EXIT_SITTING_STATE()
    {
        _birdStateManager.restPoint = Vector3.zero;
    }
}
