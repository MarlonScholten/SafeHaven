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

    public FieldOfView fieldOfView;



    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        fieldOfView = gameObject.GetComponent<FieldOfView>();

    }

    void FixedUpdate()
    {
            MoveToLocation(fieldOfView.FindClosestHidingSpot());
    }

    private void MoveToLocation(Transform walkLocation){
        _navMeshAgent.SetDestination(walkLocation.position);
    }

//action will become an enum
    public void PingBrother(PingType ping, Transform location){
        if(ping == PingType.PanicHide){
            CustomEvent.Trigger(this.gameObject, "PanicHide");
        }
        // if(action instanceof HideAction ){
        // HideAction hideAction = action;
        // hideAction.hide();
        // hideNearLocation(location);
        // }
        // if(action instanceof PanicHide){
        // panicHide();
        // }
    }

    public void FollowEnter(){

    }

    public void FollowUpdate(){

    }

    public void FollowFixedUpdate(){

    }

    public void FollowExit(){

    }

    public void PanicHideEnter(){

    }

    public void PanicHideUpdate(){

    }

    public void PanicHideFixedUpdate(){

    }

    public void PanicHideExit(){
        
    }
}
