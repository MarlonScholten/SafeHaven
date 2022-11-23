using UnityEngine;

/// <summary>
/// Author: Hugo Ulfman </para>
/// Modified by:  Marlon Kerstens </para>
/// Description: Scriptable object for the EnemyAi.
/// </summary>
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyAIScriptableObject", order = 1)]
public class FSM_Scriptable_Object : ScriptableObject
{
    [Tooltip("The range/distance in which the enemy can see the player/brother")]
    public float visionRange = 10f; 
    [Tooltip("The angle in which the enemy can see the player/brother")]
    public float visionAngle = 90f;
    
    [Tooltip("The threshold for small sounds that the enemy can hear")]
    public float thresholdSmallSounds = 0.1f;
    [Tooltip("The threshold for loud sounds that the enemy can hear")]
    public float thresholdLoudSounds = 6f;
    [Tooltip("The number of small sounds that the enemy needs to hear before investigating")]
    public int numberOfSmallSoundsToInvestigate = 3;
    [Tooltip("The time in seconds that the enemy needs to reduce the number of small sounds that it has heard")]
    public int reduceSmallSoundsTime = 3;

    [Tooltip("The time in seconds that the enemy will wait at a waypoint before moving on to the next waypoint")] 
    public int waitAtWaypointTime = 4;
   
    [Tooltip("The time in seconds that the enemy will wait at a waypoint while investigating before moving on to the next waypoint")]
    public int waitAtInvestigatingWaypointTime = 2;
    [Tooltip("The distance that the enemy will investigate on the last known position of the player/brother or the last position of a sound")]
    public int investigateDistance = 3;
    [Tooltip("The time in seconds that the enemy will investigate before giving up and returning to its patrol")] 
    public int investigateTime = 10;
    [Tooltip("The time in seconds that the enemy will chase the player/brother when the player/brother is not seen. (This is to keep chasing the player/brother if the player/brother is behind a wall)")]
    public int chaseTimeWhenNotSeen = 3;
    
    [Tooltip("The time in seconds that the enemy will stop when alerted")]
    public int stopWhenAlertedTime = 3;

    [Tooltip("The distance in which the enemy can catch the player/brother")]
    public float catchDistance = 0.5f;
}

