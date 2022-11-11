using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] [Range(1f, 20f)] private float movementSpeed = 5f;
        [SerializeField] [Range(1f, 20f)] private float lookSensitivity = 5f;
        public float MovementSpeed => movementSpeed;
        public CharacterController CharacterController { get; private set; }
        public PlayerBaseState CurrentState { get; set; }
        public Vector2 MovementInput { get; private set; }
        private Vector3 _movement;
        public Vector3 Movement { set => _movement = value; }
        private Vector2 LookInput { get; set; }

        private readonly float _gravity = 9.8f;
        [SerializeField] private float gravityMultiplier = 1f;
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
        /// Update the movement values whenever the player inputs movement keys
        /// </summary>
        /// <param name="context"></param>
        public void OnMovementInput(InputAction.CallbackContext context)
        {
            MovementInput = context.action.ReadValue<Vector2>();
        }

        /// <summary>
        /// Look input to rotate the camera.
        /// </summary>
        public void OnLookInput(InputAction.CallbackContext context)
        {
            LookInput = context.ReadValue<Vector2>();
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
        /// Rotate the player based on look input
        /// </summary>
        private void Look()
        {
            transform.Rotate(new Vector3(0f, LookInput.x * lookSensitivity * Time.deltaTime, 0f));
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
                _verticalSpeed -= (_gravity * gravityMultiplier) * Time.deltaTime;
                _movement.y = _verticalSpeed;
            }
        }
    }
}
