using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters
{
    public class PlayerController : MonoBehaviour
    {
        public float movementSpeed;
        public CharacterController CharacterController { get; private set; }
        public PlayerBaseState CurrentState { get; set; }
        public Vector2 Movement { get; private set; }
        private PlayerStateFactory _states;
        
        private void Awake()
        {
            _states = new PlayerStateFactory(this);
            CurrentState = _states.Idle();
            CurrentState.EnterState();
            
            CharacterController = GetComponent<CharacterController>();
        }
        
        void Update()
        {
            CurrentState.UpdateState();
        }
        
        /// <summary>
        /// Update the movement values whenever the player inputs movement keys
        /// </summary>
        /// <param name="context"></param>
        public void Move(InputAction.CallbackContext context)
        {
            Movement = context.action.ReadValue<Vector2>();
        }

        public bool IsMoving()
        {
            return Movement.x != 0 || Movement.y != 0;
        }
    }
}
