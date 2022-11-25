using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private List<RouteWaypoint> _waypoints;

    private void Awake()
    {
        //_waypoints = new();
    }

    private void Start()
    {
        Debug.Log("STARTING!");
        StartCoroutine(RouteCoroutine());
    }

    private void Update() 
    {
        
    }

    private IEnumerator RouteCoroutine()
    {
        Debug.Log("Started the coroutine");
        foreach (RouteWaypoint waypoint in _waypoints)
        {
            Debug.Log("In waypoint 1");
            while (_target.transform.position != waypoint.transform.position)
            {
                Debug.Log("Moving to waypoint 1");
                Vector3 translation = waypoint.transform.position * waypoint.Velocity * Time.deltaTime;
                _target.transform.Translate(translation);
            }

            Debug.Log("Reached, waiting.");
            yield return new WaitForSeconds(waypoint.WaitTime);
        }

        yield return null;
    }
}
