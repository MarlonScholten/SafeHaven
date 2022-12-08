using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Marlon Kerstens<br/>
/// Modified by: <br/>
/// Description: Controller for a waypoint to determine if the enemy has to wait on the waypoint or not.
/// </summary>
///<list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///	    <item>
///         <term>WaypointPrefab</term>
///		    <term>GameObject</term>
///         <term>EnemyWayPointController</term>
///		    <term>This script will hold a bool to determine if the enemy has to wait on it or not.</term>
///	    </item>
/// </list>
public class EnemyWayPointController : MonoBehaviour
{
    [Tooltip("Bool to determine if the enemy has to wait on the waypoint or not")] public bool isWaitingWayPoint;
}
