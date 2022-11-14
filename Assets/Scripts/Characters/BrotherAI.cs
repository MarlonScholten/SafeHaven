using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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
    public void PingBrother(object action){
        // if(action instanceof HideAction ){
        // HideAction hideAction = action;
        // hideAction.hide();
        // hideNearLocation(location);
        // }
        // if(action instanceof PanicHide){
        // panicHide();
        // }
    }
}
