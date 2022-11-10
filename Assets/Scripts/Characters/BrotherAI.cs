using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BrotherAI : MonoBehaviour
{
    [SerializeField] Transform _walkLocation;
    [SerializeField] GameObject _pickup;

    [SerializeField] float _pickupDistance;

    private NavMeshAgent _navMeshAgent;

    [SerializeField] Transform _hand;

    private GameObject inventory = null;

    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate()
    {
        PickupAction(_pickup.transform, _pickup);           

        // Dont have a specific place for holding right now
        // When the brother has an object the object location needs to be updated.
        if(inventory != null){
            HoldItem();
            MoveToLocation(_walkLocation);
        }

        if(_navMeshAgent.remainingDistance < 1 && inventory != null){
            DropItem();
        }
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

    public void PickupAction(Transform objectLocation, GameObject objectToPickup){
        MoveToLocation(objectLocation);
        if(Vector3.Distance(_pickup.transform.position, transform.position) < _pickupDistance){
            PickupItem(_pickup);
        }     
    }

    public void PickupItem(GameObject item){
        if(inventory is not null){
            DropItem();
        }
        inventory = item;
    }

    public void DropItem(){
      inventory.transform.parent = null;
      inventory.GetComponent<Rigidbody>().isKinematic = false;
      inventory = null;  
    }

    public void HoldItem(){
        inventory.transform.position = _hand.position;
        inventory.transform.rotation = _hand.rotation;
        inventory.transform.parent = _hand.transform;
        inventory.GetComponent<Rigidbody>().isKinematic = true;
    }
}
