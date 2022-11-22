using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// This script is a the Investigate state of the enemy.
/// </summary>
/// <list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///     <item>
///         <term>The GameObject this thing need to be on for this script to work</term>
///		    <term>What type of thing this is. Component, Script, Tag or Layer?</term>
///         <term>The name of the thing</term>
///		    <term>A description of why this thing is needed</term>
///	    </item>
///	    <item>
///         <term>EnemyObject (Assets/Prefabs/NPCs/Enemies/EnemyObject.prefab)</term>
///		    <term>Component</term>
///         <term>EnemyObject</term>
///		    <term>This script contains the ENTER/(FIXED)UPDATE/EXIT Investigate states</term>
///	    </item>
/// </list>
public class InvestigateState : MonoBehaviour
{
    private EnemyAiStateManager _stateManager;
    
    private IEnumerator _investigateCoroutine;
    private bool _investigateCoroutineIsRunning;
    private IEnumerator _waitingAtWaypointDuringInvestigationCoroutine;
    private bool _waitingAtWaypointDuringInvestigationCoroutineIsRunning;
    void Awake()
    {
        _stateManager = GetComponent<EnemyAiStateManager>();
    }
   
    /// <summary>
    /// Enter investigate state
    /// </summary>
    public void Enter_Investigate()
    {
        if (_stateManager.alertedBySound) _stateManager.CalculateInvestigateLocation(_stateManager.locationOfNoise);
        else if(_stateManager.alertedByVision) _stateManager.CalculateInvestigateLocation(_stateManager.spottedPlayerLastPosition);
    }
    
    /// <summary>
    /// Update investigate state
    /// </summary>
    public void Update_Investigate()
    {
        //If the player is in vision, chase the player
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
        //Check if location is reachable
        if (!_stateManager.waitingAtWaypoint && _stateManager.navMeshAgent.velocity.magnitude < 0.1f)
        {
            _stateManager.CalculateInvestigateLocation(transform.position);
        }
        //Check if the enemy is at the location.
        
        if (_stateManager.CheckIfEnemyIsAtWaypoint())
        {
            //If the enemy is at the location, start the waiting coroutine
            if (_stateManager.waitingAtWaypoint && !_investigateCoroutineIsRunning)
            {
                _investigateCoroutineIsRunning = true;
                _investigateCoroutine =
                    _stateManager.CallFunctionAfterSeconds(_stateManager.enemyAiScriptableObject.investigateTime, () =>
                    {
                        CustomEvent.Trigger(gameObject, "Patrol");
                        
                        _investigateCoroutineIsRunning = false;
                    });
                StartCoroutine(_investigateCoroutine);
            }
            //If the enemy is not waiting at the location, calculate the next location.
            if (!_stateManager.waitingAtWaypoint)
            {
                _stateManager.waitingAtWaypoint = true;
                _waitingAtWaypointDuringInvestigationCoroutineIsRunning = true;
                _waitingAtWaypointDuringInvestigationCoroutine =
                    _stateManager.CallFunctionAfterSeconds(_stateManager.enemyAiScriptableObject.waitAtInvestigatingWaypointTime, () =>
                    {
                        if (_stateManager.alertedBySound) _stateManager.CalculateInvestigateLocation(_stateManager.locationOfNoise);
                        else if(_stateManager.alertedByVision) _stateManager.CalculateInvestigateLocation(_stateManager.spottedPlayerLastPosition);
                        _waitingAtWaypointDuringInvestigationCoroutineIsRunning = false;
                        
                    });
                StartCoroutine(_waitingAtWaypointDuringInvestigationCoroutine);
            }
            //look around at each waypoint.
            _stateManager.LookAround();
        }
    }
    
    /// <summary>
    /// Exit investigate state
    /// </summary>
    public void Exit_Investigate()
    {
        //Stop the investigate coroutine
        if(_investigateCoroutineIsRunning)StopCoroutine(_investigateCoroutine);
        _investigateCoroutineIsRunning = false;
        //Stop the waiting at waypoint coroutine
        if(_waitingAtWaypointDuringInvestigationCoroutineIsRunning)StopCoroutine(_waitingAtWaypointDuringInvestigationCoroutine);
        _waitingAtWaypointDuringInvestigationCoroutineIsRunning = false;
    }
    
    
}
