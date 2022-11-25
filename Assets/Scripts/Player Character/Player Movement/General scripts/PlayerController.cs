using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerMovement
{
    /// <summary>
    /// Author: Marlon Scholten <br/>
    /// Modified by: Hugo Verweij <br/>
    /// PlayerController behaviour. Controller for everything related to the player character's state, movement and actions. <br />
    /// Controls the states, and updates the correct parameters when the player inputs movement buttons. <br />
    /// </summary>
    /// <list type="table">
    ///	    <listheader>
    ///         <term>On what GameObject</term>
    ///         <term>Type</term>
    ///         <term>Name of type</term>
    ///         <term>Description</term>
    ///     </listheader>
    ///	    <item>
    ///         <term>The player (No prefab yet.)</term>
    ///		    <term>Script</term>
    ///         <term>PlayerController</term>
    ///		    <term>PlayerController behaviour. Controller for everything related to the player character's state, movement and actions.</term>
    ///	    </item>
    /// </list>
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] [Range(1f, 20f)] private float _movementSpeed = 5f;
        [SerializeField] [Range(1f, 20f)] private float _lookSensitivity = 5f;
        [SerializeField] private bool _canMoveInAir = true;

        public bool CanMoveInAir { get => _canMoveInAir;}
        public float MovementSpeed => _movementSpeed;
        private CharacterController CharacterController { get; set; }
        public PlayerBaseState CurrentState { get; set; }
        public Vector2 MovementInput { get => InputBehaviour.Instance.OnMoveVector; }
        private Vector3 _movement;
        public Vector3 Movement { set => _movement = value; }
        private Vector2 LookInput { get => InputBehaviour.Instance.OnLookVector; }
        private const float _gravity = 9.8f;
        [SerializeField] private float _gravityMultiplier = 1f;
        private PlayerStateFactory _states;
        private float _verticalSpeed;

        private void Awake()
        {
            _states = new PlayerStateFactory(this);
            CurrentState = _states.Idle();
            CurrentState.EnterState();
            
            CharacterController = GetComponent<CharacterController>();
        }

        /// <summary>
        /// Call the Update function of whatever state we are in on every frame and move the player.
        /// </summary>
        /// <remarks>Movement is here too because gravity influences all kinds of movement</remarks>
        void Update()
        {
            CurrentState.UpdateState();
            ApplyGravity();
            Look();
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
        /// Rotate the player based on look input
        /// </summary>
        private void Look()
        {
            transform.Rotate(new Vector3(0f, LookInput.x * _lookSensitivity * Time.deltaTime, 0f));
            _movement = transform.rotation * _movement;
        }

        /// <summary>
        /// Apply gravity to the calculated movement when not grounded.
        /// </summary>
        private void ApplyGravity()
        {
            if (CharacterController.isGrounded && _verticalSpeed != 0)
                _verticalSpeed = 0;

            if (!CharacterController.isGrounded)
            {
                _verticalSpeed -= (_gravity * _gravityMultiplier) * Time.deltaTime;
                _movement.y = _verticalSpeed;
            }
        }
    }
}
