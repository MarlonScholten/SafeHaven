using UnityEngine;

/// <summary>
/// Scriptable object for the Enemy AI
/// </summary>
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyAIScriptableObject", order = 1)]
public class FSM_Scriptable_Object : ScriptableObject
{
    public float visionRange = 10f;
    public float visionAngle = 90f;
    
    public float thresholdSmallSounds = 0.1f;
    public float thresholdLoudSounds = 6f;
    
    public int investigateDistance = 3;
    public int waitAtWaypointTime = 4;
    public int waitAtInvestigatingWaypointTime = 2;
    public int investigateTime= 10;
    public int chaseTimeWhenNotSeen = 3;
    
    public int numberOfSmallSoundsToInvestigate = 3;
    public int reduceSmallSoundsTime = 3;
    public int stopWhenAlertedTime = 3;

    public float catchDistance = 0.05f;
}

