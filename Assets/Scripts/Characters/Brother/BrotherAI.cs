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

    [SerializeField] private float _walkSpeed = 3.5f;
    [SerializeField] private float _runSpeed = 5f;

    [SerializeField] private float _pathEndThreshold = 0.1f;

    private NavMeshAgent _navMeshAgent;

    private FindHidingSpot _findHidingSpot;

    private Transform _pingLocation;

    private FearSystem _fearSystem;

    private GameObject _player;

    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _findHidingSpot = gameObject.GetComponent<FindHidingSpot>();
        _fearSystem = GetComponent<FearSystem>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private bool PathCompleted(){
        return _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance + _pathEndThreshold;
    }

    private void MoveToLocation(Transform walkLocation, float speed){
        _navMeshAgent.SetDestination(walkLocation.position);
    }

    public void Comfort(){
        _fearSystem.Comfort();
    }

    public void PingBrother(PingType ping, Transform location){
        CustomEvent.Trigger(this.gameObject, ping.ToString());
        _pingLocation = location;
    }

    public void FollowEnter(){
        // MoveToLocation(FindHidingSpot.FindBestHidingSpot(), _walkSpeed);
    }

    public void FollowUpdate(){
        if(Input.GetKey(KeyCode.W)){
            CustomEvent.Trigger(this.gameObject, "panicHide");
        }
        Debug.Log("Follow");
    }

    public void FollowFixedUpdate(){

    }

    public void FollowExit(){

    }

    public void PanicHideEnter(){
        MoveToLocation(_findHidingSpot.FindBestHidingSpot(), _walkSpeed);
    }

    public void PanicHideUpdate(){
        if(PathCompleted()){
            Debug.Log("Brother Hidden");
            //Hide();
        }
    }

    public void PanicHideFixedUpdate(){

    }

    public void PanicHideExit(){
        
    }

    public void HideEnter(){
        MoveToLocation(_pingLocation, _walkSpeed);
    }

    public void HideUpdate(){
        if(PathCompleted()){
            //Hide();
        }    
    }

    public void HideFixedUpdate(){

    }

    public void HideExit(){
        
    }

    public void WalkEnter(){
        MoveToLocation(_pingLocation, _walkSpeed);
    }

    public void WalkUpdate(){
        
    }

    public void WalkFixedUpdate(){

    }

    public void WalkExit(){
        
    }

    public void RunEnter(){
        MoveToLocation(_pingLocation, _runSpeed);
    }

    public void RunUpdate(){
        
    }

    public void RunFixedUpdate(){

    }

    public void RunExit(){
        
    }
}
