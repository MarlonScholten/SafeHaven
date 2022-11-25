using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        Enter_Walking();
    }

    private void Update()
    {
        Update_Walking();
    }

    /// <summary>
    /// Enter walking state
    /// </summary>
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