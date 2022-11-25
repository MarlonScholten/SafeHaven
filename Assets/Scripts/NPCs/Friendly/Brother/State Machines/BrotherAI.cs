using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;


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
///         <term>Brother</term>
///		    <term>Navmesh Agent</term>
///         <term>NavmeshAgent</term>
///		    <term>The navmeshAgent controls the movement of the brother</term>
///	    </item>
///     <item>
///         <term>Brother</term>
///		    <term>Script</term>
///         <term>FearSystem</term>
///		    <term>The fearsystem controls the fear of the brother.</term>
///	    </item>
///     <item>
///         <term>Brother</term>
///		    <term>Script</term>
///         <term>FindHidingSpot</term>
///		    <term>The find hiding spot script is needed to get the calculate wich hiding spot is the best when the brother needs to hide.</term>
///	    </item>
///     <item>
///         <term>Sister</term>
///		    <term>Tag</term>
///         <term>Player</term>
///		    <term>The tag is needed to identify the player, so that the brother can follow the player.</term>
///	    </item>
///     <item>
///         <term>Brother</term>
///		    <term>Visual scripting state machine</term>
///         <term>brotherState</term>
///		    <term>The state machine is needed for the management of the states.</term>
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

    void Start(){
        InputBehaviour.Instance.OnCallBrother += CallBrother;
    }

    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _findHidingSpot = gameObject.GetComponent<FindHidingSpot>();
        _fearSystem = GetComponent<FearSystem>();
        _player = GameObject.FindGameObjectWithTag("Player");                
    }
    private void CallBrother(InputAction.CallbackContext ctx){
        CustomEvent.Trigger(this.gameObject, "CallBack");
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

    /// <summary>
    /// This method is used by the pinging system.</para>
    /// The method gets called when a ping is made. This method changes the state of the brother depending on the ping.
    /// </summary>
    public void PingBrother(PingType ping, Vector3 location){
        _pingLocation = location;
        CustomEvent.Trigger(this.gameObject, ping.ToString());     
    }
    /// <summary>
    /// The enter method for the follow state
    /// </summary>
    public void FollowEnter(){
        
    }
    /// <summary>
    /// The update method for the follow state
    /// </summary>
    public void FollowUpdate(){
        MoveToLocation(GetPlayerLocation(), _walkSpeed);
    }

    /// <summary>
    /// The Fixed update method for the follow state
    /// </summary>
    public void FollowFixedUpdate(){

    }

    /// <summary>
    /// The exit method for the follow state
    /// </summary>
    public void FollowExit(){

    }

    /// <summary>
    /// The enter method for the holding hands state
    /// </summary>
    public void HoldingHandsEnter(){
        
    }

    /// <summary>
    /// The update method for the holding hands state
    /// </summary>
    public void HoldingHandsUpdate(){
        MoveToLocation(GetPlayerLocation(), _walkSpeed);
    }

    /// <summary>
    /// The fixed update method for the holding hands state
    /// </summary>
    public void HoldingHandsFixedUpdate(){

    }

    /// <summary>
    /// The exit method for the holding hands state
    /// </summary>
    public void HoldingHandsExit(){

    }

    /// <summary>
    /// The enter method for the hide state
    /// </summary>
    public void HideEnter(){
        MoveToLocation(_findHidingSpot.FindBestHidingSpot(), _walkSpeed);
    }

    /// <summary>
    /// The update method for the hide state
    /// </summary>
    public void HideUpdate(){
        if(PathCompleted()){

        }    
    }

    /// <summary>
    /// The fixed update method for the hide state
    /// </summary>
    public void HideFixedUpdate(){

    }

    /// <summary>
    /// The exit method for the hide state
    /// </summary>
    public void HideExit(){
        
    }

    /// <summary>
    /// The enter method for the run state
    /// </summary>
    public void RunEnter(){
        MoveToLocation(_pingLocation, _walkSpeed);
    }

    /// <summary>
    /// The update method for the run state
    /// </summary>
    public void RunUpdate(){
        if(PathCompleted()){
            
        }
    }

    /// <summary>
    /// The fixed update method for the run state
    /// </summary>
    public void RunFixedUpdate(){

    }

    /// <summary>
    /// The exit method for the run state
    /// </summary>
    public void RunExit(){
        
    }

    /// <summary>
    /// The enter method for the idle state
    /// </summary>
    public void IdleEnter(){
        _navMeshAgent.ResetPath();
    }

    /// <summary>
    /// The update method for the idle state
    /// </summary>
    public void IdleUpdate(){

    }

    /// <summary>
    /// The fixed update method for the idle state
    /// </summary>
    public void IdleFixedUpdate(){

    }

    /// <summary>
    /// The exit method for the idle state
    /// </summary>
    public void IdleExit(){
        
    }

    /// <summary>
    /// The enter method for the interact state
    /// </summary>
    public void InteractEnter(){
        MoveToLocation(_pingLocation, _walkSpeed);
    }

    /// <summary>
    /// The update method for the interact state
    /// </summary>
    public void InteractUpdate(){

    }

    /// <summary>
    /// The fixed update method for the interact state
    /// </summary>
    public void InteractFixedUpdate(){

    }

    /// <summary>
    /// The exit method for the interact state
    /// </summary>
    public void InteractExit(){
        
    }

    /// <summary>
    /// The enter method for the use state
    /// </summary>
    public void UseEnter(){
        MoveToLocation(_pingLocation, _walkSpeed);
    }

    /// <summary>
    /// The update method for the use state
    /// </summary>
    public void UseUpdate(){

    }

    /// <summary>
    /// The fixed update method for the use state
    /// </summary>
    public void UseFixedUpdate(){

    }

    /// <summary>
    /// The exit method for the use state
    /// </summary>
    public void UseExit(){
        
    }

    /// <summary>
    /// The enter method for the pickup
    /// </summary>
    public void PickupEnter(){
        MoveToLocation(_pingLocation, _walkSpeed);
    }

    /// <summary>
    /// The update method for the pickup
    /// </summary>
    public void PickupUpdate(){

    }

    /// <summary>
    /// The fixed update method for the pickup
    /// </summary>
    public void PickupFixedUpdate(){

    }

    /// <summary>
    /// The exit method for the pickup
    /// </summary>
    public void PickupExit(){
        
    }
}
