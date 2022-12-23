using PlayerCharacter.Movement;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Author: Jelco van der Straaten </para>
/// Modified by: Thijs Orsel and Iris Giezen, Thomas van den Oever </para>
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

[RequireComponent(typeof(FindHidingSpot),typeof(FearSystem))]
public class BrotherAI : MonoBehaviour
{
    /// <summary>
    /// This value determines the maximum walkspeed of the brother.
    /// </summary>
    [Range(2.0f, 4.0f), Tooltip("This value determines the maximum walkspeed of the brother.")] [SerializeField]
    private float _walkSpeed;

    /// <summary>
    /// This value determines the height of the brother collider.
    /// </summary>
    [Range(0f, 2.0f), Tooltip("This value determines the height of the brother collider when in stealth.")]
    [SerializeField]
    private float _colliderHeightStealth = 0.5f;

    [Range(0f, 2.0f), Tooltip("This value determines the height of the brother collider when not in stealth.")]
    [SerializeField]
    private float _colliderHeightBase = 1f;

    /// <summary>
    /// This value shows if the brother is currently in stealth mode, this should also be used by a sound script to make footsteps less loud during stealth.
    /// </summary>
    [SerializeField] private bool _isInStealth = false;

    /// <summary>
    /// This value represents the speed the brother must move at during stealth.
    /// </summary>
    [Range(2.0f, 4.0f), Tooltip("This value determines the maximum walkspeed of the brother during stealth.")]
    [SerializeField]
    private float _stealthSpeed = 2.0f;

    /// <summary>
    /// This value represents the speed the brother must move at when not in stealth.
    /// </summary>
    [Range(2.0f, 10.0f), Tooltip("This value determines the maximum walkspeed of the brother when not in stealth.")]
    [SerializeField]
    private float _baseSpeed = 5.0f;

    /// <summary>
    /// This value determines the following distance of the brother.
    /// </summary>
    [Range(0.5f, 4.0f), Tooltip("This value determines the following distance of the brother.")]
    [SerializeField] private float _followDistance = 1.5f;

    /// <summary>
    /// This value determines the max distance between the brother and player.
    /// If distance is greater than this distance, the brother will follow again.
    /// </summary>
    [Range(10.0f, 50.0f), Tooltip("This value determines the max distance between the brother and player. If distance is greater than this distance, the brother will follow again.")]
    public float _brotherRange = 25f;
    /// <summary>
    /// This value determines the range in wich a path considers to be completed to get to the next state.
    /// </summary>
    private const float _pathEndThreshold = 0.1f;

    /// <summary>
    /// Used to store a reference to the capsule collider of the brother
    /// </summary>
    private CapsuleCollider _capsuleCollider;

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
    
    /// <summary>
    /// This contains a reference to the playerController script
    /// </summary>
    private PlayerController _playerController;

    /// <summary>
    /// A bool to check if this is the first time the script has started
    /// </summary>
    private bool _firstStart = true;

    /// <summary>
    /// Governs if the little brother should be allowed to go into stealth upon the player's invokation of <see cref="OnStealthEvent(object)"/>.
    /// </summary>
    private bool _canStealth = true;

    private Animator _animator;
    private int _velocityHash;
    private int _itemHeldHash;
    private int _interactableObjectHash;
    private int _stealthHash;

    /// <summary>
    /// In the start method the declaration for the input is made.
    /// </summary>
    void Start(){
        InputBehaviour.Instance.OnCallBrotherEvent += CallBrother;
        _playerController.OnStealthToggle += OnStealthEvent;
        _velocityHash = Animator.StringToHash("forwardVelocity");
        _itemHeldHash = Animator.StringToHash("ItemHeld");
        _interactableObjectHash = Animator.StringToHash("InteractableObject");
        _stealthHash = Animator.StringToHash("Stealth");
    }

    /// <summary>
    /// When the player presses the stealth button, this method gets called to determine if the brother should enter stealth mode.
    /// It slows the player and decreases the collider size when in stealth mode.
    /// </summary>
    private void OnStealthEvent(object sender)
    {
        if (sender is bool stealth)
            ToggleStealth(stealth);
    }

    /// <summary>
    /// Checks in stealth mode or not and based on that the speed and the collider height gets set.
    /// </summary>
    /// /// <param name="isPlayerInStealth">Bool to know if the player is in stealth or not. Based on that the brother goes in stealth or not.</param>
    private void ToggleStealth(bool isPlayerInStealth)
    {
        // Return if the brother is doing an action that prevents him from going in and out of stealth upon the player's crouch invokation.
        if (!_canStealth)
            return;

        _isInStealth = isPlayerInStealth;
        if (_isInStealth)
        {
            _capsuleCollider.SetCapsuleCollider(_colliderHeightStealth, 0, 0.25f, 0);
            _walkSpeed = _stealthSpeed;
        }
        else
        {
            _walkSpeed = _baseSpeed;
            _capsuleCollider.SetCapsuleCollider(_colliderHeightBase, 0, 0.5f, 0);
        }
    }

