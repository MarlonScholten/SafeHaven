using Player_Character.Player_Movement.State_machine.State_machines;
using Player_Character.Player_Movement.State_machine.States;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player_Character.Player_Movement.General_scripts
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
        [SerializeField] [Range(0.01f, 0.5f)] private float _smoothTurnTime = 0.1f;
        [SerializeField] private bool _canMoveInAir = true;
        [SerializeField] private float _gravityMultiplier = 1f;
        [SerializeField] private Transform _playerCamera;

        public bool CanMoveInAir => _canMoveInAir;
        public PlayerBaseState CurrentState { get; set; }
        public Vector3 Movement { set => _movement = value; }
        public Vector2 MovementInput { get; private set; }
        public float MovementSpeed => _movementSpeed;
        public Transform PlayerCamera => _playerCamera;
        public Quaternion Rotation {get => _rotation; set => _rotation = value; }
        public float SmoothTurnTime { get => _smoothTurnTime; set => _smoothTurnTime = value; }

        private CharacterController CharacterController { get; set; }
        private Vector3 _movement;
        private const float _gravity = 9.8f;
        private Quaternion _rotation;
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
    }
}
