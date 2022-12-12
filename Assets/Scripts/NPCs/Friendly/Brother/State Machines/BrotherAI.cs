using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Author: Jelco van der Straaten </para>
/// Modified by: Thijs Orsel, Iris Giezen and Thomas van den Oever</para>
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
/// <summary>
/// This requireComponent is used for the triggers. The AkComponents needs that the object to trigger the trigger has an rigidbody. But make it kineMatic so it does not affect anything.
/// </summary>
[RequireComponent(typeof(Rigidbody),typeof(Collider))]
public class BrotherAI : MonoBehaviour
{
    /// <summary>
    /// This value determines the maximum walkspeed of the brother.
    /// </summary>
    [Range(2.0f, 4.0f), Tooltip("This value determines the maximum walkspeed of the brother.")] [SerializeField]
    private float _walkSpeed = 3.5f;

    /// <summary>
    /// This value determines the following distance of the brother.
    /// </summary>
    [Range(0.5f, 4.0f), Tooltip("This value determines the following distance of the brother.")] [SerializeField]
    private float _followDistance = 1.5f;

    /// <summary>
    /// This value determines the range in wich a path considers to be completed to get to the next state.
    /// </summary>
    private const float _pathEndThreshold = 0.1f;

    /// <summary>
    /// The navmeshAgent component used for the movement of the brother.
    /// </summary>
    private NavMeshAgent _navMeshAgent;

    /// <summary>
    /// This is the script used for finding hidingSpots.
    /// </summary>
    private FindHidingSpot _findHidingSpot;

    /// <summary>
    /// The pingLocation gets assigned when a ping is made. The last ping location will be saved here.
    /// </summary>
    private Vector3 _pingLocation;

    /// <summary>
    /// This is the sister (the player)
    /// </summary>
    private GameObject _player;
    
    private Animator _animator;
    private int _velocityHash;
    private int _itemHeldHash;
    private int _interactableObjectHash;
    private int _stealthHash;

    [Tooltip("This value determines if the brother is stealth.")]
    [SerializeField] private bool _isStealth = false;

    /// <summary>
    /// In the start method the declaration for the input is made.
    /// </summary>
    void Start()
    {
        InputBehaviour.Instance.OnCallBrotherEvent += CallBrother;
        _velocityHash = Animator.StringToHash("forwardVelocity");
        _itemHeldHash = Animator.StringToHash("ItemHeld");
        _interactableObjectHash = Animator.StringToHash("InteractableObject");
        _stealthHash = Animator.StringToHash("Stealth");
    }

    /// <summary>
    /// In the fixedUpdate the animations variables are updated.
    /// </summary>
    private void FixedUpdate()
    {
        _animator.SetFloat(_velocityHash, _navMeshAgent.velocity.magnitude);
        _animator.SetBool(_stealthHash, _isStealth);
    }

    /// <summary>
    /// When the brother is called back this method makes sure the brother gets back to the follow state.
    /// </summary>
    private void CallBrother()
    {
        CustomEvent.Trigger(this.gameObject, "Follow");
    }

    /// <summary>
    /// This method checks if the path is completed.
    /// </summary>
    private bool PathCompleted()
    {
        return _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance + _pathEndThreshold;
    }

    /// <summary>
    /// This method makes the brother move to a certain location
    /// </summary>
    private void MoveToLocation(Vector3 walkLocation, float speed)
    {
        _navMeshAgent.speed = speed;
        _navMeshAgent.SetDestination(walkLocation);
    }

    /// <summary>
    /// This method gets the current location of the player (sister)
    /// </summary>
    private Vector3 GetPlayerLocation()
    {
        return _player.transform.position;
    }

    /// <summary>
    /// <para>This method is used by the pinging system.</para>
    /// The method gets called when a ping is made. This method changes the state of the brother depending on the ping.
    /// </summary>
    public void PingBrother(PingType ping, Vector3 location)
    {
        _pingLocation = location;
        CustomEvent.Trigger(this.gameObject, ping.ToString());
    }

