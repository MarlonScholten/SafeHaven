using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class FSM_Scriptable_Object : ScriptableObject
{
    [SerializeField]
    private List<Transform> wayPoints;
    private int _currentWpIndex;
    [SerializeField]
    private float visionRange = 10f;
    [SerializeField]
    private float visionAngle = 45f;
    
    [SerializeField]
    private float thresholdSmallSounds = 0.1f;
    [SerializeField]
    private float thresholdLoudSounds = 6f;
    
    [SerializeField]
    private int investigateDistance = 3;
    [SerializeField]
    private int waitAtWaypointTime = 4;
    [SerializeField]
    private int waitAtInvestigatingWaypointTime = 2;
    [SerializeField]
    private int investigateTime= 10;
    
    [SerializeField]
    private int numberOfSmallSoundsToInvestigate = 3;
    [SerializeField]
    private int reduceSmallSoundsTime = 3;
    [SerializeField]
    private int stopWhenAlertedTime = 3;
}

