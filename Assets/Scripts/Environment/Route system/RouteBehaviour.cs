using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RouteBehaviour : MonoBehaviour
{
    public enum ExecuteType
    {
        OnStart,
        OnTrigger
    }

    [Header("References")]
    [SerializeField] private NavMeshAgent _target;
    [SerializeField] private List<RouteWaypoint> _waypoints;

    [Header("Settings")]
    [SerializeField] private ExecuteType _executeType;
    [SerializeField] private bool _destroyOnFinish;

    private float _speed;

    private void Awake()
    {
        _speed = _target.speed;
    }

    private void Start()
    {
        StartCoroutine(RouteCoroutine(ExecuteType.OnStart));
    }

    public IEnumerator RouteCoroutine(ExecuteType type)
    {
        // Return if not executed from the correct location.
        if (type != _executeType)
            yield break;

        // Loop through the gameobjects.
        foreach (RouteWaypoint waypoint in _waypoints)
        {
            _target.SetDestination(waypoint.transform.position);

            // Wait until the destination has been reached.
            while (!_target.DestinationReached())
                yield return new WaitForSeconds(0.01f);

            // Set the speed if needed.
            _target.speed = waypoint.Speed == 0 ? _speed : waypoint.Speed;

            yield return new WaitForSeconds(waypoint.WaitTime);
        }

        if (_destroyOnFinish)
            Destroy(_target.gameObject);
    }
}
