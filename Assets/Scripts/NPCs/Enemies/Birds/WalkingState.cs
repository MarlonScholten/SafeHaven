using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PathCreation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class WalkingState : MonoBehaviour
{

    private NavMeshAgent _navMeshAgent;
    private Vector3 startPoint;
    private Vector3 targetWpLocation;
    private bool isWaiting;
    public bool alerted;
    private bool _isOnPathUp;
    private bool _isOnPathDown;
    private float _distanceTravelled;
    private bool _waitingCoroutine;
    private EndOfPathInstruction _endOfPathInstruction = EndOfPathInstruction.Stop;
    [SerializeField] private PathCreator pathUp;
    [SerializeField] private PathCreator pathDown;
    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        Enter_Walking();
    }

    public void FixedUpdate()
    {
        if(!alerted && !_isOnPathUp && !_isOnPathDown) Update_Walking();
        if (alerted && !_isOnPathUp)
        {
            _navMeshAgent.SetDestination(pathUp.path.GetPoint(0));
            if (Vector3.Distance(transform.position, pathUp.path.GetPoint(0)) < 1f)
            {
                _isOnPathUp = true;
                _navMeshAgent.enabled = false;
            }
        }
        if(alerted && _isOnPathUp){
            TravelPath(pathUp);
            if (_distanceTravelled >= pathUp.path.length)
            {
                if (!_waitingCoroutine)
                {
                    _waitingCoroutine = true;
                    StartCoroutine(CallFunctionAfterSeconds(2, () =>
                    {
                        _distanceTravelled = 0;
                        alerted = false; 
                        _isOnPathUp = false;
                        _isOnPathDown = true;
                        _waitingCoroutine = false;
                    }));
                }
         
            }
        }
        if(!alerted && _isOnPathDown){
            TravelPath(pathDown);
            if (_distanceTravelled >= pathDown.path.length)
            {
                _navMeshAgent.enabled = true;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(transform.position, out hit, 5f, NavMesh.AllAreas))
                { 
                    _navMeshAgent.SetDestination(hit.position);
                    targetWpLocation = hit.position;
                    _isOnPathDown = false;
                }
                _distanceTravelled = 0;
            }
        }
    }
    
    private void TravelPath(PathCreator path){
        _distanceTravelled += 5 * Time.deltaTime;
        transform.position = path.path.GetPointAtDistance(_distanceTravelled, _endOfPathInstruction);
        transform.rotation = path.path.GetRotationAtDistance(_distanceTravelled, _endOfPathInstruction);
    }
    
    IEnumerator CallFunctionAfterSeconds(float seconds, Action function)
    {
        yield return new WaitForSeconds(seconds);
        function();
    }
    
    
    // _navMeshAgent.enabled = true;
    // NavMeshHit hit;
    //     if (NavMesh.SamplePosition(transform.position, out hit, 1.0f, NavMesh.AllAreas))
    // { 
    //     _navMeshAgent.SetDestination(hit.position);
    //     xxx = false;
    //     _isOnPathUp = false;
    // }

    /// <summary>
    /// Enter walking state
    /// </summary>
    /// 
    public void Enter_Walking()
    {
        startPoint = transform.position;
        CalculateNextWalkableWaypoint(startPoint);
    }
    
    /// <summary>
    /// Update walking state
    /// </summary>
    public void Update_Walking()
    {
        if (CheckIfIsAtWaypoint() && !isWaiting)
        {
            CalculateNextWalkableWaypoint(startPoint);
        }

    }
    
    /// <summary>
    /// Fixed Update walking state
    /// </summary>
    public void FixedUpdate_Walking()
    {
    }
    
    /// <summary>
    /// Exit walking state
    /// </summary>
    public void Exit_Walking()
    {
    }
    
    public void CalculateNextWalkableWaypoint(Vector3 position) {
        var randDirection = Random.insideUnitSphere * 10;
        randDirection += position;
        NavMesh.SamplePosition (randDirection, out NavMeshHit navHit, 10, 1);
        var path = new NavMeshPath();
        _navMeshAgent.CalculatePath(navHit.position, path);
        if (path.status == NavMeshPathStatus.PathPartial)
        {
            targetWpLocation = path.corners.Last();
            _navMeshAgent.SetDestination(path.corners.Last());
            StartCoroutine(RotateToNextPosition());
        }
        else
        {
            targetWpLocation = navHit.position;
            _navMeshAgent.SetDestination(navHit.position);
            StartCoroutine(RotateToNextPosition());
        }
    }
    
    private IEnumerator RotateToNextPosition()
    {
        _navMeshAgent.isStopped = true;
        // check if the agent is rotated to the next position
        
        float dot = Vector3.Dot(transform.forward, (targetWpLocation - transform.position).normalized);
        while (dot < 0.9f)
        {
            // rotate the agent to the next position
            dot = Vector3.Dot(transform.forward, (targetWpLocation - transform.position).normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetWpLocation - transform.position), 3 * Time.deltaTime);
            // rotate towards the target
            yield return null;
        }
        
        _navMeshAgent.isStopped = false;
        isWaiting = false;
    }


    /// <summary>
    /// This method checks if the enemy is at the waypoint.
    /// </summary>
    public bool CheckIfIsAtWaypoint()
    {
        return Vector3.Distance(transform.position, targetWpLocation) <= 2f;
    }
    
    /// <summary>
    /// This method checks if the enemy can reach the player/brother position.
    /// </summary>
    public void CheckPlayerPositionReachable(Vector3 playerPosition)
    {
    }

}