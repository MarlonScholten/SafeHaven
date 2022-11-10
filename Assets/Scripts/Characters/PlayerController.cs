using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] [Range(1f, 20f)] private float movementSpeed = 5f;
        public float MovementSpeed => movementSpeed;
        public CharacterController CharacterController { get; private set; }
        public PlayerBaseState CurrentState { get; set; }
        public Vector2 Movement { get; private set; }

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
        /// Call the Update function of whatever state we are in on every frame.
        /// </summary>
        void Update()
        {
            CurrentState.UpdateState();
            HandleGravity();
        }
        
        /// <summary>
        /// Update the movement values whenever the player inputs movement keys
        /// </summary>
        /// <param name="context"></param>
        public void OnMove(InputAction.CallbackContext context)
        {
            Movement = context.action.ReadValue<Vector2>();
        }

        /// <summary>
        /// Check to see if the player is currently inputting any movement.
        /// </summary>
        /// <returns>true if the player is pressing movement-related input</returns>
        public bool IsMoving()
        {
            return Movement.x != 0 || Movement.y != 0;
        }

        private void HandleGravity()
        {
            if (CharacterController.isGrounded && _verticalSpeed != 0)
                _verticalSpeed = 0;

            if (!CharacterController.isGrounded)
            {
                _verticalSpeed -= (_gravity * gravityMultiplier) * Time.deltaTime;
                var velocity = new Vector3(0, _verticalSpeed, 0);
                CharacterController.Move(velocity * Time.deltaTime);
            }
        }
    }
}
