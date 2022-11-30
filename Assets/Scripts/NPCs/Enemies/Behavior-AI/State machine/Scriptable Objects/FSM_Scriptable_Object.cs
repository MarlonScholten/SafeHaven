using UnityEngine;

/// <summary>
/// Author: Hugo Ulfman<br/>
/// Modified by:  Marlon Kerstens<br/>
/// Description: Scriptable object for the EnemyAi.
/// </summary>
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyAIScriptableObject", order = 1)]
public class FSM_Scriptable_Object : ScriptableObject
{
    [Tooltip("The range/distance in which the enemy can see the player/brother")]
    [SerializeField] private float visionRange = 10f; 
    public float VisionRange => visionRange;
    
    [Tooltip("The angle in which the enemy can see the player/brother")]
    [SerializeField] private float visionAngle = 90f; 
    public float VisionAngle => visionAngle;
    
    [Tooltip("The threshold for small sounds that the enemy can hear")]
    [SerializeField] private float thresholdSmallSounds = 0.1f;
    public float ThresholdSmallSounds => thresholdSmallSounds;
    
    [Tooltip("The threshold for loud sounds that the enemy can hear")]
    [SerializeField] private float thresholdLoudSounds = 6f;
    public float ThresholdLoudSounds => thresholdLoudSounds;
    
    [Tooltip("The number of small sounds that the enemy needs to hear before investigating")]
    [SerializeField] private int numberOfSmallSoundsToInvestigate = 3;
    public int NumberOfSmallSoundsToInvestigate => numberOfSmallSoundsToInvestigate;

    [Tooltip("The time in seconds that the enemy needs to reduce the number of small sounds that it has heard")]
    [SerializeField] private int reduceSmallSoundsTime = 3;
    public int ReduceSmallSoundsTime => reduceSmallSoundsTime;
    
    [Tooltip("The time in seconds that the enemy will wait at a waypoint before moving on to the next waypoint")] 
    [SerializeField] private int waitAtWaypointTime = 4;
    public int WaitAtWaypointTime => waitAtWaypointTime;
    
    [Tooltip("The time in seconds that the enemy will wait at a waypoint while investigating before moving on to the next waypoint")]
    [SerializeField] private int waitAtInvestigatingWaypointTime = 2;
    public int WaitAtInvestigatingWaypointTime => waitAtInvestigatingWaypointTime;
    
    [Tooltip("The distance that the enemy will investigate on the last known position of the player/brother or the last position of a sound")]
    [SerializeField] private int investigateDistance = 2;
    public int InvestigateDistance => investigateDistance;
    
    [Tooltip("The time in seconds that the enemy will investigate before giving up and returning to its patrol")] 
    [SerializeField] private int investigateTime = 10;
    public int InvestigateTime => investigateTime;
    
    [Tooltip("The time in seconds that the enemy will chase the player/brother when the player/brother is not seen. (This is to keep chasing the player/brother if the player/brother is behind a wall)")]
    [SerializeField] private int chaseTimeWhenNotSeen = 3;
    public int ChaseTimeWhenNotSeen => chaseTimeWhenNotSeen;

    [Tooltip("The time in seconds that the enemy will stop when alerted")]
    [SerializeField] private int stopWhenAlertedTime = 3;
    public int StopWhenAlertedTime => stopWhenAlertedTime;
    
    [Tooltip("The distance in which the enemy can catch the player/brother")]
    [SerializeField] private float catchDistance = 0.5f;
    public float CatchDistance => catchDistance;
    
    [Tooltip("The distance that a guard can notify other enemies")]
    [SerializeField] private float guardAlertRadius = 10;
    public float GuardAlertRadius => guardAlertRadius;
    
    [Tooltip("The distance that a guard can patrol around its waypoint")]
    [SerializeField] private float guardPatrolRadius = 3;
    public float GuardPatrolRadius => guardPatrolRadius;
    
    [Tooltip("The time that the enemy will communicate with other enemies")]
    [SerializeField] private int communicationTime = 4;
    public int CommunicationTime => communicationTime;
    
    [Tooltip("The radius in which the enemy will communicate with other enemies")]
    [SerializeField] private float communicationRadius = 2;
    public float CommunicationRadius => communicationRadius;
    
    [Tooltip("The time to forget the object that the enemy communicated with. (This is to prevent the enemy from communicating with the same object over and over again)")]
    [SerializeField] private int timeToForgetCommunicationWithEnemy = 2;
    public int TimeToForgetCommunicationWithEnemy => timeToForgetCommunicationWithEnemy;
}

