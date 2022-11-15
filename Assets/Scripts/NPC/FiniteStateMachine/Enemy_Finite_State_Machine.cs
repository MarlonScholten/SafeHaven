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
    public List<Transform> _wayPoints;
    private int _currentWpIndex;
    
    public float visionRange = 10f;
    public float visionAngle = 45f;
    
    private SoundSource _noiseMaker;
    private Vector3 _locationOfNoise;
    public float thresholdSmallSounds = 0.1f;
    public float thresholdLoudSounds = 6f;
    
    public int investigateDistance = 3;
    public int waitAtWaypointTime = 4;
    public int waitAtInvestigatingWaypointTime = 2;
    public int investigateTime= 10;
    
    public int numberOfSmallSoundsToInvestigate = 3;
    public int reduceSmallSoundsTime = 3;
    public int stopWhenAlertedTime = 3;
    private int _numberOfSmallSoundsHeard;
    private bool _smallSoundReducer;
    
    
    private bool _investigationTimeIsStarted ;
    private bool _alertedBySound;
    private bool _alertedByVision;
    private Vector3 _spottedPlayerLastPosition;
    private GameObject _spottedPlayer;
    private bool _waitingAtWaypoint;

    private DateTime inVision;
    
    private IEnumerator _patrolCoroutine;
    private IEnumerator _investigateCoroutine;
    private IEnumerator _waitingAtWaypointCoroutine;
    private IEnumerator _alertedCoroutine;
    
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
       // Play walk animation
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
    }
    public void FixedUpdate_Patrol()
    {
        if (CheckIfEnemyIsAtWaypoint())
        {
            if (_navMeshAgent.isStopped == false)
            {
                LookAround();
                StartCoroutine(CallFunctionAfterSeconds(waitAtWaypointTime, DetermineNextWaypoint));
            }
        }
    }
    public void Exit_Patrol()
    {
        // StopCoroutine(_patrolCoroutine);
    }
    /// ALERTED ///
    public void Enter_Alerted()
    {
        if (_alertedBySound)
        {
            _navMeshAgent.isStopped = true;
            _alertedCoroutine = CallFunctionAfterSeconds(stopWhenAlertedTime, () => { _navMeshAgent.isStopped = false; });
            StartCoroutine(_alertedCoroutine);
        }
    }
    public void Update_Alerted()
    {
    
        // if numberofsoundssmall sounds is not zero then delete lower numberofsmall sounds after 10 seconds
        
        if (_numberOfSmallSoundsHeard > 0 && !_smallSoundReducer)
            {
                _smallSoundReducer = true; 
                StartCoroutine(CallFunctionAfterSeconds(reduceSmallSoundsTime, () => { 
                    _numberOfSmallSoundsHeard--;
                    _smallSoundReducer = false;
                }));
            }


        if (_navMeshAgent.isStopped == false)
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
        if(_alertedBySound)StopCoroutine(_alertedCoroutine);
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
            if (_waitingAtWaypoint && !_investigationTimeIsStarted)
            {
                _investigationTimeIsStarted = true;
                _investigateCoroutine =
                    CallFunctionAfterSeconds(investigateTime, () => CustomEvent.Trigger(gameObject, "Patrol"));
                StartCoroutine(_investigateCoroutine);
            }
            if (!_waitingAtWaypoint)
            {
                _waitingAtWaypoint = true;
               
                _waitingAtWaypointCoroutine =
                    CallFunctionAfterSeconds(waitAtInvestigatingWaypointTime, CalculateInvestigateLocation);
                StartCoroutine(_waitingAtWaypointCoroutine);
            }
            LookAround();
        }
    }
    
    public void Exit_Investigate()
    {
        if(_investigationTimeIsStarted)StopCoroutine(_investigateCoroutine);
        if(_waitingAtWaypoint)StopCoroutine(_waitingAtWaypointCoroutine);
        _investigationTimeIsStarted = false;
    }
    
    /// CHASING ///
    public void Enter_Chasing()
    {
    }
    public void Update_Chasing()
    {
        if(!checkVision() && inVision.AddSeconds(2) < DateTime.Now && Vector3.Distance(transform.position, _spottedPlayerLastPosition) <= 2f)
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
        else if (!checkVision() && inVision.AddSeconds(2) > DateTime.Now)
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
    private int GetClosestWaypoint()
    {
        float closestDistance = Mathf.Infinity;
        int closestIndex = 0;
        for (int i = 0; i < _wayPoints.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, _wayPoints[i].position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }
        return closestIndex;
    }
    private void LookAround()
    {
        _navMeshAgent.isStopped = true;
        // rotate towards random direction smoothly
        // add random rotation to vector 3
        var x = Random.rotation;
        var y = Random.rotation;
        var z = Random.rotation;
        var randomRotation = new Vector3(x.x, Random.Range(transform.position.y - 50, transform.position.y + 50), z.z);
        Quaternion lookRotation = Quaternion.LookRotation((randomRotation- transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 2f * Time.deltaTime);

        // Play look around animation
    }

    private IEnumerator CallFunctionAfterSeconds(int seconds, Action method)
    {
        yield return new WaitForSeconds(seconds);
        method();
        // Play walk animation
    }

    private void DetermineNextWaypoint()
    {
        _currentWpIndex = _currentWpIndex == _wayPoints.Count - 1 ? 0 : _currentWpIndex + 1;
        _targetWpLocation = _wayPoints[_currentWpIndex].position;
        _navMeshAgent.SetDestination(_targetWpLocation);
        _navMeshAgent.isStopped = false;
    }
    

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
                RaycastHit hit;
                if (Physics.Raycast(transform.position, directionToPlayer, out hit, visionRange))
                {
                    if (hit.collider.gameObject.CompareTag("Player"))
                    {
                        var hitPlayer = hit.collider.gameObject;
                        _spottedPlayer = hitPlayer;
                        _spottedPlayerLastPosition = hitPlayer.transform.position;
                        _alertedByVision = true;
                        _alertedBySound = false;
                        inVision = DateTime.Now;
                        return true;
                    }
                }
            }
        }
        return false;
    }
    
    private Collider GetPlayer(Collider[] objects)
    {
        return objects.FirstOrDefault(obj => obj.gameObject.CompareTag("Player"));
    }
    void NoiseReceived(SoundSource source)
    {
        if (source.getVolume() <= thresholdLoudSounds && source.getVolume() >= thresholdSmallSounds)_numberOfSmallSoundsHeard++;
        if (_numberOfSmallSoundsHeard >= numberOfSmallSoundsToInvestigate || source.getVolume() > thresholdLoudSounds)
        {
            _numberOfSmallSoundsHeard = 0;
            _noiseMaker = source;
            _locationOfNoise = _noiseMaker.source.transform.position;
            _alertedBySound = true;
        }
    }
    private void CalculateInvestigateLocation() {
        Vector3 randDirection = Random.insideUnitSphere * investigateDistance;
        if (_alertedBySound) randDirection += _locationOfNoise;
        if(_alertedByVision) randDirection += _spottedPlayerLastPosition;
        
        NavMesh.SamplePosition (randDirection, out var navHit, investigateDistance, 1);
        _targetWpLocation = navHit.position;
        _navMeshAgent.SetDestination(_targetWpLocation);
        _navMeshAgent.isStopped = false;
        _waitingAtWaypoint = false;
    }
    private bool CheckIfEnemyIsAtWaypoint()
    {
        return Vector3.Distance(transform.position, _targetWpLocation) <= 2f;
    }
    
}
