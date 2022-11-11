using System;
using System.Collections;
using System.Collections.Generic;
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
    public float threshold = 0.1f;
    
    public int investigateDistance = 3;
    public int waitAtWaypointInSeconds = 4;
    public int waitAtWaypointDuringInvestigatingInSeconds = 2;
    public int investigateTimeInSeconds= 10;
    private bool _investigationTimeIsStarted ;
    private bool _alerted;
    private Vector3 _spottedPlayerLastPosition;
    private GameObject _spottedPlayer;
    private bool _waitingAtWaypoint;

    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }
    
    /// PATROLLING ///
    public void Enter_Patrol()
    { 
        _alerted = false;
        _currentWpIndex = GetClosestWaypoint();
       DetermineNextWaypoint();
       // Play walk animation
    }
    public void Update_Patrol()
    {
        if (checkVision())
        {
            CustomEvent.Trigger(gameObject, "Alerted");
        }
        if (_alerted)
        {
            CustomEvent.Trigger(gameObject, "Alerted");
        }
    }
    public void FixedUpdate_Patrol()
    {
        if (CheckIfEnemyIsAtWaypoint())
        {
            if (_navMeshAgent.isStopped == false)
            {
                LookAround();
                StartCoroutine(CallFunctionAfterSeconds(waitAtWaypointInSeconds, DetermineNextWaypoint));
            }
        }
    }
    public void Exit_Patrol()
    {
    }
    /// ALERTED ///
    public void Enter_Alerted()
    {
        _navMeshAgent.isStopped = true;
        StartCoroutine(CallFunctionAfterSeconds(3, () => { _navMeshAgent.isStopped = false; }));
    }
    public void Update_Alerted()
    {
        // rotate to random directions 
        if (_navMeshAgent.isStopped == false)
        {
            if (_alerted && _noiseMaker.getVolume() > threshold)
            {
                CustomEvent.Trigger(gameObject, "Investigate");
            }
            else if (_alerted && _noiseMaker.getVolume() <= threshold)
            {
                CustomEvent.Trigger(gameObject, "Patrol");
            }
            else if (!_alerted)
            {
                CustomEvent.Trigger(gameObject, "Chasing");
            }
        }
    }
    public void FixedUpdate_Alerted()
    {
    }
    
    public void Exit_Alerted()
    {
  
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
                StartCoroutine(CallFunctionAfterSeconds(investigateTimeInSeconds, () => CustomEvent.Trigger(gameObject, "Patrol")));
            }
            if (!_waitingAtWaypoint)
            {
                _waitingAtWaypoint = true;
                LookAround();
                StartCoroutine(CallFunctionAfterSeconds(waitAtWaypointDuringInvestigatingInSeconds, CalculateInvestigateLocation));
            }
        }
    }
    
    public void Exit_Investigate()
    {
    }
    
    /// CHASING ///
    public void Enter_Chasing()
    {
    }
    public void Update_Chasing()
    {
        if(!checkVision() && Vector3.Distance(transform.position, _spottedPlayerLastPosition) <= 2f)
        {
            CustomEvent.Trigger(gameObject, "Investigate");
            Debug.Log("Investigate");
        }
    }
    public void FixedUpdate_Chasing()
    {
        if (checkVision())
        {
            _navMeshAgent.SetDestination(_spottedPlayer.transform.position);
            Debug.Log("Chasing to Player");
        }
        else
        {
            _navMeshAgent.SetDestination(_spottedPlayerLastPosition);
            Debug.Log("Chasing to Point" + _spottedPlayerLastPosition );
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
        transform.Rotate(0, Random.Range(0, 360), 0);

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
                        return true;
                    }
                }
            }
        }
        return false;
    }
    
    private Collider GetPlayer(Collider[] objects)
    {
        foreach (Collider obj in objects)
        {
            if (obj.gameObject.CompareTag("Player"))
            {
                return obj;
            }
        }
        return null;
    }
    void NoiseReceived(SoundSource source)
    {
        _noiseMaker = source;
        _locationOfNoise = _noiseMaker.source.transform.position;
        _alerted = true;
    }
    private void CalculateInvestigateLocation() {
        Vector3 randDirection = Random.insideUnitSphere * investigateDistance;
        if (_alerted)
        {
            randDirection += _locationOfNoise;
        }
        else
        {
            randDirection += _spottedPlayerLastPosition;
        }
        NavMeshHit navHit;
        NavMesh.SamplePosition (randDirection, out navHit, investigateDistance, -1);
        _targetWpLocation = navHit.position;
        _navMeshAgent.SetDestination(navHit.position);
        _navMeshAgent.isStopped = false;
        _waitingAtWaypoint = false;
        Debug.Log("Investigate Location is " + _targetWpLocation);
    }
    private bool CheckIfEnemyIsAtWaypoint()
    {
        return Vector3.Distance(transform.position, _targetWpLocation) <= 2f;
    }
}
