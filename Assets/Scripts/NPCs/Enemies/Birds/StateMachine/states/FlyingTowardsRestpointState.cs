using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PathCreation;
using Unity.VisualScripting;
using UnityEngine;

public class FlyingTowardsRestpointState : MonoBehaviour
{
    private BirdStateManager _birdStateManager;

    private PathCreator _path;
    private float _distanceTravelled;
    private const EndOfPathInstruction EndOfPathInstruction = PathCreation.EndOfPathInstruction.Stop;

    public void Awake()
    {
        _birdStateManager = GetComponent<BirdStateManager>();
    }

    public void ENTER_FLYING_TOWARDS_REST_POINT_STATE()
    {
        
        DetachFromNavmesh();
        _birdStateManager.restPoint = GetClosestRestPoint();
        _path = _birdStateManager.CreatePathToClosestPointOnGivenPath(_birdStateManager.restPoint);
    }

    public void UPDATE_FLYING_TOWARDS_REST_POINT_STATE()
    {
        // if bird is on rest point then change state to resting
        if (transform.position == _birdStateManager.restPoint)
        {
            CustomEvent.Trigger(gameObject, "Sitting");
        }
    }
    public void FIXED_UPDATE_FLYING_TOWARDS_REST_POINT_STATE()
    {
        TravelPath(_path);
    }
    public void EXIT_FLYING_TOWARDS_REST_POINT_STATE()
    {
        _path = null;
        _distanceTravelled = 0;
    }

    private Vector3 GetClosestRestPoint()
    {
        var restPoints = GameObject.FindGameObjectsWithTag("BirdRestPoint");
        Vector3 closest = Vector3.zero;
        var distance = Mathf.Infinity;
        var position = transform.position;
        if (restPoints == null) return Vector3.zero;
        foreach (var restPointObject in restPoints.Select(rp => rp.transform))
        {
            Vector3 diff = restPointObject.transform.position - position;
            var curDistance = diff.sqrMagnitude;
            if (curDistance >= distance) continue;
            closest = restPointObject.transform.position;
            distance = curDistance;
        }
        return closest;
    }
    
    private void TravelPath(PathCreator path){
        _distanceTravelled += _birdStateManager.birdScriptableObject.FlySpeed * Time.deltaTime;
        transform.position = path.path.GetPointAtDistance(_distanceTravelled, EndOfPathInstruction);
        transform.rotation = path.path.GetRotationAtDistance(_distanceTravelled, EndOfPathInstruction);
    }

    private void DetachFromNavmesh()
    {
        _birdStateManager.navMeshAgent.enabled = false;
    }
}
