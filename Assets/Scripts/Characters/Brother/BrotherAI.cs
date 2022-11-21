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

    private float _pathEndThreshold = 0.1f;

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

    void FixedUpdate(){
        if(Input.GetKey(KeyCode.Z)){
            CustomEvent.Trigger(this.gameObject, "CallBack");
        }

        if(Input.GetKey(KeyCode.W)){
            CustomEvent.Trigger(this.gameObject, "panicHide");
        }
    }

    private bool PathCompleted(){
        return _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance + _pathEndThreshold;
    }

    private void MoveToLocation(Transform walkLocation, float speed){
        _navMeshAgent.SetDestination(walkLocation.position);
    }

    private Transform GetPlayerLocation(){
        return _player.transform;
    }

    public void Comfort(){
        _fearSystem.Comfort();
    }

    public void PingBrother(PingType ping, Transform location){
        CustomEvent.Trigger(this.gameObject, ping.ToString());
        _pingLocation = location;
    }

    public void FollowEnter(){
        
    }

    public void FollowUpdate(){
        MoveToLocation(GetPlayerLocation(), _walkSpeed);
        Debug.Log("Follow");
    }

    public void FollowFixedUpdate(){

    }

    public void FollowExit(){

    }

    public void HoldingHandsEnter(){
        
    }

    public void HoldingHandsUpdate(){
        MoveToLocation(GetPlayerLocation(), _walkSpeed);
    }

    public void HoldingHandsFixedUpdate(){

    }

    public void HoldingHandsExit(){

    }

    public void PanicHideEnter(){
        MoveToLocation(_findHidingSpot.FindBestHidingSpot(), _walkSpeed);
    }

    public void PanicHideUpdate(){
        if(PathCompleted()){
            Debug.Log("Brother Hidden");
            CustomEvent.Trigger(this.gameObject, "Hidden");
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
            CustomEvent.Trigger(this.gameObject, "Hidden");
        }    
    }

    public void HideFixedUpdate(){

    }

    public void HideExit(){
        
    }

    public void HiddenEnter(){

    }

    public void HiddenUpdate(){
        
    }

    public void HiddenFixedUpdate(){

    }

    public void HiddenExit(){
        
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

    public void IdleEnter(){
        _navMeshAgent.ResetPath();
    }

    public void IdleUpdate(){
        
    }

    public void IdleFixedUpdate(){

    }

    public void IdleExit(){
        
    }
}