    /// <summary>
    /// The enter method for the initialize state, it instantiates the state machine.
    /// </summary>
    public void InitializeEnter()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _findHidingSpot = GetComponent<FindHidingSpot>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _animator = GetComponentInChildren<Animator>();
        CustomEvent.Trigger(this.gameObject, "Follow");
    }

    /// <summary>
    /// The update method for the initialize state.
    /// </summary>
    public void InitializeUpdate()
    {
    }

    /// <summary>
    /// The Fixed update method for the initialize state.
    /// </summary>
    public void InitializeFixedUpdate()
    {
    }

    /// <summary>
    /// The exit method for the initialize state.
    /// </summary>
    public void InitializeExit()
    {
    }

    /// <summary>
    /// The enter method for the follow state, it sets the following distance for the brother.
    /// </summary>
    public void FollowEnter(){
        _navMeshAgent.stoppingDistance = _followDistance;  
    }

    /// <summary>
    /// The update method for the follow state
    /// </summary>
    public void FollowUpdate()
    {
        MoveToLocation(GetPlayerLocation(), _walkSpeed);
    }

    /// <summary>
    /// The Fixed update method for the follow state
    /// </summary>
    public void FollowFixedUpdate()
    {
    }

    /// <summary>
    /// The exit method for the follow state, it resets the follow distance.
    /// </summary>
    public void FollowExit()
    {
        _navMeshAgent.stoppingDistance = 0;
    }

    /// <summary>
    /// The enter method for the holding hands state
    /// </summary>
    public void HoldingHandsEnter()
    {
    }

    /// <summary>
    /// The update method for the holding hands state
    /// </summary>
    public void HoldingHandsUpdate()
    {
        MoveToLocation(GetPlayerLocation(), _walkSpeed);
    }

    /// <summary>
    /// The fixed update method for the holding hands state
    /// </summary>
    public void HoldingHandsFixedUpdate()
    {
    }

    /// <summary>
    /// The exit method for the holding hands state
    /// </summary>
    public void HoldingHandsExit()
    {
    }

    /// <summary>
    /// The enter method for the hide state
    /// </summary>
    public void HideEnter()
    {
        MoveToLocation(_findHidingSpot.FindBestHidingSpot(), _walkSpeed);
    }

    /// <summary>
    /// The update method for the hide state
    /// </summary>
    public void HideUpdate(){
        if(PathCompleted())
        {
            _isStealth = true;
        }    
    }

    /// <summary>
    /// The fixed update method for the hide state
    /// </summary>
    public void HideFixedUpdate()
    {
    }

    /// <summary>
    /// The exit method for the hide state
    /// </summary>
    public void HideExit()
    {
        _isStealth = false;
    }

    /// <summary>
    /// The enter method for the run state
    /// </summary>
    public void RunEnter()
    {
        MoveToLocation(_pingLocation, _walkSpeed);
    }

    /// <summary>
    /// The update method for the run state
    /// </summary>
    public void RunUpdate()
    {
        if (PathCompleted())
        {
        }
    }

    /// <summary>
    /// The fixed update method for the run state
    /// </summary>
    public void RunFixedUpdate()
    {
    }

    /// <summary>
    /// The exit method for the run state
    /// </summary>
    public void RunExit()
    {
    }

    /// <summary>
    /// The enter method for the idle state
    /// </summary>
    public void IdleEnter()
    {
        _navMeshAgent.ResetPath();
    }

    /// <summary>
    /// The update method for the idle state
    /// </summary>
    public void IdleUpdate()
    {
    }

    /// <summary>
    /// The fixed update method for the idle state
    /// </summary>
    public void IdleFixedUpdate()
    {
    }

    /// <summary>
    /// The exit method for the idle state
    /// </summary>
    public void IdleExit()
    {
    }

    /// <summary>
    /// The enter method for the interact state
    /// </summary>
    public void InteractEnter()
    {
        MoveToLocation(_pingLocation, _walkSpeed);
    }

    /// <summary>
    /// The update method for the interact state
    /// </summary>
    public void InteractUpdate()
    {
    }

    /// <summary>
    /// The fixed update method for the interact state
    /// </summary>
    public void InteractFixedUpdate()
    {
    }

    /// <summary>
    /// The exit method for the interact state
    /// </summary>
    public void InteractExit()
    {
    }

    /// <summary>
    /// The enter method for the use state
    /// </summary>
    public void UseEnter()
    {
        MoveToLocation(_pingLocation, _walkSpeed);
    }

    /// <summary>
    /// The update method for the use state
    /// </summary>
    public void UseUpdate()
    {
    }

    /// <summary>
    /// The fixed update method for the use state
    /// </summary>
    public void UseFixedUpdate()
    {
    }

    /// <summary>
    /// The exit method for the use state
    /// </summary>
    public void UseExit()
    {
    }

    /// <summary>
    /// The enter method for the pickup
    /// </summary>
    public void PickupEnter()
    {
        MoveToLocation(_pingLocation, _walkSpeed);
    }

    /// <summary>
    /// The update method for the pickup
    /// </summary>
    public void PickupUpdate()
    {
    }

    /// <summary>
    /// The fixed update method for the pickup
    /// </summary>
    public void PickupFixedUpdate()
    {
    }

    /// <summary>
    /// The exit method for the pickup
    /// </summary>
    public void PickupExit()
    {
    }
}