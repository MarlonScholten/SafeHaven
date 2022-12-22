using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Author: Marlon Kerstens<br/>
/// Modified by: Hugo Ulfman, Thomas van den Oever<br/>
/// Description: This script is a the Investigate state of the enemy.
/// </summary>
/// <list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///	    <item>
///         <term>EnemyObject (Assets/Prefabs/NPCs/Enemies/EnemyObject.prefab)</term>
///		    <term>Component</term>
///         <term>EnemyObject</term>
///		    <term>This script contains the ENTER/(FIXED)UPDATE/EXIT Investigate states</term>
///	    </item>
///	    <item>
///         <term>EnemyObject</term>
///		    <term>Script</term>
///         <term>EnemyAIStateManager (Assets/Scripts/NPCs/Enemies/Behavior-AI/State machine/stateMachines/EnemyAiStateManager.cs)</term>
///		    <term>This script contains variables that are used in this script to manage the state</term>
///	    </item>
///	    <item>
///         <term>EnemyObject</term>
///		    <term>Visual scripting</term>
///         <term>Enemy_Visual_Scripting (Assets/Scripts/NPCs/Enemies/Behavior-AI/State machine/visualScripting/Enemy_Visual_Scripting.asset)</term>
///		    <term>This script need to be added to the EnemyObject with the Enemy_Visual_Scripting</term>
///	    </item>
/// </list>
public class InvestigateState : MonoBehaviour
{
    private EnemyAiStateManager _stateManager; // Reference to the state manager
    private IEnumerator _investigateCoroutine; // a coroutine that is used to wait for a certain amount of time before going back to the patrol state
    private bool _investigateCoroutineIsRunning; // a bool that is used to check if the coroutine is running
    private IEnumerator _waitingAtWaypointDuringInvestigationCoroutine; // a coroutine that is used to wait for a certain amount of time before going to the next investigate waypoint
    private bool _waitingAtWaypointDuringInvestigationCoroutineIsRunning; // a bool that is used to check if the coroutine is running
    private SoundManager.EnemyStateWatcher _enemyStateWatcher;
    private static readonly int InvestigateHash = Animator.StringToHash("Investigate");

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        _stateManager = GetComponent<EnemyAiStateManager>();
        _enemyStateWatcher = GameObject.Find("EnemyStateWatcher").GetComponent<SoundManager.EnemyStateWatcher>();
    }
   
    /// <summary>
    /// Enter investigate state
    /// </summary>
    public void Enter_Investigate()
    {
        _stateManager.navMeshAgent.speed = _stateManager.enemyAiScriptableObject.ToInvestigatePointSpeed;
        //shows the current state as text above the enemy when this is enabled in the inspector.
        if (_stateManager.enemyAiScriptableObject.showCurrentState)
        {
            _stateManager.textMesh.text = "Investigate";
            _stateManager.textMesh.color = Color.magenta;
        }

        if (_stateManager.alertedBySound) _stateManager.CheckPositionReachable(_stateManager.locationOfNoise);
        else if(_stateManager.alertedByVision) _stateManager.CheckPositionReachable(_stateManager.spottedPlayerLastPosition);
        else if(_stateManager.alertedByGuard) _stateManager.CheckPositionReachable(_stateManager.recievedLocationFromGuard);

        GameObject.Find("EnemyStateWatcher").GetComponent<SoundManager.EnemyStateWatcher>().isInvestegating(true);
    }
    
    /// <summary>
    /// Update investigate state
    /// </summary>
    public void Update_Investigate()
    {
        //If the player/brother is in vision, chase the player/brother
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
        //Prevent the enemy from getting stuck in the investigate state
        if (Vector3.Distance(transform.position , _stateManager.navMeshAgent.destination) > 0.5f && !_waitingAtWaypointDuringInvestigationCoroutineIsRunning)
        {
            _stateManager.waitingAtWaypoint = false;
        }
            
        //Check if navmesh agent is still moving
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
                _stateManager.navMeshAgent.speed = _stateManager.defaultSpeed;
                _investigateCoroutineIsRunning = true;
                _investigateCoroutine =
                    _stateManager.CallFunctionAfterSeconds(_stateManager.enemyAiScriptableObject.InvestigateTime, () =>
                    {
                        CustomEvent.Trigger(gameObject, "Patrol");
                        _investigateCoroutineIsRunning = false;
                    });
                StartCoroutine(_investigateCoroutine);
            }
            
            //If the enemy is not waiting at the location, calculate the next location.
            if (!_stateManager.waitingAtWaypoint && !_waitingAtWaypointDuringInvestigationCoroutineIsRunning)
            {
                _stateManager.waitingAtWaypoint = true;
                _waitingAtWaypointDuringInvestigationCoroutineIsRunning = true;
                _waitingAtWaypointDuringInvestigationCoroutine =
                    _stateManager.CallFunctionAfterSeconds(_stateManager.enemyAiScriptableObject.WaitAtInvestigatingWaypointTime, () =>
                    {
                        if (_stateManager.alertedBySound) _stateManager.CalculateInvestigateLocation(_stateManager.locationOfNoise);
                        else if(_stateManager.alertedByVision) _stateManager.CalculateInvestigateLocation(_stateManager.spottedPlayerLastPosition);
                        else if(_stateManager.alertedByGuard) _stateManager.CalculateInvestigateLocation(_stateManager.recievedLocationFromGuard);
                        _waitingAtWaypointDuringInvestigationCoroutineIsRunning = false;
                        
                    });
                StartCoroutine(_waitingAtWaypointDuringInvestigationCoroutine);
            }
            //look around at each waypoint.
            _stateManager.LookAround();
        }
        _stateManager.animator.SetBool(InvestigateHash, _waitingAtWaypointDuringInvestigationCoroutineIsRunning);
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

        _enemyStateWatcher.isInvestegating(false);
        _stateManager.animator.SetBool(InvestigateHash, false);
    }
}
