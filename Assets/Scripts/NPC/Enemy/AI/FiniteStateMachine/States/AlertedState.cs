using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AlertedState : MonoBehaviour
{
    private Enemy_Finite_State_Machine _stateManager;
    private IEnumerator _alertedCoroutine;
    private bool _alertedCoroutineIsRunning;
    private FSM_Scriptable_Object _fsmScriptableObject;

    void Start()
    {
        _stateManager = FindObjectOfType<Enemy_Finite_State_Machine>();
        
    }
    
    
    /// ALERTED ///
    
    /// <summary>
    /// Enter alerted state
    /// </summary>
    public void Enter_Alerted()
    {
        if (_stateManager.alertedBySound)
        {
            _stateManager.navMeshAgent.isStopped = true;
            _alertedCoroutineIsRunning = true;
            _alertedCoroutine = _stateManager.CallFunctionAfterSeconds(_fsmScriptableObject.stopWhenAlertedTime, () =>
            {
                _stateManager.navMeshAgent.isStopped = false; 
                _alertedCoroutineIsRunning = false;
            });
            StartCoroutine(_alertedCoroutine);
        }
    }
    
    /// <summary>
    /// Update alerted state
    /// </summary>
    public void Update_Alerted()
    {
        if (_alertedCoroutineIsRunning) return;
        if (_stateManager.alertedBySound)
        {
            CustomEvent.Trigger(gameObject, "Investigate");
        }
        else if (_stateManager.alertedByVision)
        {
            CustomEvent.Trigger(gameObject, "Chasing");
        }
        else
        {
            CustomEvent.Trigger(gameObject, "Patrol");
        }
    }
    
    /// <summary>
    /// Fixed update alerted state
    /// </summary>
    public void FixedUpdate_Alerted()
    {
        _stateManager.LookAround();
    }
    
    /// <summary>
    /// Exit alerted state
    /// </summary>
    public void Exit_Alerted()
    {
        if(_alertedCoroutineIsRunning)StopCoroutine(_alertedCoroutine);
        _alertedCoroutineIsRunning = false;
    }
}
