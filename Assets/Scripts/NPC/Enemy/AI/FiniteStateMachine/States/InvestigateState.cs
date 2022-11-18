using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InvestigateState : MonoBehaviour
{
    private Enemy_Finite_State_Machine _stateManager;
    
    private IEnumerator _investigateCoroutine;
    private bool _investigateCoroutineIsRunning;
    private IEnumerator _waitingAtWaypointDuringInvestigationCoroutine;
    private bool _waitingAtWaypointDuringInvestigationCoroutineIsRunning;
    
    private FSM_Scriptable_Object _fsmScriptableObject;
    void Start()
    {
        _stateManager = FindObjectOfType<Enemy_Finite_State_Machine>();
    }
    
    
    /// INVESTIGATE ///
    
    /// <summary>
    /// Enter investigate state
    /// </summary>
    public void Enter_Investigate()
    {
        _stateManager.CalculateInvestigateLocation();
    }
    
    /// <summary>
    /// Update investigate state
    /// </summary>
    public void Update_Investigate()
    {
        if (_stateManager.CheckVision())
        {
            CustomEvent.Trigger(gameObject, "Chasing");
        }

    }
    
    /// <summary>
    /// FIxed update investigate state
    /// </summary>
    public void FixedUpdate_Investigate()
    {
        if (_stateManager.CheckIfEnemyIsAtWaypoint())
        {
            if (_stateManager.waitingAtWaypoint && !_investigateCoroutineIsRunning)
            {
                _investigateCoroutineIsRunning = true;
                _investigateCoroutine =
                    _stateManager.CallFunctionAfterSeconds(_fsmScriptableObject.investigateTime, () =>
                    {
                        CustomEvent.Trigger(gameObject, "Patrol");
                        _investigateCoroutineIsRunning = false;
                    });
                StartCoroutine(_investigateCoroutine);
            }
            if (!_stateManager.waitingAtWaypoint)
            {
                _stateManager.waitingAtWaypoint = true;
                _waitingAtWaypointDuringInvestigationCoroutineIsRunning = true;
                _waitingAtWaypointDuringInvestigationCoroutine =
                    _stateManager.CallFunctionAfterSeconds(_fsmScriptableObject.waitAtInvestigatingWaypointTime, () =>
                    {
                        _stateManager.CalculateInvestigateLocation();
                        _waitingAtWaypointDuringInvestigationCoroutineIsRunning = false;
                        
                    });
                StartCoroutine(_waitingAtWaypointDuringInvestigationCoroutine);
            }
            _stateManager.LookAround();
        }
    }
    
    /// <summary>
    /// Exit investigate state
    /// </summary>
    public void Exit_Investigate()
    {
        if(_investigateCoroutineIsRunning)StopCoroutine(_investigateCoroutine);
        _investigateCoroutineIsRunning = false;
        if(_waitingAtWaypointDuringInvestigationCoroutineIsRunning)StopCoroutine(_waitingAtWaypointDuringInvestigationCoroutine);
        _waitingAtWaypointDuringInvestigationCoroutineIsRunning = false;
    }
    
    
}
