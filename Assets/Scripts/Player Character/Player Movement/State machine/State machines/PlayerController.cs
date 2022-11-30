using System.Collections;
using Player_Character.Player_Movement.State_machine.General_Scripts;
using Player_Character.Player_Movement.State_machine.States;
using UnityEngine;

namespace Player_Character.Player_Movement.State_machine.State_machines
{
    /// <summary>
    /// Author: Marlon Scholten <br/>
    /// Modified by: Hugo Verweij, Hugo Ulfman <br/>
    /// Description: PlayerController behaviour. Controller for everything related to the player character's state, movement and actions. <br />
    /// Controls the states, and updates the correct parameters when the player inputs movement buttons. <br />
    /// Installation steps: <br />
    /// 1. Drag the Player prefab and PlayerThirdPersonCamera prefabs into the scene. <br />
    /// 2. Select Player prefab in scene and drag the PlayerThirdpersonCamera into PlayerController component > Player Camera <br />
    /// 3. Select the PlayerThirdpersonCamera prefab in the scene and drag the player(in the scene) into the Follow and Look At
    /// </summary>
    /// <list type="table">
    ///	    <listheader>
    ///         <term>On what GameObject</term>
    ///         <term>Type</term>
    ///         <term>Name of type</term>
    ///         <term>Description</term>
    ///     </listheader>
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
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] [Range(1f, 20f)]
        [Tooltip("How fast the character is able to move through the world")]
        private float _movementSpeed = 5f;
        [SerializeField] [Range(0.01f, 0.5f)]
        [Tooltip("Smoothing strength for the rotation of the character")]
        private float _smoothTurnTime = 0.1f;
        [SerializeField]
        [Tooltip("Toggle whether or not the player can move while in the air.")]
        private bool _canMoveInAir = true;
        [SerializeField]
        [Tooltip("Gravity strength multiplier for faster or slower falling speed")]
        private float _gravityMultiplier = 1f;
        [SerializeField]
        [Tooltip("The camera that is responsible for keeping the player in view")]
        private Camera _playerCamera;
        [SerializeField]
        [Tooltip("The layers that the camera raycast should hit")]
        private LayerMask _camRayCastLayers;
        [SerializeField] [Range(1f, 200f)]
        [Tooltip("The length of the camera raycast")]
        private float _camRayCastLength = 40f;
        [SerializeField]
        [Tooltip("Show the raycast as a red line or not, make sure gizmos are acivated to see it!")]
        private bool DrawRayDebug = false;

        public bool CanMoveInAir => _canMoveInAir;
        /// <summary>
        /// Access this raycast to get the info of what the player cam is looking at.
        /// Change the layermask (in the inspector) to include any layers your environment or interactable items live on.
        /// </summary>
        public RaycastHit CamRayCastHit => _camRayCastHit;
        public PlayerBaseState CurrentState { get; set; }
        public Vector3 Movement { set => _movement = value; }
        public Vector2 MovementInput { get => InputBehaviour.Instance.OnMoveVector; }
        public float MovementSpeed => _movementSpeed;
        public Camera PlayerCamera => _playerCamera;
        public Quaternion Rotation {set => _rotation = value; }
        public float SmoothTurnTime => _smoothTurnTime;

        private CharacterController CharacterController { get; set; }
        private Vector3 _movement;
        private const float _gravity = 9.8f;
        private Quaternion _rotation;
        private PlayerStateFactory _states;
        private float _verticalSpeed;
        private Ray _playerCamRay;
        private RaycastHit _camRayCastHit;

        private bool _crouching;

        private void Awake()
        {
            _states = new PlayerStateFactory(this);
            CurrentState = _states.Idle();
            CurrentState.EnterState();
            
            CharacterController = GetComponent<CharacterController>();
            _playerCamRay = PlayerCamera.ScreenPointToRay(Input.mousePosition);
        }

        private void Start()
        {
            InputBehaviour.Instance.OnToggleStealthEvent += Crouch;
            StartCoroutine(CastLookingRay());
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
            CurrentState.UpdateState();
            ApplyGravity();
            transform.rotation = _rotation;
            CharacterController.Move(_movement * Time.deltaTime);
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
        /// adjusts the movement speed of the player if the OnToggleStealthEvent is invoked.
        /// </summary>
        private void Crouch() {
            _crouching = !_crouching;
            if (_crouching) {
                _movementSpeed = 2f;
            } else {
                _movementSpeed = 5f;
            }
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
                _playerCamRay = PlayerCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(_playerCamRay, out hit, _camRayCastLength, _camRayCastLayers))
                {
                    _camRayCastHit = hit;
                    if(DrawRayDebug)
                        Debug.DrawLine(_playerCamRay.origin, _camRayCastHit.point, Color.red);
                }

                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}