    private void FixedUpdate()
    {
        _animator.SetFloat(_velocityHash, _navMeshAgent.velocity.magnitude);
        _animator.SetBool(_stealthHash, _isInStealth);
        
        if (Vector3.Distance(_player.transform.position, transform.position) > _brotherRange)
        {
            CallBrother();
        }
    }

    /// <summary>
    /// When the brother is called back this method makes sure the brother gets back to the follow state.
    /// </summary>
    private void CallBrother(){
        CustomEvent.Trigger(this.gameObject, "Follow");
    }

    /// <summary>
    /// This method checks if the path is completed.
    /// </summary>
    public bool PathCompleted(){
        return _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance + _pathEndThreshold;
    }

    /// <summary>
    /// This method makes the brother move to a certain location
    /// </summary>
    private void MoveToLocation(Vector3 walkLocation, float speed){
        _navMeshAgent.speed = speed;
        _navMeshAgent.SetDestination(walkLocation);
    }

    /// <summary>
    /// This method gets the current location of the player (sister)
    /// </summary>
    private Vector3 GetPlayerLocation(){
        return _player.transform.position;
    }

    /// <summary>
    /// <para>This method is used by the pinging system.</para>
    /// The method gets called when a ping is made. This method changes the state of the brother depending on the ping.
    /// If no hiding spot near, follow brother.
    /// </summary>
    public void PingBrother(PingType ping, Vector3 location)
    {
        _pingLocation = location;
        switch (ping)
        {
            case PingType.Move when _isInStealth:
                CustomEvent.Trigger(this.gameObject, "PassiveHide");
                break;
            case PingType.Hide when _findHidingSpot.FindBestHidingSpot(_player.transform.position).Equals(new Vector3()):
                CustomEvent.Trigger(this.gameObject, "Follow");
                break;
            case PingType.Interact:
                break;
            default:
                CustomEvent.Trigger(this.gameObject, ping.ToString());
                break;
        }
    }

    /// <summary>
    /// The enter method for the initialize state, it instantiates the state machine.
    /// </summary>
    public void InitializeEnter()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _findHidingSpot = GetComponent<FindHidingSpot>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
        _animator = GetComponentInChildren<Animator>();
        _walkSpeed = _baseSpeed;
        _playerController = _player.GetComponent<PlayerController>();

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

    public void FollowEnter()
    {
        _navMeshAgent.stoppingDistance = _followDistance;
    }
    
    /// <summary>
    /// The update method for the follow state
    /// </summary>
    public void FollowUpdate(){
        MoveToLocation(GetPlayerLocation(),_walkSpeed);
    }

    /// <summary>
    /// The Fixed update method for the follow state
    /// </summary>
    public void FollowFixedUpdate(){

    }

    /// <summary>
    /// The exit method for the follow state, it resets the follow distance.
    /// </summary>
    public void FollowExit(){
        _navMeshAgent.stoppingDistance = 0;
    }

    /// <summary>
    /// The enter method for the hide state
    /// </summary>
    public void HideEnter()
    {
        MoveToLocation(_findHidingSpot.FindBestHidingSpot(_player.transform.position), _walkSpeed);

        // Disallow manual stealth/crouch requests.
        _canStealth = false;

        if (!_isInStealth)
        {
            ToggleStealth(true);
        }
    }

    /// <summary>
    /// The update method for the hide state
    /// </summary>
    public void HideUpdate()
    {
    }

    /// <summary>
    /// The fixed update method for the hide state
    /// </summary>
    public void HideFixedUpdate(){

    }

    /// <summary>
    /// The exit method for the hide state
    /// </summary>
    public void HideExit()
    {
        Debug.Log("Exit hide stealth state before: " + _isInStealth);
        if (_isInStealth)
        {
            ToggleStealth(false);
        }

        // Allow manual stealth/crouch requests.
        _canStealth = true;
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
            CustomEvent.Trigger(this.gameObject, "Idle");  
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
    /// The enter method for the passive hide state
    /// </summary>
    public void PassiveHideEnter(){
        MoveToLocation(_pingLocation, _walkSpeed);
        _isInStealth = true;
    }

    /// <summary>
    /// The update method for the passive hide state
    /// </summary>
    public void PassiveHideUpdate(){

    }

    /// <summary>
    /// The fixed update method for the passive hide state
    /// </summary>
    public void PassiveHideFixedUpdate(){

    }

    /// <summary>
    /// The exit method for the passive hide state
    /// </summary>
    public void PassiveHideExit(){
        if (_isInStealth)
        {
            ToggleStealth(false);
        }
    }
}