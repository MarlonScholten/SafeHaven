using UnityEngine;

/// <summary>
/// Scriptable object for the EnemyAi
/// </summary>
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyAIScriptableObject", order = 1)]
public class FSM_Scriptable_Object : ScriptableObject
{

    public float visionRange = 10f; // The range/distance in which the enemy can see the Sibling 
    public float visionAngle = 90f; // The angle in which the enemy can see the Sibling
    
    public float thresholdSmallSounds = 0.1f; // The threshold for small sounds that the enemy can hear
    public float thresholdLoudSounds = 6f; // The threshold for loud sounds that the enemy can hear
    public int numberOfSmallSoundsToInvestigate = 3; // The number of small sounds that the enemy needs to hear before investigating
    public int reduceSmallSoundsTime = 3; // The time in seconds that the enemy needs to reduce the number of small sounds that it has heard
    
    public int waitAtWaypointTime = 4; // The time in seconds that the enemy will wait at a waypoint before moving on to the next waypoint
   
    public int waitAtInvestigatingWaypointTime = 2; // The time in seconds that the enemy will wait at a waypoint while investigating before moving on to the next waypoint
    public int investigateDistance = 3; // The distance that the enemy will investigate on the last known position of the Sibling or the last position of a sound
    public int investigateTime = 10; // The time in seconds that the enemy will investigate before giving up and returning to its patrol
    public int chaseTimeWhenNotSeen = 3; // The time in seconds that the enemy will chase the Sibling when the Sibling is not seen. (This is to keep chasing the Sibling if the Sibling is behind a wall)
    
    public int stopWhenAlertedTime = 3; // The time in seconds that the enemy will stop when alerted

    public float catchDistance = 0.05f; // The distance in which the enemy can catch the Sibling
}

