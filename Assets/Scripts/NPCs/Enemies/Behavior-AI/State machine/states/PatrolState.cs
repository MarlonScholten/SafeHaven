using System;
using System.Collections;
using NPC;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// <br>Author: Hugo Ulfman</br>
/// <br>Modified by: </br>
/// Description: Unity event for when the player/brother sound is detected. This is created so a UnityEvent can pass an argument
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
///		    <term>This script contains the ENTER/(FIXED)UPDATE/EXIT Patrol states</term>
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
public class PatrolState : MonoBehaviour
{
    private EnemyAiStateManager _stateManager; // Reference to the state manager
    private bool _smallSoundReducer; // a bool that checks if the small sound couroutine is running
    private int _numberOfSmallSoundsHeard; // the number of small sounds heard
    private bool _waitingAtWaypointCoroutineIsRunning; // a bool that checks if the waiting at waypoint coroutine is running
    private IEnumerator _waitingAtWaypointCoroutine; // a coroutine that waits at a waypoint
    [NonSerialized] public HeardASoundEvent HeardASoundEvent; // a event that other can call to make the enemy hear a sound

    private bool firstStart = true;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        /*_stateManager = GetComponent<EnemyAiStateManager>();
        HeardASoundEvent ??= new HeardASoundEvent();
        HeardASoundEvent.AddListener(HeardASoundFromPlayer);*/
    }
    /// <summary>
    /// Enter patrol state
    /// </summary>
    public void Enter_Patrol()
    {
        if (firstStart)
        {
            _stateManager = GetComponent<EnemyAiStateManager>();
            _stateManager.HotfixAwake(); //TODO: look at excecution order
            HeardASoundEvent ??= new HeardASoundEvent();
            HeardASoundEvent.AddListener(HeardASoundFromPlayer);
            firstStart = false;
        }
        _stateManager.alertedBySound = false;
        _stateManager.currentWpIndex = GetClosestWaypoint();
        DetermineNextWaypoint();
       // TODO: Play walk animation
    }
    
    /// <summary>
    /// Update patrol state
    /// </summary>
    public void Update_Patrol()
    {
        var path = new NavMeshPath();
        // If the player/brother is in vision but the path is partial, the enemy will go back to the alerted state.
        if (_stateManager.CheckVision())
        {
            _stateManager.navMeshAgent.CalculatePath(_stateManager.spottedPlayerLastPosition, path);
            if (path.status != NavMeshPathStatus.PathPartial)
            {
                CustomEvent.Trigger(gameObject, "Alerted");
            }
        }
        //if the player/brother is alerted by sound but the path is partial, the enemy will go back to the alerted state.
        if (_stateManager.alertedBySound)
        {
            _stateManager.navMeshAgent.CalculatePath(_stateManager.locationOfNoise, path);
            if (path.status != NavMeshPathStatus.PathPartial)
            {
                CustomEvent.Trigger(gameObject, "Alerted");
            }
        }
        // If the enemy heard more than 0 and a certain amount of time has passed, the number of small sounds heard will be decreased by one.
        if (_numberOfSmallSoundsHeard > 0 && !_smallSoundReducer)
        {
            _smallSoundReducer = true; 
            StartCoroutine(_stateManager.CallFunctionAfterSeconds(_stateManager.enemyAiScriptableObject.ReduceSmallSoundsTime, () => { 
                if(_numberOfSmallSoundsHeard > 0) _numberOfSmallSoundsHeard--;
                _smallSoundReducer = false;
            }));
        }
    }
    
    /// <summary>
    /// Fixed update patrol state
    /// </summary>
    public void FixedUpdate_Patrol()
    {
        // If the enemy is waiting at the waypoint, lookAround and determine the next waypoint after investigating the current one.
        if (_stateManager.CheckIfEnemyIsAtWaypoint())
        {
            if (!_waitingAtWaypointCoroutineIsRunning)
            {
                _waitingAtWaypointCoroutineIsRunning = true;
                _stateManager.LookAround();
                _waitingAtWaypointCoroutine = _stateManager.CallFunctionAfterSeconds(_stateManager.enemyAiScriptableObject.WaitAtWaypointTime, () =>
                {
                    DetermineNextWaypoint();
                    _waitingAtWaypointCoroutineIsRunning = false;
                });
                StartCoroutine(_waitingAtWaypointCoroutine);
            }
        }
    }
    /// <summary>
    /// Exit patrol state
    /// </summary>
    public void Exit_Patrol()
    {
        // Stop the patrol coroutine if it is running.
        if(_waitingAtWaypointCoroutineIsRunning)StopCoroutine(_waitingAtWaypointCoroutine);
        _waitingAtWaypointCoroutineIsRunning = false;
        _numberOfSmallSoundsHeard = 0;
    }
    /// <summary>
    /// This method determines the next waypoint based on the index of the current waypoint.
    /// </summary>
    private void DetermineNextWaypoint()
    {
        // If there is a next waypoint, go to that one, otherwise return to the first waypoint.
        _stateManager.currentWpIndex = _stateManager.currentWpIndex == _stateManager.wayPoints.Count - 1 ? 0 : _stateManager.currentWpIndex + 1;
        // Set the next waypoint by using the index.
        _stateManager.targetWpLocation = _stateManager.wayPoints[_stateManager.currentWpIndex].position;
        // Set the destination of the navmesh agent to the next waypoint.
        _stateManager.navMeshAgent.SetDestination(_stateManager.targetWpLocation);
    }
    /// /// <summary>
    /// This method check for the closest waypoint to the enemy.
    /// <returns>The index of that waypoint.</returns>
    /// </summary>
    private int GetClosestWaypoint()
    {
        var closestDistance = Mathf.Infinity;
        var closestIndex = 0;
        // Loop through all the waypoints and check which one is closest to the enemy.
        for (var i = 0; i < _stateManager.wayPoints.Count; i++)
        {
            var distance = Vector3.Distance(transform.position, _stateManager.wayPoints[i].position);
            if (distance >= closestDistance) continue;
            closestDistance = distance;
            closestIndex = i;
        }
        //return the index of the closest waypoint.
        return closestIndex;
    }
    /// /// <summary>
    /// This method can be called by the HeardASoundEvent and checks if the sound is loud enough to react on or add to the small sounds heard.
    /// </summary>
    private void HeardASoundFromPlayer(SoundSource source)
    {
        Debug.Log("HeardASoundFromPlayer" +  _stateManager.enemyAiScriptableObject.ThresholdSmallSounds + source.GetVolume() + _stateManager.enemyAiScriptableObject.ThresholdLoudSounds +" "+ _numberOfSmallSoundsHeard);
        // If the sound is too small, return.
        if(source.GetVolume() <_stateManager.enemyAiScriptableObject.ThresholdSmallSounds) return;
        // If the sound is loud enough, increase the number of small sounds heard.
        if (source.GetVolume() <= _stateManager.enemyAiScriptableObject.ThresholdLoudSounds && source.GetVolume() >= _stateManager.enemyAiScriptableObject.ThresholdSmallSounds) _numberOfSmallSoundsHeard++;
        // If the sound is loud enough, set the location of the noise and set the alertedBySound to true.
        if (_numberOfSmallSoundsHeard >= _stateManager.enemyAiScriptableObject.NumberOfSmallSoundsToInvestigate || source.GetVolume() > _stateManager.enemyAiScriptableObject.ThresholdLoudSounds)
        {
            _numberOfSmallSoundsHeard = 0;
            _stateManager.locationOfNoise = source.GetSource().transform.position;
            _stateManager.alertedBySound = true;
        }
    }
    
}
