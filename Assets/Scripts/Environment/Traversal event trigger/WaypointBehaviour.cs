using UnityEngine;

/// /// <summary>
/// Author: Hugo Verweij <br/>
/// Modified by: - <br/>
/// Waypoint behaviour. Handles the bottom most behaviour for the waypoints. <br />
/// Sends custom events when the player enters and exists a certain waypoint. <br />
/// </summary>
/// <list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///	    <item>
///         <term>Interactable Prefab, waypoints.</term>
///		    <term>Script</term>
///         <term>WaypointBehaviour</term>
///		    <term>Waypoint behaviour. Handles the bottom most behaviour for the waypoints.</term>
///	    </item>
/// </list>
public class WaypointBehaviour : MonoBehaviour
{
    // Public.
    public GameObject Waypoint1 { get => gameObject; }
    public GameObject Waypoint2 { get => _linkedWaypoint; }
    public bool OneWay { get => _oneWay;  }

    // Delegate.
    public delegate void WaypointEvent();
    public event WaypointEvent OnWaypointEnter;
    public event WaypointEvent OnWaypointExit;

    // Private.
    [Header("References")]
    [SerializeField]
    private GameObject _linkedWaypoint;
    [SerializeField]
    private bool _oneWay;

    /// <summary>
    /// Standard start, handles the editor waypoint color.
    /// </summary>
    private void Start()
    {
        if (OneWay)
            Waypoint2.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    /// <summary>
    /// Invokes the <see cref="OnWaypointEnter"/> event once triggered.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        OnWaypointEnter?.Invoke();
    }

    /// <summary>
    /// Invokes the <see cref="OnWaypointExit"/> event once triggered.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        OnWaypointExit?.Invoke();
    }
}
