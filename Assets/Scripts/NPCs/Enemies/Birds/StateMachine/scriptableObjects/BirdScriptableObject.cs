using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BirdScriptableObject", order = 2)] 
public class BirdScriptableObject : ScriptableObject
{
    [Tooltip("The radius the bird will walk from start point")]
    [SerializeField] private float walkRadius = 10f; 
    public float WalkRadius => walkRadius;
    
    [Tooltip("The tags that the bird will get alerted by")]
    [SerializeField] private List<string> alertTags; 
    public List<string> AlertTags => alertTags;
    
    [Tooltip("The radius the bird will get alerted")]
    [SerializeField] private float alertRadius = 5f; 
    public float AlertRadius => alertRadius;
    
    [Tooltip("The speed the bird will rotate")]
    [SerializeField] private float rotateSpeed = 5f;
    public float RotateSpeed => rotateSpeed;
    
    [Tooltip("The speed the bird will fly")]
    [SerializeField] private float flySpeed = 3f;
    public float FlySpeed => flySpeed;
    
    [Tooltip("The time the bird will stay at rest point")]
    [SerializeField] private int timeAtRestPoint = 3;
    public int TimeAtRestPoint => timeAtRestPoint;

}
