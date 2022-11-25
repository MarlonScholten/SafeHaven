using UnityEngine;

/// <summary>
/// Author: Hugo Verweij <br/>
/// Modified by: - <br/>
/// RouteWaypoint script. Handles and holds the waypoint information that makes up the route, which the <see cref="RouteBehaviour"/> uses. <br/>
/// </summary>
/// <list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///	    <item>
///         <term>Route prefab..</term>
///		    <term>Script</term>
///         <term>RouteWaypoint</term>
///		    <term>RouteWaypoint script. Handles and holds the waypoint information that makes up the route, which the <see cref="RouteBehaviour"/> uses.</term>
///	    </item>
/// </list>
public class RouteWaypoint : MonoBehaviour
{
    /// <summary>
    /// The speed that the object will have from here on out.
    /// </summary>
    public float Speed => _speed;
    /// <summary>
    /// The time the object will take to wait at this waypoint.
    /// </summary>
    public float WaitTime => _waitTime;

    [Header("Settings")]
    [SerializeField]
    [Tooltip("The speed the object will have from here on out.")]
    private float _speed;
    [SerializeField]
    [Tooltip("The time the object will take to wait at this waypoint.")]
    private float _waitTime;
}
