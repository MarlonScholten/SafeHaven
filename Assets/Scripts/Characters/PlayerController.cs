using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters
{
    public class PlayerController : MonoBehaviour
    {
        public float MovementSpeed => _movementSpeed;
        public CharacterController CharacterController { get; private set; }
        public PlayerBaseState CurrentState { get; set; }
        public Vector2 Movement { get; private set; }
        
        private float _movementSpeed;
        private PlayerStateFactory _states;

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
    }
}
