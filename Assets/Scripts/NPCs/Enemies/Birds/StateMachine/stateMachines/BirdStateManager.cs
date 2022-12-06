using System;
using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;
using UnityEngine.AI;

public class BirdStateManager : MonoBehaviour
{
    public BirdScriptableObject birdScriptableObject;
    [NonSerialized] public NavMeshAgent navMeshAgent;
    [NonSerialized] public Vector3 restPoint;
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        // TODO: Play walk animation
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
}
