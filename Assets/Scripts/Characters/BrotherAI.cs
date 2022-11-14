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
    [SerializeField] Transform _walkLocation;

    private NavMeshAgent _navMeshAgent;

    private FindHidingSpot findClosestHidingSpot;

    private Transform pingLocation;



    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        findClosestHidingSpot = gameObject.GetComponent<FindHidingSpot>();

    }

    void FixedUpdate()
    {
            
    }

    private void MoveToLocation(Transform walkLocation){
        _navMeshAgent.SetDestination(walkLocation.position);
    }

//action will become an enum
    public void PingBrother(PingType ping, Transform location){
        CustomEvent.Trigger(this.gameObject, ping.ToString());
        pingLocation = location;
    }

    public void FollowEnter(){
        
    }

    public void FollowUpdate(){
        MoveToLocation(findClosestHidingSpot.FindClosestHidingSpot());
    }

    public void FollowFixedUpdate(){

    }

    public void FollowExit(){

    }

    public void PanicHideEnter(){

    }

    public void PanicHideUpdate(){
        Transform hidingSpot = findClosestHidingSpot.FindClosestHidingSpot();
        MoveToLocation(hidingSpot);
    }

    public void PanicHideFixedUpdate(){

    }

    public void PanicHideExit(){
        
    }

    public void HideEnter(){

    }

    public void HideUpdate(){
        MoveToLocation(pingLocation);
    }

    public void HideFixedUpdate(){

    }

    public void HideExit(){
        
    }

    public void WalkEnter(){

    }

    public void WalkUpdate(){
        MoveToLocation(pingLocation);
    }

    public void WalkFixedUpdate(){

    }

    public void WalkExit(){
        
    }
}
