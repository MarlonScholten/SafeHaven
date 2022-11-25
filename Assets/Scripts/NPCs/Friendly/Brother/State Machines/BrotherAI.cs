using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// Author: Jelco van der Straaten </para>
/// Modified by:  Jelco</para>
/// This script controls the state of the brotherAI. In this script al the calculation for the states are made.
/// </summary>
/// <list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///     <item>
///         <term>-</term>
///		    <term>-</term>
///         <term>-</term>
///		    <term>-</term>
///	    </item>
/// </list>


public class BrotherAI : MonoBehaviour
{
    /// <summary>
    /// This value determines the maximum walkspeed of the brother.
    /// </summary>
    [Range(2.0f, 4.0f), Tooltip("This value determines the maximum walkspeed of the brother.")]
    [SerializeField] private float _walkSpeed = 3.5f;

    private float _pathEndThreshold = 0.1f;

    private NavMeshAgent _navMeshAgent;

    private FindHidingSpot _findHidingSpot;

    private Vector3 _pingLocation;

    private FearSystem _fearSystem;

    private GameObject _player;

    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _findHidingSpot = gameObject.GetComponent<FindHidingSpot>();
        _fearSystem = GetComponent<FearSystem>();
        _player = GameObject.FindGameObjectWithTag("Player");                
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

    private void MoveToLocation(Vector3 walkLocation, float speed){
        _navMeshAgent.speed = speed;
        _navMeshAgent.SetDestination(walkLocation);
    }

    private Vector3 GetPlayerLocation(){
        return _player.transform.position;
    }

    public void PingBrother(PingType ping, Vector3 location){
        Debug.Log("brother ping: " + ping);
        _pingLocation = location;
        CustomEvent.Trigger(this.gameObject, ping.ToString());     
    }

    public void FollowEnter(){
        
    }

    public void FollowUpdate(){
        MoveToLocation(GetPlayerLocation(), _walkSpeed);
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

    public void HideEnter(){
        MoveToLocation(_findHidingSpot.FindBestHidingSpot(), _walkSpeed);
    }

    public void HideUpdate(){
        if(PathCompleted()){
            Debug.Log("Hidden");
        }    
    }

    public void HideFixedUpdate(){

    }

    public void HideExit(){
        
    }

    public void RunEnter(){
        MoveToLocation(_pingLocation, _walkSpeed);
    }

    public void RunUpdate(){
        if(PathCompleted()){
            Debug.Log("Hallo");
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

    public void InteractEnter(){
        MoveToLocation(_pingLocation, _walkSpeed);
    }

    public void InteractUpdate(){
        Debug.Log("Interacting");
    }

    public void InteractFixedUpdate(){

    }

    public void InteractExit(){
        
    }

    public void UseEnter(){
        MoveToLocation(_pingLocation, _walkSpeed);
    }

    public void UseUpdate(){
        Debug.Log("Use");
    }

    public void UseFixedUpdate(){

    }

    public void UseExit(){
        
    }

    public void PickupEnter(){
        MoveToLocation(_pingLocation, _walkSpeed);
    }

    public void PickupUpdate(){
        Debug.Log("Picking up");
    }

    public void PickupFixedUpdate(){

    }

    public void PickupExit(){
        
    }
}
