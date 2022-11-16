using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using NPC;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;
using Random = UnityEngine.Random;

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

    private DateTime _inVision;
    
    private IEnumerator _patrolCoroutine;
    private IEnumerator _investigateCoroutine;
    private IEnumerator _waitingAtWaypointDuringInvestigationCoroutine;
    private IEnumerator _alertedCoroutine;
    private bool _waitingAtWaypointCoroutineIsRunning;
    private bool _patrolCoroutineIsRunning;
    private bool _waitingAtWaypointDuringInvestigationCoroutineIsRunning;
    private bool _investigateCoroutineIsRunning;
    private bool _alertedCoroutineIsRunning;
    

    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }
    
    /// PATROLLING ///
    public void Enter_Patrol()
    { 
        _alertedBySound = false;
        _currentWpIndex = GetClosestWaypoint();
      
        DetermineNextWaypoint();
       // TODO: Play walk animation
    }
    public void Update_Patrol()
    {
        NavMeshPath path = new NavMeshPath();
        if (checkVision())
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
    public void Exit_Patrol()
    {
        if(_waitingAtWaypointCoroutineIsRunning)StopCoroutine(_patrolCoroutine);
        _waitingAtWaypointCoroutineIsRunning = false;
    }
    /// ALERTED ///
    // generate documentation
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
    public void Update_Alerted()
    {
        if (_alertedCoroutineIsRunning == false)
        {
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
    }
    public void FixedUpdate_Alerted()
    {
        // rotate to random directions 
    }
    
    public void Exit_Alerted()
    {
        if(_alertedCoroutineIsRunning)StopCoroutine(_alertedCoroutine);
        _alertedCoroutineIsRunning = false;
    }
    
    /// INVESTIGATE ///
    public void Enter_Investigate()
    {
        CalculateInvestigateLocation();
    }
    public void Update_Investigate()
    {
        if (checkVision())
        {
            CustomEvent.Trigger(gameObject, "Chasing");
        }

    }
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
    
    public void Exit_Investigate()
    {
        if(_investigateCoroutineIsRunning)StopCoroutine(_investigateCoroutine);
        _investigateCoroutineIsRunning = false;
        if(_waitingAtWaypointDuringInvestigationCoroutineIsRunning)StopCoroutine(_waitingAtWaypointDuringInvestigationCoroutine);
        _waitingAtWaypointDuringInvestigationCoroutineIsRunning = false;
    }
    
    /// CHASING ///
    public void Enter_Chasing()
    {
    }
    public void Update_Chasing()
    {
        if(!checkVision() && _inVision.AddSeconds(2) < DateTime.Now && Vector3.Distance(transform.position, _spottedPlayerLastPosition) <= 2f)
        {
            CustomEvent.Trigger(gameObject, "Investigate");
        }
    }
    public void FixedUpdate_Chasing()
    {
        if (checkVision())
        {
            _navMeshAgent.SetDestination(_spottedPlayer.transform.position);
        } 
        else if (!checkVision() && _inVision.AddSeconds(2) > DateTime.Now)
        {
            
            _spottedPlayerLastPosition = _spottedPlayer.transform.position;;
            CalculateInvestigateLocation();
        }
        else 
        {
            _navMeshAgent.SetDestination(_spottedPlayerLastPosition);
        }
        
    }
    
    public void Exit_Chasing()
    {
    }
    
    
    /// METHODS ///
    /// /// <summary>
    /// This method check for the closest waypoint to the enemy and returns the index of that waypoint.
    /// </summary>
    private int GetClosestWaypoint()
    {
        float closestDistance = Mathf.Infinity;
        int closestIndex = 0;
        for (var i = 0; i < wayPoints.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, wayPoints[i].position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
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
        var randomRotation = new Vector3(x.x, Random.Range(transform.position.y - 50, transform.position.y + 50), z.z);
        Quaternion lookRotation = Quaternion.LookRotation((randomRotation- transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 2f * Time.deltaTime);

        // TODO: Play look around animation
    }

    /// <summary>
    /// This method calls a method after a certain amount of seconds.
    /// <param name="seconds">Amount of seconds that takes to call the method.</param>
    /// <param name="method">The method that need to be called after the amount of seconds.</param>
    /// </summary>
    private IEnumerator CallFunctionAfterSeconds(int seconds, Action method)
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
    private bool checkVision()
    {
        Collider[] foundObjects = Physics.OverlapSphere(transform.position, visionRange);
        Collider player = GetPlayer(foundObjects);
        if (player != null)
        {
            Vector3 directionToPlayer = player.transform.position - transform.position;
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            if (angleToPlayer < visionAngle)
            {
                if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, visionRange))
                {
                    if (hit.collider.gameObject.CompareTag("Player"))
                    {
                        var hitPlayer = hit.collider.gameObject;
                        _spottedPlayer = hitPlayer;
                        _spottedPlayerLastPosition = hitPlayer.transform.position;
                        _alertedByVision = true;
                        _alertedBySound = false;
                        _inVision = DateTime.Now;
                        return true;
                    }
                }
            }
        }
        return false;
    }
    
    /// <summary>
    /// This method check if the player is in the list of colliders.
    /// <param name="objects">Collided objects.</param>
    /// <returns>The player collider if exists.</returns>
    /// </summary>
    private Collider GetPlayer(Collider[] objects)
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
    void NoiseReceived(SoundSource source)
    {
        if (source.getVolume() <= thresholdLoudSounds && source.getVolume() >= thresholdSmallSounds)_numberOfSmallSoundsHeard++;
        if (_numberOfSmallSoundsHeard >= numberOfSmallSoundsToInvestigate || source.getVolume() > thresholdLoudSounds)
        {
            _numberOfSmallSoundsHeard = 0;
            _locationOfNoise = source.source.transform.position;
            _alertedBySound = true; 
        }
    }
    /// <summary>
    /// This method calculates the location where the enemy will investigate.
    /// The NavMeshSamplePosition method is used to find a closest point on the NavMesh to the location of the noise.
    /// </summary>
    private void CalculateInvestigateLocation() {
        Vector3 randDirection = Random.insideUnitSphere * investigateDistance;
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
