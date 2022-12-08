using PathCreation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

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
        // if the bird is close enough to the destination, then switch to the next state
        if (_birdStateManager.CheckIfIsAtWaypoint(_destinationAtNavmesh))
        {
            AttachToNavmesh();
            CustomEvent.Trigger(gameObject, "Walking");
        }
    }

    public void FIXED_UPDATE_FLYING_TOWARDS_NAVMESH_STATE()
    {
        TravelPath(_path);
    }

    public void EXIT_FLYING_TOWARDS_NAVMESH_STATE()
    {
        _path = null;
        Destroy(_birdStateManager.pathGameObject);
        _distanceTravelled = 0;
        _birdStateManager.restPoint = null;
        _birdStateManager.lastRestPoint = Vector3.zero;
    }
    
    private void TravelPath(PathCreator path){
        _distanceTravelled += _birdStateManager.birdScriptableObject.FlySpeed * Time.deltaTime;
        transform.position = path.path.GetPointAtDistance(_distanceTravelled, EndOfPathInstruction);
        transform.rotation = path.path.GetRotationAtDistance(_distanceTravelled, EndOfPathInstruction);
    }
    
    private Vector3 GetPointOnNavmesh()
    {
        var point = transform.position;
        point.y = 0;
        point.x += Random.Range(4, 6) * (Random.value > 0.5f ? 1 : -1);
        point.z += Random.Range(4, 6) * (Random.value > 0.5f ? 1 : -1);
        NavMesh.SamplePosition(point, out var hit, 10, NavMesh.AllAreas);
        return hit.position;
    }
    
    private void AttachToNavmesh()
    {
        _birdStateManager.navMeshAgent.enabled = true;
    }
}

