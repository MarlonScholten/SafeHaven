using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// Custom interactable event, passes <see cref="WaypointBehaviour"/> as an additional parameter.
/// </summary>
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
    public InteractableEvent OnTraversalRestricted;
    public InteractableEvent OnTraversalUnrestricted;

    // Private.
    [Header("References")]
    [SerializeField]
    private TMP_Text _instructions;

    private PlayerInput _input;
    private GameObject _player;
    private List<WaypointBehaviour> _waypointsInRange;

    /// <summary>
    /// Standard awake, sets references & events.
    /// </summary>
    private void Awake()
    {
        _input = new();
        _waypointsInRange = new();

        _input.Player.Interact.performed += OnInteractInput;
        _player = GameObject.FindGameObjectWithTag("Player");

        HandleWaypointEvents();
    }

    private void Start()
    {
        // TODO : Create a custom text iput for the active control input.
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
    /// Enables the <see cref="PlayerInput"/> whenever <see cref="InteractableBehaviour"/> is active.
    /// </summary>
    public void OnEnable()
    {
        _input.Enable();
    }

    /// <summary>
    /// Enables the <see cref="PlayerInput"/> whenever <see cref="InteractableBehaviour"/> is inactive.
    /// </summary>
    public void OnDisable()
    {
        _input.Disable();
    }

    /// <summary>
    /// Input function, gets called whenever the player presses the corresponding input key.
    /// Fetches the closest waypoint, and invokes the corresponding traversal event to be handled.
    /// </summary>
    /// <remarks>
    /// <para><see cref="OnTraversalRestricted"/> invoked when <see cref="WaypointBehaviour.OneWay"/> is enabled.</para>
    /// <para><see cref="OnTraversalUnrestricted"/> invoked when <see cref="WaypointBehaviour.OneWay"/> is not enabled.</para>
    /// </remarks>
    /// <param name="context"></param>
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (_waypointsInRange.Count == 0)
            return;

        Vector3 originPos = _player.transform.position;
        GameObject closest = _waypointsInRange.Select(x => x.gameObject).GetClosestGameObject(originPos);
        WaypointBehaviour converted = closest.GetComponent<WaypointBehaviour>();

        if (converted.OneWay || converted.Waypoint1.GetComponent<WaypointBehaviour>().OneWay)
            OnTraversalRestricted?.Invoke(converted);
        else OnTraversalUnrestricted?.Invoke(converted);
    }
}
