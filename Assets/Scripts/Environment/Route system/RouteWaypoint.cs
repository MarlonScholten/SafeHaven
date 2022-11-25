using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RouteWaypoint : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent WaypointReached;
    public float Velocity => _velocity;
    public float WaitTime => _waitTime;

    [Header("Settings")]
    [SerializeField]
    [Tooltip("The velocity the object will have from here on out.")]
    private float _velocity = 10;
    [SerializeField]
    [Tooltip("The time the object will take to wait at this waypoint.")]
    private float _waitTime;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
