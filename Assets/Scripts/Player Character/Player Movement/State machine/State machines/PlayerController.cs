using System.Collections;
using PlayerCharacter.States;
using UnityEngine;

namespace PlayerCharacter.Movement
{
    /// <summary>
    /// Author: Marlon Scholten <br/>
    /// Modified by: Hugo Verweij <br/>
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
        private Camera _playerCamera;
        private const float _gravity = 9.8f;
        private Quaternion _rotation;
        private PlayerStateFactory _states;
        private float _verticalSpeed;
        private Ray _playerCamRay;
        private RaycastHit _camRayCastHit;

        private void Awake()
        {
            _states = new PlayerStateFactory(this);
            CurrentState = _states.Idle();
            CurrentState.EnterState();
            
            CharacterController = GetComponent<CharacterController>();
            _playerCamRay = PlayerCamera.ScreenPointToRay(Input.mousePosition);
            _playerCamera = Camera.main;
        }

        private void Start()
        {
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
