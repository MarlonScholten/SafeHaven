using System;
using System.Collections;
using PlayerCharacter.States;
using UnityEngine;
using UnityEngine.AI;

namespace PlayerCharacter.Movement
{
    /// <summary>
    /// Author: Marlon Scholten <br/>
    /// Modified by: Hugo Verweij, Hugo Ulfman and Iris Giezen <br/>
    /// Description: PlayerController behaviour. Controller for everything related to the player character's state, movement and actions. <br />
    /// Controls the states, and updates the correct parameters when the player inputs movement buttons. <br />
    /// Installation steps: <br />
    /// 1. Drag the Player prefab into the scene/hierarchy.
    /// 2. Drag the PlayerThirdPersonCamera prefab into the scene/hierarchy.
    /// 3. Select PlayerThirdPersonCamera and drag the Player into the Follow and LookAt properties in the inspector.
    /// 4. Check the table below to configure necessary tags, layers and other needed components.
    /// </summary>
    /// <list type="table">
    ///	    <listheader>
    ///         <term>On what GameObject</term>
    ///         <term>Type</term>
    ///         <term>Name of type</term>
    ///         <term>Description</term>
    ///     </listheader>
    ///     <item>
    ///         <term>Main Camera</term>
    ///		    <term>Component</term>
    ///         <term>CinemachineBrain</term>
    ///		    <term>So Cinemachine can do it's job</term>
    ///	    </item>
    ///	    <item>
    ///         <term>InputManager object / prefab in the scene</term>
    ///		    <term>Object</term>
    ///         <term>InputManager</term>
    ///		    <term>The PlayerController uses this inputmanager for moving the player around</term>
    ///	    </item>
    ///     <item>
    ///         <term>Player character GameObject</term>
    ///		    <term>Component</term>
    ///         <term>CharacterController</term>
    ///		    <term>For moving the player around and checking for grounded state</term>
    ///	    </item>
    ///     <item>
    ///         <term>Player character GameObject</term>
    ///         <term>Component / Script</term>
    ///         <term>PlayerController (Script)</term>
    ///         <term>This controls the movement and rotation of the player</term>
    ///	    </item>
    ///     <item>
    ///         <term>Player character GameObject</term>
    ///         <term>Tag</term>
    ///         <term>Player</term>
    ///         <term>So the Cinemachine collider knows to ignore the player when colliding</term>
    ///	    </item>
    ///      <item>
    ///         <term>Any static environment in the level</term>
    ///         <term>Layer</term>
    ///         <term>StaticEnvironment</term>
    ///         <term>The Cinemachine collider will collide with anything on this layer, preventing clipping through objects and obstructing view of the player</term>
    ///	    </item>
    /// </list>
    [RequireComponent(typeof(NavMeshObstacle))]
    public class PlayerController : MonoBehaviour
    {
        /// <summary>
        /// The event handler for <see cref="PlayerController"/>.
        /// </summary>
        /// <param name="sender"></param>
        public delegate void PlayerControllerEvent(object sender);

        /// <summary>
        /// The <see cref="OnStealthToggle"/> event of the player, fires when the player presses the crouch button.
        /// </summary>
        public event PlayerControllerEvent OnStealthToggle;

        [SerializeField] [Range(1f, 20f)] [Tooltip("How fast the character is able to move through the world")]
        private float _movementSpeed = 5f;

        [SerializeField] [Range(0.01f, 0.5f)] [Tooltip("Smoothing strength for the rotation of the character")]
        private float _smoothTurnTime = 0.1f;

        [SerializeField] [Tooltip("Toggle whether or not the player can move while in the air.")]
        private bool _canMoveInAir = true;

        [SerializeField] [Tooltip("Gravity strength multiplier for faster or slower falling speed")]
        private float _gravityMultiplier = 1f;

        [SerializeField] [Tooltip("The layers that the camera raycast should hit")]
        private LayerMask _camRayCastLayers;

        [SerializeField] [Range(1f, 200f)] [Tooltip("The length of the camera raycast")]
        private float _camRayCastLength = 40f;

        [SerializeField] [Tooltip("Show the raycast as a red line or not, make sure gizmos are acivated to see it!")]
        private bool DrawRayDebug = false;

        [SerializeField] [Tooltip("Speed of the player when in stealth.")]
        private float _stealthSpeed = 2.0f;

        [SerializeField] [Tooltip("Speed of the player when moving.")]
        private float _moveSpeed = 4.0f;

        [SerializeField] [Tooltip("Speed of the player when running.")]
        private float _runningSpeed = 6.0f;

        [Header("References")] [SerializeField]
        private GameObject _standCollider;

        [SerializeField] private GameObject _crouchCollider;

        public bool CanMoveInAir => _canMoveInAir;

        /// <summary>
        /// Access this raycast to get the info of what the player cam is looking at.
        /// Change the layermask (in the inspector) to include any layers your environment or interactable items live on.
        /// </summary>
        public RaycastHit CamRayCastHit => _camRayCastHit;

        public PlayerBaseState CurrentState { get; set; }

        public Vector3 Movement
        {
            set => _movement = value;
        }

        public Vector2 MovementInput
        {
            get => InputBehaviour.Instance.OnMoveVector;
        }

        public float MovementSpeed => _movementSpeed;
        public Camera PlayerCamera => _playerCamera;

        public Quaternion Rotation
        {
            set => _rotation = value;
        }

        public float SmoothTurnTime => _smoothTurnTime;

