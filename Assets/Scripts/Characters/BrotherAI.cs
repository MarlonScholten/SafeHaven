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
    private float fear = 0;
    [SerializeField] Transform _walkLocation;

    public float _walkSpeed = 3.5f;
    public float _runSpeed = 5f;

    private NavMeshAgent _navMeshAgent;

    private FindHidingSpot findClosestHidingSpot;

    private Transform pingLocation;

    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        findClosestHidingSpot = gameObject.GetComponent<FindHidingSpot>();

    }

    private void MoveToLocation(Transform walkLocation, float speed){
        _navMeshAgent.SetDestination(walkLocation.position);
    }

    public void Comfort(){
        if(fear > 0){
        fear -= 0.1f;
        }
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
