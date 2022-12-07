using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class WalkingState : MonoBehaviour
{
    private BirdStateManager _birdStateManager;
    private Vector3 startPoint;
    private Vector3 targetWpLocation;
    private bool _isRotating;
    private IEnumerator _rotateCoroutine;
    private bool _rotateCoroutineIsRunning;
    public void Awake()
    {
        _birdStateManager = GetComponent<BirdStateManager>();
    }

    public void ENTER_WALKING_STATE()
    {
        startPoint = transform.position;
        CalculateNextWalkableWaypoint(startPoint);
    }
    
    public void UPDATE_WALKING_STATE()
    {
        if (_birdStateManager.CheckIfAlertingObjectsAreNearby(_birdStateManager.birdScriptableObject.AlertTags)){CustomEvent.Trigger(gameObject, "FlyingTowardsRestPoint");}
    }
    
    public void FIXED_UPDATE_WALKING_STATE()
    {
        if (CheckIfIsAtWaypoint() && !_isRotating)
        {
            CalculateNextWalkableWaypoint(startPoint);
        }
    }
    
    public void EXIT_WALKING_STATE()
    {
        targetWpLocation = Vector3.zero;
        startPoint = Vector3.zero;
        if (_rotateCoroutineIsRunning) StopCoroutine(_rotateCoroutine); 
        _rotateCoroutineIsRunning = false;
    }

    private void CalculateNextWalkableWaypoint(Vector3 position) {
        var randDirection = Random.insideUnitSphere * _birdStateManager.birdScriptableObject.WalkRadius;
        randDirection += position;
        NavMesh.SamplePosition (randDirection, out var navHit, _birdStateManager.birdScriptableObject.WalkRadius, 1);
        var path = new NavMeshPath();
        _birdStateManager.navMeshAgent.CalculatePath(navHit.position, path);
        if (path.status == NavMeshPathStatus.PathPartial)
        {
            targetWpLocation = path.corners.Last();
            _birdStateManager.navMeshAgent.SetDestination(path.corners.Last());
        }
        else
        {
            targetWpLocation = navHit.position;
            _birdStateManager.navMeshAgent.SetDestination(navHit.position);
            
        }
        _rotateCoroutine = RotateToNextPosition();
        StartCoroutine(_rotateCoroutine);
    }
    
    private IEnumerator RotateToNextPosition()
    {
        _rotateCoroutineIsRunning = true;
        _birdStateManager.navMeshAgent.isStopped = true;
        _isRotating = true;
        var dot = Vector3.Dot(transform.forward, (targetWpLocation - transform.position).normalized);
        while (dot < 0.9f)
        {
            dot = Vector3.Dot(transform.forward, (targetWpLocation - transform.position).normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetWpLocation - transform.position), _birdStateManager.birdScriptableObject.RotateSpeed * Time.deltaTime);
            yield return null;
        }
        _rotateCoroutineIsRunning = false;
        _birdStateManager.navMeshAgent.isStopped = false;
        _isRotating = false;
    }
    
    /// <summary>
    /// This method checks if the enemy is at the waypoint.
    /// </summary>
    private bool CheckIfIsAtWaypoint()
    {
        return Vector3.Distance(transform.position, targetWpLocation) <= 0.5f;
    }
}
