using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PathCreation;
using UnityEngine;
using UnityEngine.AI;

public class BirdStateManager : MonoBehaviour
{
    public BirdScriptableObject birdScriptableObject;
    [NonSerialized] public NavMeshAgent navMeshAgent;
    [NonSerialized] public Transform restPoint;
    [NonSerialized] public float groundHeight;
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// This method calls a method after a certain amount of seconds.
    /// <param name="seconds">Amount of seconds that takes to call the method.</param>
    /// <param name="method">The method that need to be called after the amount of seconds.</param>
    /// </summary>   
    public IEnumerator CallFunctionAfterSeconds(int seconds, Action method)
    {
        yield return new WaitForSeconds(seconds);
        method();
    }
    
    public PathCreator CreatePathToClosestPointOnGivenPath(Vector3 destination)
    {
        var position = transform.position;
        var createdPath = new GameObject().AddComponent<PathCreator>();
        var startCurve = Vector3.Lerp(position, destination, 0.25f);
        startCurve.y = position.y;
        var endCurve = Vector3.Lerp(position, destination, 0.75f);
        endCurve.y = destination.y;
        var bezierPath = new BezierPath(new List<Vector3> { position, startCurve, endCurve, destination }, false);
        createdPath.bezierPath = bezierPath;
        createdPath.bezierPath.ControlPointMode = BezierPath.ControlMode.Automatic;
        createdPath.bezierPath.GlobalNormalsAngle = 20;
        createdPath.bezierPath.FlipNormals = true;
        return createdPath;
    }
    
    public bool CheckIfAlertingObjectsAreNearby(ICollection<string> alertingObjects)
    {
        var position = transform.position;
        position.y = groundHeight;
        var colliders = Physics.OverlapSphere(position, birdScriptableObject.AlertRadius);
        return colliders.Any(col => alertingObjects.Contains(col.gameObject.tag));
    }
    
    /// <summary>
    /// This method checks if the bird is at destination.
    /// </summary>
    public bool CheckIfIsAtWaypoint(Vector3 destination)
    {
        return Vector3.Distance(transform.position, destination) <= 0.5f;
    }
}
