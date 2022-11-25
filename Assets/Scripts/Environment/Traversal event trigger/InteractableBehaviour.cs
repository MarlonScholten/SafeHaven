using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// /// <summary>
/// Author: Hugo Verweij <br/>
/// Modified by: - <br/>
/// Interactable behaviour. Handles the top most logic for <see cref="WaypointBehaviour"/>. <br />
/// Saves which waypoint is in range, and fires custom events for other classes to handle actual movement. <br />
/// </summary>
/// <list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///	    <item>
///         <term>Interactable Prefab</term>
///		    <term>Script</term>
///         <term>InteractableEvent</term>
///		    <term>Interactable behaviour. Handles the top most logic for <see cref="WaypointBehaviour"/>.</term>
///	    </item>
/// </list>
[Serializable]
public class InteractableEvent : UnityEvent<WaypointBehaviour>
{
}

/// <summary>
/// Handles how the main interactable component behaves, and is the top most level class in the interactable hierarchy.
/// </summary>
public class InteractableBehaviour : MonoBehaviour
{
    // Public.
    [Header("Events")]
    [Tooltip("Takes 'OneWay' into consideration.")]
    public InteractableEvent OnTraversalRestricted;
    [Tooltip("Does not take 'OneWay' into consideration.")]
    public InteractableEvent OnTraversalUnrestricted;

    // Private.
    [Header("References")]
    [SerializeField]
    private TMP_Text _instructions;

    private GameObject _player;
    private List<WaypointBehaviour> _waypointsInRange;

    /// <summary>
    /// Standard awake, sets references & events.
    /// </summary>
    private void Awake()
    {
        _waypointsInRange = new();

        InputBehaviour.Instance.OnObstacleInteract += OnObstacleInteract;
        _player = GameObject.FindGameObjectWithTag("Player");

        HandleWaypointEvents();
    }

    /// <summary>
    /// Sets the behaviour for the waypoint <see cref="WaypointBehaviour.OnWaypointEnter"/> and <see cref="WaypointBehaviour.OnWaypointExit"/>.
    /// Removes and adds the active waypoints to the <see cref="_waypointsInRange"/>, whilst handling the visibility of <see cref="_instructions"/>.
    /// </summary>
    private void HandleWaypointEvents()
    {
        // Loop through all waypoints.
        foreach (WaypointBehaviour waypoint in GetComponentsInChildren<WaypointBehaviour>())
        {
            // Remove the waypoint when applicable.
            waypoint.OnWaypointExit += () =>
            {
                if (_waypointsInRange.Contains(waypoint))
                    _waypointsInRange.Remove(waypoint);

                _instructions.gameObject.SetActive(false);
            };
            // Add the waypoint when applicable.
            waypoint.OnWaypointEnter += () =>
            {
                if (!_waypointsInRange.Contains(waypoint))
                    _waypointsInRange.Add(waypoint);

                _instructions.gameObject.SetActive(true);
            };
        }
    }

    /// <summary>
    /// Input function, gets called whenever the player presses the corresponding input key.
    /// Fetches the closest waypoint, and invokes the corresponding traversal event to be handled.
    /// </summary>
    /// <remarks>
    /// <para><see cref="OnTraversalRestricted"/> invoked when <see cref="WaypointBehaviour.OneWay"/> is not enabled.</para>
    /// <para><see cref="OnTraversalUnrestricted"/> invoked regarldess of <see cref="WaypointBehaviour.OneWay"/>.</para>
    /// </remarks>
    /// <param name="context"></param>
    public void OnObstacleInteract(InputAction.CallbackContext context)
    {
        if (_waypointsInRange.Count == 0)
            return;

        // Fetch position, gets the closest and converts it.
        Vector3 originPos = _player.transform.position;
        GameObject closest = _waypointsInRange.Select(x => x.gameObject).GetClosestGameObject(originPos);
        WaypointBehaviour converted = closest.GetComponent<WaypointBehaviour>();

        // Only traverse restricted if the linked waypoint isn't marked as a one way.
        if (!converted.Waypoint2.GetComponent<WaypointBehaviour>().OneWay)
            OnTraversalRestricted?.Invoke(converted);

        OnTraversalUnrestricted?.Invoke(converted);
    }
}
