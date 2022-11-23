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
        var run = GameObject.FindGameObjectWithTag("RunLocation");
        var walk = GameObject.FindGameObjectWithTag("WalkLocation");

        if(Input.GetKey(KeyCode.O)){
            Debug.Log("ping run: " + run);
            PingBrother(PingType.Run, run.transform);
        }
        if(Input.GetKey(KeyCode.P)){
            Debug.Log("ping walk: " + walk);
            PingBrother(PingType.Walk, walk.transform);
        }
    }

    void OnRecall(){
        CustomEvent.Trigger(this.gameObject, "CallBack");
    }

    void OnPanic(){
        CustomEvent.Trigger(this.gameObject, "panicHide");
    }

    
    void OnComfort(){
        _fearSystem.Comfort();
    }

    private bool PathCompleted(){
        return _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance + _pathEndThreshold;
    }

    private void MoveToLocation(Transform walkLocation, float speed){
        _navMeshAgent.speed = speed;
        _navMeshAgent.SetDestination(walkLocation.position);
    }

    private Transform GetPlayerLocation(){
        return _player.transform;
    }

    public void PingBrother(PingType ping, Transform location){
        Debug.Log("brother ping: " + ping);
        _pingLocation = location;
        CustomEvent.Trigger(this.gameObject, ping.ToString());     
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
        Debug.Log("Brother Hidden");
    }

    public void HiddenFixedUpdate(){

    }

    public void HiddenExit(){
        
    }

    public void WalkEnter(){
        MoveToLocation(_pingLocation, _walkSpeed);
    }

    public void WalkUpdate(){
        if(PathCompleted()){
            CustomEvent.Trigger(this.gameObject, "Idle");
        }
    }

    public void WalkFixedUpdate(){

    }

    public void WalkExit(){
        
    }

    public void RunEnter(){
        MoveToLocation(_pingLocation, _runSpeed);
    }

    public void RunUpdate(){
        if(PathCompleted()){
            CustomEvent.Trigger(this.gameObject, "Idle");
        }
    }

    public void RunFixedUpdate(){

    }

    public void RunExit(){
        
    }

    public void IdleEnter(){
        _navMeshAgent.ResetPath();
    }

    public void IdleUpdate(){
        Debug.Log("Brother idle");
    }

    public void IdleFixedUpdate(){

    }

    public void IdleExit(){
        
    }
}
