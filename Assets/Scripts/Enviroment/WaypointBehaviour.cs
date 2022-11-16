using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles how the waypoints behave, and is the bottom most class in the interactable hierarchy.
/// </summary>
public class WaypointBehaviour : MonoBehaviour
{
    // Public.
    public GameObject Waypoint1 { get => gameObject; }
    public GameObject Waypoint2 { get => _linkedWaypoint; }
    public bool OneWay { get => _oneWay;  }

    // Delegate.
    public delegate void WaypointEventHandler();
    public event WaypointEventHandler OnWaypointEnter;
    public event WaypointEventHandler OnWaypointExit;

    // Private.
    [Header("References")]
    [SerializeField]
    private GameObject _linkedWaypoint;
    [SerializeField]
    private bool _oneWay;

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
