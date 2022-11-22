using System;
using System.Collections;
using NPC;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This script is a the Patrol state of the enemy.
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
///		    <term>This script contains the ENTER/(FIXED)UPDATE/EXIT Patrol states</term>
///	    </item>
/// </list>
public class PatrolState : MonoBehaviour
{
    private EnemyAiStateManager _stateManager;
    private bool _smallSoundReducer;
    private int _numberOfSmallSoundsHeard;
    private bool _waitingAtWaypointCoroutineIsRunning;
    private IEnumerator _patrolCoroutine;
    private bool _patrolCoroutineIsRunning;
    [NonSerialized] public HeardASoundEvent HeardASoundEvent;

    
    private void Awake()
    {
        _stateManager = GetComponent<EnemyAiStateManager>();
        HeardASoundEvent ??= new HeardASoundEvent();
        HeardASoundEvent.AddListener(HeardASoundFromPlayer);
    }
    /// <summary>
    /// Enter patrol state
    /// </summary>
    public void Enter_Patrol()
    { 
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
        // If the player is in vision but the path is partial, the enemy will go back to the alerted state.
        if (_stateManager.CheckVision())
        {
            _stateManager.navMeshAgent.CalculatePath(_stateManager.spottedPlayerLastPosition, path);
            if (path.status != NavMeshPathStatus.PathPartial)
            {
                CustomEvent.Trigger(gameObject, "Alerted");
            }
        }
        //if the player is alerted by sound but the path is partial, the enemy will go back to the alerted state.
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
            StartCoroutine(_stateManager.CallFunctionAfterSeconds(_stateManager.enemyAiScriptableObject.reduceSmallSoundsTime, () => { 
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
                _patrolCoroutine = _stateManager.CallFunctionAfterSeconds(_stateManager.enemyAiScriptableObject.waitAtWaypointTime, () =>
                {
                    DetermineNextWaypoint();
                    _waitingAtWaypointCoroutineIsRunning = false;
                });
                StartCoroutine(_patrolCoroutine);
            }
        }
    }
    /// <summary>
    /// Exit patrol state
    /// </summary>
    public void Exit_Patrol()
    {
        // Stop the patrol coroutine if it is running.
        if(_waitingAtWaypointCoroutineIsRunning)StopCoroutine(_patrolCoroutine);
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
        Debug.Log("HeardASoundFromPlayer" +  _stateManager.enemyAiScriptableObject.thresholdSmallSounds + source.GetVolume() + _stateManager.enemyAiScriptableObject.thresholdLoudSounds +" "+ _numberOfSmallSoundsHeard);
        // If the sound is too small, return.
        if(source.GetVolume() <_stateManager.enemyAiScriptableObject.thresholdSmallSounds) return;
        // If the sound is loud enough, increase the number of small sounds heard.
        if (source.GetVolume() <= _stateManager.enemyAiScriptableObject.thresholdLoudSounds && source.GetVolume() >= _stateManager.enemyAiScriptableObject.thresholdSmallSounds) _numberOfSmallSoundsHeard++;
        // If the sound is loud enough, set the location of the noise and set the alertedBySound to true.
        if (_numberOfSmallSoundsHeard >= _stateManager.enemyAiScriptableObject.numberOfSmallSoundsToInvestigate || source.GetVolume() > _stateManager.enemyAiScriptableObject.thresholdLoudSounds)
        {
            _numberOfSmallSoundsHeard = 0;
            _stateManager.locationOfNoise = source.GetSource().transform.position;
            _stateManager.alertedBySound = true;
        }
    }
    
}
