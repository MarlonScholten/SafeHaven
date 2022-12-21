using System;
using System.Collections;
using System.Linq;
using NPC;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// Author: Marlon Kerstens<br/>
/// Modified by: Thomas van den Oever<br/>
/// Description: Unity event for when an enemy is want's to communicate with the other enemy.
/// </summary>
[System.Serializable]
public class StartCommuncicationAlert : UnityEvent<GameObject>
{
}

/// <summary>
/// Author: Hugo Ulfman<br/>
/// Modified by: Iris Giezen <br/>
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
    /// <summary>
    /// Reference to the state manager.
    /// </summary>
    private EnemyAiStateManager _stateManager;

    /// <summary>
    /// A bool that checks if the small sound coroutine is running.
    /// </summary>
    private bool _smallSoundReducer;

    /// <summary>
    /// The number of small sounds heard.
    /// </summary>
    private int _numberOfSmallSoundsHeard;

    /// <summary>
    /// A coroutine that waits at a waypoint.
    /// </summary>
    private IEnumerator _waitingAtWaypointCoroutine;

    /// <summary>
    /// A bool that checks if the waiting at waypoint coroutine is running.
    /// </summary>
    private bool
        _waitingAtWaypointCoroutineIsRunning;

    /// <summary>
    /// A coroutine that is used during the communicate with other enemy.
    /// </summary>
    private IEnumerator
        _communicateWithOtherEnemyCoroutine;

    /// <summary>
    /// A bool that checks if the communicate with other enemy coroutine is running.
    /// </summary>
    private bool
        _communicateWithOtherEnemyCoroutineIsRunning;

    /// <summary>
    /// A coroutine that deletes the enemy object reference.
    /// </summary>
    private IEnumerator
        _timeToForgetCommunicationWithEnemyCoroutine;

    /// <summary>
    /// A bool that checks if the waiting at waypoint coroutine is running.
    /// </summary>
    private bool
        _timeToForgetCommunicationWithEnemyCoroutineIsRunning;

    /// <summary>
    /// A event that other can call to make the enemy hear a sound.
    /// </summary>
    [NonSerialized] public HeardASoundEvent HeardASoundEvent;

    /// <summary>
    /// A event that other can call to make the enemy alert.
    /// </summary>
    [NonSerialized] public AlertEnemyEvent AlertEnemyEvent;

    /// <summary>
    /// A event that other can call to start the communication.
    /// </summary>
    [NonSerialized] private StartCommuncicationAlert _startCommunicationAlert;

    private bool _communicateWithOtherEnemy;
    private GameObject _communicateTowards;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        _stateManager = GetComponent<EnemyAiStateManager>();
        _stateManager.navMeshAgent = GetComponent<NavMeshAgent>();
        HeardASoundEvent ??= new HeardASoundEvent();
        HeardASoundEvent.AddListener(HeardASoundFromPlayer);

        AlertEnemyEvent ??= new AlertEnemyEvent();
        AlertEnemyEvent.AddListener(AlertedByGuard);

        _startCommunicationAlert ??= new StartCommuncicationAlert();
        _startCommunicationAlert.AddListener(StartCommunicating);
    }

    /// <summary>
    /// Enter patrol state
    /// </summary>
    public void Enter_Patrol()
    {
        _stateManager.alertedBySound = false;
        _stateManager.alertedByGuard = false;
        _stateManager.alertedBySound = false;
        if (!_stateManager.isGuard) _stateManager.currentWpIndex = GetClosestWaypoint();
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

        //if the player/brother is alerted by sound but the path is partial, the enemy will go back to the alerted state.
        if (_stateManager.alertedByGuard)
        {
            _stateManager.navMeshAgent.CalculatePath(_stateManager.recievedLocationFromGuard, path);
            if (path.status != NavMeshPathStatus.PathPartial)
            {
                CustomEvent.Trigger(gameObject, "Alerted");
            }
        }

        // If the enemy heard more than 0 and a certain amount of time has passed, the number of small sounds heard will be decreased by one.
        if (_numberOfSmallSoundsHeard > 0 && !_smallSoundReducer)
        {
            _smallSoundReducer = true;
            StartCoroutine(_stateManager.CallFunctionAfterSeconds(
                _stateManager.enemyAiScriptableObject.ReduceSmallSoundsTime, () =>
                {
                    if (_numberOfSmallSoundsHeard > 0) _numberOfSmallSoundsHeard--;
                    _smallSoundReducer = false;
                }));
        }
    }

    /// <summary>
    /// Fixed update patrol state
    /// </summary>
    public void FixedUpdate_Patrol()
    {
        // if the enemy encounters another enemy, face the enemy, wait a certain amount of time and then continue patrol.

        if (!_communicateWithOtherEnemy && _communicateTowards == null) CheckIfOtherEnemyIsInVision();
        if (_communicateWithOtherEnemy && _communicateTowards != null)
            Communicate(_communicateTowards.transform.position);
        // If the enemy is waiting at the waypoint, lookAround and determine the next waypoint after investigating the current one.
        if (_stateManager.CheckIfEnemyIsAtWaypoint())
        {
            if (_stateManager.isGuard && !_waitingAtWaypointCoroutineIsRunning)
            {
                _waitingAtWaypointCoroutineIsRunning = true;
                _stateManager.LookAround();
                _waitingAtWaypointCoroutine = _stateManager.CallFunctionAfterSeconds(
                    _stateManager.enemyAiScriptableObject.WaitAtWaypointTime, () =>
                    {
                        DetermineNextWaypoint();
                        _waitingAtWaypointCoroutineIsRunning = false;
                    });
                StartCoroutine(_waitingAtWaypointCoroutine);
            }

            if (!_stateManager.isGuard)
            {
                if (!_waitingAtWaypointCoroutineIsRunning && HasToWaitAtCurrentWaypoint())
                {
                    _waitingAtWaypointCoroutineIsRunning = true;
                    _stateManager.LookAround();
                    _waitingAtWaypointCoroutine = _stateManager.CallFunctionAfterSeconds(
                        _stateManager.enemyAiScriptableObject.WaitAtWaypointTime, () =>
                        {
                            DetermineNextWaypoint();
                            _waitingAtWaypointCoroutineIsRunning = false;
                        });
                    StartCoroutine(_waitingAtWaypointCoroutine);
                }
            }
        }
        // if the enemy should not wait at the waypoint.
        if (_stateManager.isGuard) return;
        if (!HasToWaitAtCurrentWaypoint() &&
            Vector3.Distance(transform.position, _stateManager.targetWpLocation) <= 3f)
        {
            DetermineNextWaypoint();
        }
    }

    /// <summary>
    /// Exit patrol state
    /// </summary>
    public void Exit_Patrol()
    {
        // Stop the patrol coroutine if it is running.
        if (_waitingAtWaypointCoroutineIsRunning) StopCoroutine(_waitingAtWaypointCoroutine);
        _waitingAtWaypointCoroutineIsRunning = false;

        // Stop the communicate with other enemy coroutine if it is running.
        if (_communicateWithOtherEnemyCoroutineIsRunning) StopCoroutine(_communicateWithOtherEnemyCoroutine);
        _communicateWithOtherEnemyCoroutineIsRunning = false;

        // Stop the time to forget communication with enemy coroutine if it is running.
        if (_timeToForgetCommunicationWithEnemyCoroutineIsRunning)
            StopCoroutine(_timeToForgetCommunicationWithEnemyCoroutine);
        _timeToForgetCommunicationWithEnemyCoroutineIsRunning = false;

        // reset state variables
        _communicateWithOtherEnemyCoroutineIsRunning = false;
        _communicateWithOtherEnemy = false;
        _stateManager.navMeshAgent.isStopped = false;
        _communicateTowards = null;
        _numberOfSmallSoundsHeard = 0;
    }

    /// <summary>
    /// This method determines the next waypoint based on the index of the current waypoint.
    /// </summary>
    private void DetermineNextWaypoint()
    {
        if (_stateManager.isGuard)
        {
            var radius = _stateManager.enemyAiScriptableObject.GuardPatrolRadius;
            var randDirection = Random.insideUnitSphere * radius;
            randDirection += _stateManager.guardWaypoint.gameObject.transform.position;
            NavMesh.SamplePosition(randDirection, out NavMeshHit navHit, radius, 1);
            var targetWpLocation = !navHit.hit ? _stateManager.guardWaypoint.gameObject.transform.position : navHit.position;
            _stateManager.CheckPositionReachable(targetWpLocation);
            _stateManager.waitingAtWaypoint = false;
        }
        else
        {
            // If there is a next waypoint, go to that one, otherwise return to the first waypoint.
            _stateManager.currentWpIndex = _stateManager.currentWpIndex == _stateManager.wayPoints.Count - 1
                ? 0
                : _stateManager.currentWpIndex + 1;
            // Set the next waypoint by using the index.
            _stateManager.targetWpLocation = _stateManager.wayPoints[_stateManager.currentWpIndex].gameObject.transform.position;
            // Set the destination of the navmesh agent to the next waypoint.
            _stateManager.navMeshAgent.SetDestination(_stateManager.targetWpLocation);
        }
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
            var distance = Vector3.Distance(transform.position, _stateManager.wayPoints[i].gameObject.transform.position);
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
        // If the sound is too small, return.
        if (source.GetVolume() < _stateManager.enemyAiScriptableObject.ThresholdSmallSounds) return;
        // If the sound is loud enough, increase the number of small sounds heard.
        if (source.GetVolume() <= _stateManager.enemyAiScriptableObject.ThresholdLoudSounds &&
            source.GetVolume() >= _stateManager.enemyAiScriptableObject.ThresholdSmallSounds)
            _numberOfSmallSoundsHeard++;
        // If the sound is loud enough, set the location of the noise and set the alertedBySound to true.
        if (_numberOfSmallSoundsHeard >= _stateManager.enemyAiScriptableObject.NumberOfSmallSoundsToInvestigate ||
            source.GetVolume() > _stateManager.enemyAiScriptableObject.ThresholdLoudSounds)
        {
            _numberOfSmallSoundsHeard = 0;
            _stateManager.locationOfNoise = source.GetSource().transform.position;
            _stateManager.alertedBySound = true;
        }
    }

    /// <summary>
    /// This method is called when the enemy is alerted by the guard.
    /// <param name="location">The location of the player/brother or a sound.</param>
    /// </summary>
    private void AlertedByGuard(Vector3 location)
    {
        _stateManager.alertedByGuard = true;
        _stateManager.recievedLocationFromGuard = location;
    }

    /// <summary>
    /// This method checks if the player/brother is in the vision of the enemy.
    /// </summary>
    private void CheckIfOtherEnemyIsInVision()
    {
        // check if there is an object with the tag enemy in vision of the enemy
        var colliders =
            Physics.OverlapSphere(transform.position, _stateManager.enemyAiScriptableObject.CommunicationRadius);
        var enemy = colliders.FirstOrDefault(obj => obj.gameObject.CompareTag("Enemy"));
        if (enemy == null) return;
        var direction = enemy.transform.position - transform.position;
        var angle = Vector3.Angle(direction, transform.forward);
        if (angle >= _stateManager.enemyAiScriptableObject.VisionAngle * 2)
            return; // Check if the player is in set vision angle
        if (!Physics.Raycast(transform.position + new Vector3(0f, transform.lossyScale.y / 2, 0f), direction.normalized,
                out var hit, _stateManager.enemyAiScriptableObject.CommunicationRadius)) return;
        if (hit.collider.gameObject != enemy.gameObject) return;
        if (enemy.gameObject == _communicateTowards)
            return; // Check if the enemy is already communicating with the collided enemy
        StartCommunicating(enemy.gameObject);
        enemy.GetComponent<PatrolState>()._startCommunicationAlert.Invoke(gameObject);
    }

    /// <summary>
    /// Start communicating with another enemy.
    /// </summary>
    /// <param name="enemyGameObject">Enemy Game object to communicate with</param>
    private void StartCommunicating(GameObject enemyGameObject)
    {
        _communicateTowards = enemyGameObject;
        _communicateWithOtherEnemy = true;
        _stateManager.navMeshAgent.isStopped = true;
        _communicateWithOtherEnemyCoroutineIsRunning = true;
        _communicateWithOtherEnemyCoroutine = _stateManager.CallFunctionAfterSeconds(
            _stateManager.enemyAiScriptableObject.CommunicationTime, () =>
            {
                _communicateWithOtherEnemyCoroutineIsRunning = false;
                _communicateWithOtherEnemy = false;
                _stateManager.navMeshAgent.isStopped = false;
                _timeToForgetCommunicationWithEnemyCoroutineIsRunning = true;
                _timeToForgetCommunicationWithEnemyCoroutine = _stateManager.CallFunctionAfterSeconds(
                    _stateManager.enemyAiScriptableObject.TimeToForgetCommunicationWithEnemy, () =>
                    {
                        _timeToForgetCommunicationWithEnemyCoroutineIsRunning = false;
                        _communicateTowards = null;
                    });
                StartCoroutine(_timeToForgetCommunicationWithEnemyCoroutine);
            });
        StartCoroutine(_communicateWithOtherEnemyCoroutine);
    }

    /// <summary>
    /// rotate the enemy towards the other enemy
    /// </summary>
    /// <param name="enemyTransform">Position of the enemy</param>
    private void Communicate(Vector3 enemyTransform)
    {
        _stateManager.RotateTowards(enemyTransform);
        // TODO: Play some communication animation or something else.
    }
    /// <summary>
    /// Check on the waypoint object for the isWaitingWayPoint bool.
    /// </summary>
    /// <returns>isWaitingWayPoint bool</returns>
    private bool HasToWaitAtCurrentWaypoint()
    {
        return _stateManager.wayPoints[_stateManager.currentWpIndex].GetComponent<EnemyWayPointController>()
            .isWaitingWayPoint;
    }
}