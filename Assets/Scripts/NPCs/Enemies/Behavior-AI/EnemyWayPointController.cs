using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Marlon Kerstens<br/>
/// Modified by: <br/>
/// Description: Controller for a waypoint to determine if the enemy has to wait on the waypoint or not.
/// </summary>
public class EnemyWayPointController : MonoBehaviour
{
    [Tooltip("Bool to determine if the enemy has to wait on the waypoint or not")] public bool isWaitingWayPoint;
}
