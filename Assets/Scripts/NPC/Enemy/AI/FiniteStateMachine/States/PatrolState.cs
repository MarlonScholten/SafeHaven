using System;
using System.Collections;
using System.Collections.Generic;
using NPC;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// PatrolState functions
/// </summary>
public class PatrolState : MonoBehaviour
{
    private EnemyAiStateManager _stateManager;
    private bool _smallSoundReducer;
    private int _numberOfSmallSoundsHeard;
    private bool _waitingAtWaypointCoroutineIsRunning;
    private IEnumerator _patrolCoroutine;
    private bool _patrolCoroutineIsRunning;
    private FSM_Scriptable_Object _fsmScriptableObject;
    [NonSerialized] public HeardASoundEvent HeardASoundEvent;

    
    private void Awake()
    {
        _stateManager = GetComponent<EnemyAiStateManager>();
        _fsmScriptableObject = _stateManager.enemyAiScriptableObject;
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
        if (_stateManager.CheckVision())
        {
            _stateManager.navMeshAgent.CalculatePath(_stateManager.spottedPlayerLastPosition, path);
            if (path.status != NavMeshPathStatus.PathPartial)
            {
                CustomEvent.Trigger(gameObject, "Alerted");
            }
        }
        if (_stateManager.alertedBySound)
        {
            _stateManager.navMeshAgent.CalculatePath(_stateManager.locationOfNoise, path);
            if (path.status != NavMeshPathStatus.PathPartial)
            {
                CustomEvent.Trigger(gameObject, "Alerted");
            }
        }
        if (_numberOfSmallSoundsHeard > 0 && !_smallSoundReducer)
        {
            _smallSoundReducer = true; 
            StartCoroutine(_stateManager.CallFunctionAfterSeconds(_fsmScriptableObject.reduceSmallSoundsTime, () => { 
                _numberOfSmallSoundsHeard--;
                _smallSoundReducer = false;
            }));
        }
    }
    
    /// <summary>
    /// Fixed update patrol state
    /// </summary>
    public void FixedUpdate_Patrol()
    {
        if (_stateManager.CheckIfEnemyIsAtWaypoint())
        {
            if (!_waitingAtWaypointCoroutineIsRunning)
            {
                _waitingAtWaypointCoroutineIsRunning = true;
                _stateManager.LookAround();
                _patrolCoroutine = _stateManager.CallFunctionAfterSeconds(_fsmScriptableObject.waitAtWaypointTime, () =>
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
        if(_waitingAtWaypointCoroutineIsRunning)StopCoroutine(_patrolCoroutine);
        _waitingAtWaypointCoroutineIsRunning = false;
    }
    /// <summary>
    /// This method determines the next waypoint based on the index of the current waypoint.
    /// </summary>
    private void DetermineNextWaypoint()
    {
        _stateManager.currentWpIndex = _stateManager.currentWpIndex == _stateManager.wayPoints.Count - 1 ? 0 : _stateManager.currentWpIndex + 1;
        _stateManager.targetWpLocation = _stateManager.wayPoints[_stateManager.currentWpIndex].position;
        _stateManager.navMeshAgent.SetDestination(_stateManager.targetWpLocation);
    }
    /// /// <summary>
    /// This method check for the closest waypoint to the enemy and returns the index of that waypoint.
    /// </summary>
    private int GetClosestWaypoint()
    {
        var closestDistance = Mathf.Infinity;
        var closestIndex = 0;
        for (var i = 0; i < _stateManager.wayPoints.Count; i++)
        {
            var distance = Vector3.Distance(transform.position, _stateManager.wayPoints[i].position);
            if (distance >= closestDistance) continue;
            closestDistance = distance;
            closestIndex = i;
        }
        return closestIndex;
    }
    
    private void HeardASoundFromPlayer(SoundSource source)
    {
        if(source.GetVolume() <_fsmScriptableObject.thresholdSmallSounds) return;
        if (source.GetVolume() <= _fsmScriptableObject.thresholdLoudSounds && source.GetVolume() >= _fsmScriptableObject.thresholdSmallSounds) _numberOfSmallSoundsHeard++;
        if (_numberOfSmallSoundsHeard >= _fsmScriptableObject.numberOfSmallSoundsToInvestigate || source.GetVolume() > _fsmScriptableObject.thresholdLoudSounds)
        {
            _numberOfSmallSoundsHeard = 0;
            _stateManager.locationOfNoise = source.GetSource().transform.position;
            _stateManager.alertedBySound= true; 
        }
    }
    
}
