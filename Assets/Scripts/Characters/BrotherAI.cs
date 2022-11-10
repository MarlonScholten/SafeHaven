using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BrotherAI : MonoBehaviour
{
    [SerializeField] Transform _walkLocation;
    private NavMeshAgent _navMeshAgent;
    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        WalkToLocation(_walkLocation);
    }

    private void WalkToLocation(Transform walkLocation){
        _navMeshAgent.SetDestination(walkLocation.position);
    }

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
