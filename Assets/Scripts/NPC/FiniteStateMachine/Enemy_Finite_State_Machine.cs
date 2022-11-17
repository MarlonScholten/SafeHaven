using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using NPC;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Object = System.Object;
using Random = UnityEngine.Random;


/// <summary>
/// Unity event for when the player sound is detected
/// </summary>
[System.Serializable]
public class HeardASoundEvent : UnityEvent<SoundSource>
{
}

/// <summary>
/// Enemy AI that uses a state machine to control its behavior.
/// </summary>
public class Enemy_Finite_State_Machine : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Vector3 _targetWpLocation;
    [SerializeField]
    private List<Transform> wayPoints;
    private int _currentWpIndex;
    [SerializeField]
    private float visionRange = 10f;
    [SerializeField]
    private float visionAngle = 45f;
    
    private Vector3 _locationOfNoise;
    [SerializeField]
    private float thresholdSmallSounds = 0.1f;
    [SerializeField]
    private float thresholdLoudSounds = 6f;
    
    [SerializeField]
    private int investigateDistance = 3;
    [SerializeField]
    private int waitAtWaypointTime = 4;
    [SerializeField]
    private int waitAtInvestigatingWaypointTime = 2;
    [SerializeField]
    private int investigateTime= 10;
    
    [SerializeField]
    private int numberOfSmallSoundsToInvestigate = 3;
    [SerializeField]
    private int reduceSmallSoundsTime = 3;
    [SerializeField]
    private int stopWhenAlertedTime = 3;
    private int _numberOfSmallSoundsHeard;
    private bool _smallSoundReducer;
    
    
    private bool _investigationTimeIsStarted ;
    private bool _alertedBySound;
    private bool _alertedByVision;
    private Vector3 _spottedPlayerLastPosition;
    private GameObject _spottedPlayer;
    private bool _waitingAtWaypoint;

    private float _timePlayerLastSpotted;
    
    private IEnumerator _patrolCoroutine;
    private IEnumerator _investigateCoroutine;
    private IEnumerator _waitingAtWaypointDuringInvestigationCoroutine;
    private IEnumerator _alertedCoroutine;
    private bool _waitingAtWaypointCoroutineIsRunning;
    private bool _patrolCoroutineIsRunning;
    private bool _waitingAtWaypointDuringInvestigationCoroutineIsRunning;
    private bool _investigateCoroutineIsRunning;
    private bool _alertedCoroutineIsRunning;

    public HeardASoundEvent HeardASoundEvent;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        HeardASoundEvent ??= new HeardASoundEvent();
        HeardASoundEvent.AddListener(HeardASoundFromPlayer);
    }
    
    /// PATROLLING ///
    /// <summary>
    /// Enter patrol state
    /// </summary>
    public void Enter_Patrol()
    { 
        _alertedBySound = false;
        _currentWpIndex = GetClosestWaypoint();
      
        DetermineNextWaypoint();
       // TODO: Play walk animation
    }
    
    /// <summary>
    /// Update patrol state
    /// </summary>
    public void Update_Patrol()
    {
        var path = new NavMeshPath();
        if (CheckVision())
        {
            _navMeshAgent.CalculatePath(_spottedPlayerLastPosition, path);
            if (path.status != NavMeshPathStatus.PathPartial)
            {
                CustomEvent.Trigger(gameObject, "Alerted");
            }
        }
        if (_alertedBySound)
        {
            _navMeshAgent.CalculatePath(_locationOfNoise, path);
            if (path.status != NavMeshPathStatus.PathPartial)
            {
                CustomEvent.Trigger(gameObject, "Alerted");
            }
        }
        if (_numberOfSmallSoundsHeard > 0 && !_smallSoundReducer)
        {
            _smallSoundReducer = true; 
            StartCoroutine(CallFunctionAfterSeconds(reduceSmallSoundsTime, () => { 
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
        if (CheckIfEnemyIsAtWaypoint())
        {
            if (!_waitingAtWaypointCoroutineIsRunning)
            {
                _waitingAtWaypointCoroutineIsRunning = true;
                LookAround();
                _patrolCoroutine = CallFunctionAfterSeconds(waitAtWaypointTime, () =>
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
    /// ALERTED ///
    
    /// <summary>
    /// Enter alerted state
    /// </summary>
    public void Enter_Alerted()
    {
        if (_alertedBySound)
        {
            _navMeshAgent.isStopped = true;
            _alertedCoroutineIsRunning = true;
            _alertedCoroutine = CallFunctionAfterSeconds(stopWhenAlertedTime, () =>
            {
                _navMeshAgent.isStopped = false; 
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
        if (_alertedBySound)
        {
            CustomEvent.Trigger(gameObject, "Investigate");
        }
        else if (_alertedByVision)
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
        LookAround();
    }
    
    /// <summary>
    /// Exit alerted state
    /// </summary>
    public void Exit_Alerted()
    {
        if(_alertedCoroutineIsRunning)StopCoroutine(_alertedCoroutine);
        _alertedCoroutineIsRunning = false;
    }
    
    /// INVESTIGATE ///
    
    /// <summary>
    /// Enter investigate state
    /// </summary>
    public void Enter_Investigate()
    {
        CalculateInvestigateLocation();
    }
    
    /// <summary>
    /// Update investigate state
    /// </summary>
    public void Update_Investigate()
    {
        if (CheckVision())
        {
            CustomEvent.Trigger(gameObject, "Chasing");
        }

    }
    
    /// <summary>
    /// FIxed update investigate state
    /// </summary>
    public void FixedUpdate_Investigate()
    {
        if (CheckIfEnemyIsAtWaypoint())
        {
            if (_waitingAtWaypoint && !_investigateCoroutineIsRunning)
            {
                _investigateCoroutineIsRunning = true;
                _investigateCoroutine =
                    CallFunctionAfterSeconds(investigateTime, () =>
                    {
                        CustomEvent.Trigger(gameObject, "Patrol");
                        _investigateCoroutineIsRunning = false;
                    });
                StartCoroutine(_investigateCoroutine);
            }
            if (!_waitingAtWaypoint)
            {
                _waitingAtWaypoint = true;
                _waitingAtWaypointDuringInvestigationCoroutineIsRunning = true;
                _waitingAtWaypointDuringInvestigationCoroutine =
                    CallFunctionAfterSeconds(waitAtInvestigatingWaypointTime, () =>
                    {
                        CalculateInvestigateLocation();
                        _waitingAtWaypointDuringInvestigationCoroutineIsRunning = false;
                        
                    });
                StartCoroutine(_waitingAtWaypointDuringInvestigationCoroutine);
            }
            LookAround();
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
    
    /// CHASING ///
    
    /// <summary>
    /// Enter chasing state
    /// </summary>
    public void Enter_Chasing()
    {
    }
    
    /// <summary>
    /// Update chasing state
    /// </summary>
    public void Update_Chasing()
    {
        if(!CheckVision() && (_timePlayerLastSpotted + 2) < Time.time || Vector3.Distance(transform.position, _spottedPlayerLastPosition) <= 2f)
        {
            CustomEvent.Trigger(gameObject, "Investigate");
        }
    }
    
    /// <summary>
    /// Fixed update chasing state
    /// </summary>
    public void FixedUpdate_Chasing()
    {
        if (CheckVision())
        {
            _navMeshAgent.SetDestination(_spottedPlayer.transform.position);
        } 
        else if (!CheckVision() && (_timePlayerLastSpotted + 2) > Time.time)
        {
            
            _spottedPlayerLastPosition = _spottedPlayer.transform.position;;
            CalculateInvestigateLocation();
        }
        else 
        {
            _navMeshAgent.SetDestination(_spottedPlayerLastPosition);
        }
        
    }
    
    /// <summary>
    /// Exit chasing state
    /// </summary>
    public void Exit_Chasing()
    {
    }
    
    
    /// METHODS ///
    /// /// <summary>
    /// This method check for the closest waypoint to the enemy and returns the index of that waypoint.
    /// </summary>
    private int GetClosestWaypoint()
    {
        var closestDistance = Mathf.Infinity;
        var closestIndex = 0;
        for (var i = 0; i < wayPoints.Count; i++)
        {
            var distance = Vector3.Distance(transform.position, wayPoints[i].position);
            if (!(distance < closestDistance)) continue;
            closestDistance = distance;
            closestIndex = i;
        }
        return closestIndex;
    }
    /// <summary>
    /// This method rotates the enemy to a random direction.
    /// </summary>
    private void LookAround()
    {
        var x = Random.rotation;
        var z = Random.rotation;
        var position = transform.position;
        var randomRotation = new Vector3(x.x, Random.Range(position.y - 50, position.y + 50), z.z);
        var lookRotation = Quaternion.LookRotation((randomRotation- position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 2f * Time.deltaTime);

        // TODO: Play look around animation
    }

    private void HeardASoundFromPlayer(SoundSource source)
    {
        if (source.GetVolume() <= thresholdLoudSounds && source.GetVolume() >= thresholdSmallSounds)_numberOfSmallSoundsHeard++;
        if (_numberOfSmallSoundsHeard >= numberOfSmallSoundsToInvestigate || source.GetVolume() > thresholdLoudSounds)
        {
            _numberOfSmallSoundsHeard = 0;
            _locationOfNoise = source.GetSource().transform.position;
            _alertedBySound= true; 
        }
    }

    /// <summary>
    /// This method calls a method after a certain amount of seconds.
    /// <param name="seconds">Amount of seconds that takes to call the method.</param>
    /// <param name="method">The method that need to be called after the amount of seconds.</param>
    /// </summary>   
    private static IEnumerator CallFunctionAfterSeconds(int seconds, Action method)
    {
        yield return new WaitForSeconds(seconds);
        method();
        // TODO: Play walk animation
    }

    /// <summary>
    /// This method determines the next waypoint based on the index of the current waypoint.
    /// </summary>
    private void DetermineNextWaypoint()
    {
        _currentWpIndex = _currentWpIndex == wayPoints.Count - 1 ? 0 : _currentWpIndex + 1;
        _targetWpLocation = wayPoints[_currentWpIndex].position;
        _navMeshAgent.SetDestination(_targetWpLocation);
    }
    
    /// <summary>
    /// This method checks if the player is in the vision of the enemy.
    /// </summary>
    private bool CheckVision()
    {
        var foundObjects = Physics.OverlapSphere(transform.position, visionRange);
        var player = GetPlayer(foundObjects);
        if (player == null) return false;
        
        var transform1 = transform;
        var directionToPlayer = player.transform.position - transform1.position;
        var angleToPlayer = Vector3.Angle(transform1.forward, directionToPlayer);
        
        if (!(angleToPlayer < visionAngle)) return false;
        if (!Physics.Raycast(transform.position, directionToPlayer, out var hit, visionRange)) return false;
        
        if (!hit.collider.gameObject.CompareTag("Player")) return false;
        
        var hitPlayer = hit.collider.gameObject;
        _spottedPlayer = hitPlayer;
        _spottedPlayerLastPosition = hitPlayer.transform.position;
        _alertedByVision = true;
        _alertedBySound = false;
        _timePlayerLastSpotted = Time.time;
        return true;
    }
    
    /// <summary>
    /// This method check if the player is in the list of colliders.
    /// <param name="objects">Collided objects.</param>
    /// <returns>The player collider if exists.</returns>
    /// </summary>
    private static Collider GetPlayer(IEnumerable<Collider> objects)
    {
        return objects.FirstOrDefault(obj => obj.gameObject.CompareTag("Player"));
    }
    /// <summary>
    /// This method is called by another script to alert the enemy.
    /// This method raised the number of small sounds heard if it receives a small sound.
    /// When the number of small sounds heard is higher or equal than the numberOfSmallSoundsToInvestigate, the enemy will investigate.
    /// When the sound is a louder than thresholdLoudSounds, the enemy will investigate.
    /// <param name="source">The SoundSource object of the played sound.</param>
    /// </summary>
    private void NoiseReceived(SoundSource source)
    {
        if (source.GetVolume() <= thresholdLoudSounds && source.GetVolume() >= thresholdSmallSounds)_numberOfSmallSoundsHeard++;
        if (_numberOfSmallSoundsHeard >= numberOfSmallSoundsToInvestigate || source.GetVolume() > thresholdLoudSounds)
        {
            _numberOfSmallSoundsHeard = 0;
            _locationOfNoise = source.GetSource().transform.position;
            _alertedBySound= true; 
        }
    }
    /// <summary>
    /// This method calculates the location where the enemy will investigate.
    /// The NavMeshSamplePosition method is used to find a closest point on the NavMesh to the location of the noise.
    /// </summary>
    private void CalculateInvestigateLocation() {
        var randDirection = Random.insideUnitSphere * investigateDistance;
        if (_alertedBySound) randDirection += _locationOfNoise;
        else if(_alertedByVision) randDirection += _spottedPlayerLastPosition;
        NavMesh.SamplePosition (randDirection, out NavMeshHit navHit, investigateDistance, 1);
        _targetWpLocation = navHit.position;
        _navMeshAgent.SetDestination(_targetWpLocation);
        _waitingAtWaypoint = false;
    }
    /// <summary>
    /// This method checks if the enemy is at the waypoint.
    /// </summary>
    private bool CheckIfEnemyIsAtWaypoint()
    {
        return Vector3.Distance(transform.position, _targetWpLocation) <= 2f;
    }
    
}