        private CharacterController CharacterController { get; set; }
        private Vector3 _movement;
        private Camera _playerCamera;
        private const float _gravity = 9.8f;
        private Quaternion _rotation;
        private PlayerStateFactory _states;
        private float _verticalSpeed;
        private Ray _playerCamRay;
        private RaycastHit _camRayCastHit;

        private Animator _animator;
        private int _velocityHash;
        private int _itemHeldHash;
        private int _interactableObjectHash;
        private int _stealthHash;

        private bool _crouching;
        private Vector2 _current;
        private Vector2 _smooth;

        private bool _running;

        private void Awake()
        {
            _states = new PlayerStateFactory(this);
            CurrentState = _states.Idle();
            CurrentState.EnterState();

            CharacterController = GetComponent<CharacterController>();
            _playerCamera = Camera.main;
            _playerCamRay = PlayerCamera.ScreenPointToRay(Input.mousePosition);
            _animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            InputBehaviour.Instance.OnToggleStealthEvent += Crouch;
            InputBehaviour.Instance.OnRunningEvent += Running;
            InputBehaviour.Instance.OnRunningCancelledEvent += RunningCancelled;

            StartCoroutine(CastLookingRay());
            _velocityHash = Animator.StringToHash("forwardVelocity");
            _itemHeldHash = Animator.StringToHash("ItemHeld");
            _interactableObjectHash = Animator.StringToHash("InteractableObject");
            _stealthHash = Animator.StringToHash("Stealth");
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        /// <summary>
        /// Call the Update function of whatever state we are in on every frame and move the player according to rotation.
        /// </summary>
        /// <remarks>Movement is here too because gravity influences all kinds of movement</remarks>
        void Update()
        {
            _current = Vector2.SmoothDamp(_current, MovementInput * _movementSpeed, ref _smooth, .3f);

            CurrentState.UpdateState();
            ApplyGravity();
            transform.rotation = _rotation;
            CharacterController.Move(_movement * Time.deltaTime);
            _animator.SetFloat(_velocityHash, _current.magnitude);
            _animator.SetLayerWeight(_animator.GetLayerIndex("BasicLocomotion"), _crouching ? 0 : 1);
            _animator.SetLayerWeight(_animator.GetLayerIndex("Stealth"), _crouching ? 1 : 0);
        }

        /// <summary>
        /// Check to see if the player is currently inputting any movement.
        /// </summary>
        /// <returns>true if the player is pressing movement-related input</returns>
        public bool IsMoving()
        {
            return MovementInput.x != 0 || MovementInput.y != 0;
        }

        /// <summary>
        /// Adjusts the speed of the player, animation and the collider when running.
        /// Activated by the player when holding shift.
        /// </summary>
        private void Running()
        {
            if (!IsMoving()) return;
            _running = true;
            _crouching = false;

            _animator.SetBool("Stealth", _crouching);

            _crouchCollider.SetActive(_crouching);
            _standCollider.SetActive(!_crouching);

            _movementSpeed = _runningSpeed;
        }

        /// <summary>
        /// Adjusts the speed of the player when stopping with running.
        /// Activated by the player when releasing shift.
        /// </summary>
        private void RunningCancelled()
        {
            _running = false;
            _movementSpeed = _moveSpeed;
        }

        /// <summary>
        /// adjusts the movement speed of the player if the OnToggleStealthEvent is invoked.
        /// </summary>
        private void Crouch()
        {
            if (_running.Equals(true)) return;
            _crouching = !_crouching;

            OnStealthToggle?.Invoke(_crouching);

            _running = false;
            _animator.SetBool("Stealth", _crouching);
            _crouchCollider.SetActive(_crouching);
            _standCollider.SetActive(!_crouching);

            if (_crouching)
                _movementSpeed = _stealthSpeed;
            else if (_running)
                _movementSpeed = _runningSpeed;
            else
                _movementSpeed = _moveSpeed;
        }

        /// <summary>
        /// Expose the Character Controller's isGrounded property.
        /// </summary>
        /// <remarks>This method avoids exposing the whole character controller.</remarks>
        /// <returns>true if the controller is grounded</returns>
        public bool IsGrounded()
        {
            return CharacterController.isGrounded;
        }

        /// <summary>
        /// Apply gravity to the calculated movement when not grounded.
        /// </summary>
        /// <remarks>Apparently while moving, the CharacterController considers itself not grounded</remarks>
        private void ApplyGravity()
        {
            if (CharacterController.isGrounded && _verticalSpeed != 0)
                _verticalSpeed = 0;

            if (!CharacterController.isGrounded)
            {
                _verticalSpeed -= _gravity * _gravityMultiplier;
                _movement.y = _verticalSpeed;
            }
        }

        /// <summary>
        /// Constanly casts a ray from the center of the screen into the gameworld, hitting anyhting in the mask.
        /// </summary>
        private IEnumerator CastLookingRay()
        {
            while (true)
            {
                RaycastHit hit;
                _playerCamRay = new Ray(PlayerCamera.transform.position, PlayerCamera.transform.forward);
                if (Physics.Raycast(_playerCamRay, out hit, _camRayCastLength, _camRayCastLayers))
                {
                    _camRayCastHit = hit;
                    if (DrawRayDebug)
                        Debug.DrawRay(_playerCamRay.origin, _playerCamera.transform.forward * _camRayCastLength,
                            Color.red);
                }
                else
                {
                    var nullCastHit = new RaycastHit();
                    _camRayCastHit = nullCastHit;
                }

                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}