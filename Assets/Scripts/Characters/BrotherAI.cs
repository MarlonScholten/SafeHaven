using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum PingType {
    Run,
    Walk,
    Hide,
    Interact,
    PickupOrUse,
    PanicHide,
}
public class BrotherAI : MonoBehaviour
{

    public float _walkSpeed = 3.5f;
    public float _runSpeed = 5f;

    private NavMeshAgent _navMeshAgent;

    private FindHidingSpot findClosestHidingSpot;

    private Transform pingLocation;

    private FearSystem _fearSystem;

    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        findClosestHidingSpot = gameObject.GetComponent<FindHidingSpot>();
        _fearSystem = GetComponent<FearSystem>();

    }

    private void MoveToLocation(Transform walkLocation, float speed){
        _navMeshAgent.SetDestination(walkLocation.position);
    }

    public void Comfort(){
        _fearSystem.Comfort();
    }

    public void PingBrother(PingType ping, Transform location){
        CustomEvent.Trigger(this.gameObject, ping.ToString());
        pingLocation = location;
    }

    public void FollowEnter(){
        
    }

    public void FollowUpdate(){
        MoveToLocation(findClosestHidingSpot.FindClosestHidingSpot(), _walkSpeed);
    }

    public void FollowFixedUpdate(){

    }

    public void FollowExit(){

    }

    public void PanicHideEnter(){

    }

    public void PanicHideUpdate(){
        Transform hidingSpot = findClosestHidingSpot.FindClosestHidingSpot();
        MoveToLocation(hidingSpot, _walkSpeed);
    }

    public void PanicHideFixedUpdate(){

    }

    public void PanicHideExit(){
        
    }

    public void HideEnter(){

    }

    public void HideUpdate(){
        MoveToLocation(pingLocation, _walkSpeed);
    }

    public void HideFixedUpdate(){

    }

    public void HideExit(){
        
    }

    public void WalkEnter(){

    }

    public void WalkUpdate(){
        MoveToLocation(pingLocation, _walkSpeed);
    }

    public void WalkFixedUpdate(){

    }

    public void WalkExit(){
        
    }
}
