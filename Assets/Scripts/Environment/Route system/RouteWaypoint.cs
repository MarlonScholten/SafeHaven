using UnityEngine;

public class RouteWaypoint : MonoBehaviour
{
    public float Speed => _speed;
    public float WaitTime => _waitTime;

    [Header("Settings")]
    [SerializeField]
    [Tooltip("The speed the object will have from here on out.")]
    private float _speed;
    [SerializeField]
    [Tooltip("The time the object will take to wait at this waypoint.")]
    private float _waitTime;
}
