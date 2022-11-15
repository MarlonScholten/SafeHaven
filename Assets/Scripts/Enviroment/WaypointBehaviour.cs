using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointBehaviour : MonoBehaviour
{
    [Header("Waypoint link")]
    public GameObject Waypoint1;
    public GameObject Waypoint2;
    public bool OneWay;

    public delegate void WaypointEventHandler(WaypointBehaviour waypoint);
    public event WaypointEventHandler OnWaypointEnter;
    public event WaypointEventHandler OnWaypointExit;

    private void OnTriggerEnter(Collider other)
    {
        OnWaypointEnter?.Invoke(this);
    }

    private void OnTriggerExit(Collider other)
    {
        OnWaypointExit?.Invoke(this);
    }
}
