using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Author: Hugo Verweij <br/>
/// Modified by: - <br/>
/// RouteBehaviour script. Handles how an object traverses throughout a route, holds waypoint behaviour and more. <br/>
/// </summary>
/// <list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///	    <item>
///         <term>Route prefab.</term>
///		    <term>Script</term>
///         <term>RouteBehaviour</term>
///		    <term>RouteBehaviour script. Handles how an object traverses throughout a route, holds waypoint behaviour and more.</term>
///	    </item>
/// </list>
public class RouteBehaviour : MonoBehaviour
{
    /// <summary>
    /// Handles whether the route starts from <see cref="OnStart"/> or <see cref="OnTrigger"/>.
    /// </summary>
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

    /// <summary>
    /// Private speed field, only gets set to catch the navmesh's original speed.
    /// </summary>
    private float _speed;

    private void Awake()
    {
        _speed = _target.speed;
    }

    private void Start()
    {
        StartCoroutine(RouteCoroutine(ExecuteType.OnStart));
    }

    /// <summary>
    /// The coroutine that holds the route behaviour. <br />
    /// Handles what to do when a target reaches a waypoint, how to get there, etc.
    /// </summary>
    /// <param name="type">The type the coroutine was executed from.</param>
    /// <returns></returns>
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
