using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SittingState : MonoBehaviour
{
    private BirdStateManager _birdStateManager;
    private IEnumerator _sittingCoroutine;
    private bool _sittingCoroutineIsRunning;

    public void Awake()
    {
        _birdStateManager = GetComponent<BirdStateManager>();
    }

    public void ENTER_SITTING_STATE()
    {
       // get rotation of _birdStateManager.restPoint

       transform.rotation = _birdStateManager.restPoint.rotation;
       _sittingCoroutineIsRunning = true;
        _sittingCoroutine = _birdStateManager.CallFunctionAfterSeconds(_birdStateManager.birdScriptableObject.TimeAtRestPoint,
            () => _sittingCoroutineIsRunning = false);
        StartCoroutine(_sittingCoroutine);
    }
    
    public void UPDATE_SITTING_STATE()
    {
        if (!_sittingCoroutineIsRunning &&
            !_birdStateManager.CheckIfAlertingObjectsAreNearby(_birdStateManager.birdScriptableObject.AlertTags))
        {
            CustomEvent.Trigger(gameObject, "FlyingTowardsNavmesh");
        }
    }
    
    public void FIXED_UPDATE_SITTING_STATE()
    {
        // not implemented yet
    }
    
    public void EXIT_SITTING_STATE()
    {
        if(_sittingCoroutineIsRunning) StopCoroutine(_sittingCoroutine);
        _sittingCoroutineIsRunning = false;
        _birdStateManager.restPoint = null;
    }
}
