using System.Collections;
using System.Collections.Generic;
using PathCreation;
using Unity.VisualScripting;
using UnityEngine;

public class FlyingTowardsNavmeshState : MonoBehaviour
{
    private BirdStateManager _birdStateManager;

    private Vector3 _destinationAtNavmesh;
    private PathCreator _path;
    private float _distanceTravelled;
    private const EndOfPathInstruction EndOfPathInstruction = PathCreation.EndOfPathInstruction.Stop;

    public void Awake()
    {
        _birdStateManager = GetComponent<BirdStateManager>();
    }

    public void ENTER_FLYING_TOWARDS_NAVMESH_STATE()
    {
        _destinationAtNavmesh = GetPointOnNavmesh();
        _path = _birdStateManager.CreatePathToClosestPointOnGivenPath(_destinationAtNavmesh);
    }

    public void UPDATE_FLYING_TOWARDS_NAVMESH_STATE()
    {
        // if bird is on rest point then change state to resting
    }

    public void FIXED_UPDATE_FLYING_TOWARDS_NAVMESH_STATE()
    {
        TravelPath(_path);
    }

    public void EXIT_FLYING_TOWARDS_NAVMESH_STATE()
    {
        _path = null;
        _distanceTravelled = 0;
    }
    
    private void TravelPath(PathCreator path){
        _distanceTravelled += _birdStateManager.birdScriptableObject.FlySpeed * Time.deltaTime;
        transform.position = path.path.GetPointAtDistance(_distanceTravelled, EndOfPathInstruction);
        transform.rotation = path.path.GetRotationAtDistance(_distanceTravelled, EndOfPathInstruction);
    }

    private Vector3 GetPointOnNavmesh()
    {
        // get close point on _birdStateManager.navmesh
        

    }
}

