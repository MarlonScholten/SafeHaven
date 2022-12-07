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
        _path = _birdStateManager.CreatePathToClosestPointOnGivenPath(_birdStateManager.restPoint.position);
    }

    public void UPDATE_FLYING_TOWARDS_REST_POINT_STATE()
    {
        if (transform.position == _birdStateManager.restPoint.position)
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

    private Transform GetClosestRestPoint()
    {
        var restPoints = GameObject.FindGameObjectsWithTag("BirdRestPoint");
        Transform closest = null;
        var distance = Mathf.Infinity;
        var position = transform.position;
        if (restPoints == null) return null;
        foreach (var restPointObject in restPoints.Select(rp => rp.transform))
        {
            Vector3 diff = restPointObject.transform.position - position;
            var curDistance = diff.sqrMagnitude;
            if (curDistance >= distance) continue;
            closest = restPointObject.transform;
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
