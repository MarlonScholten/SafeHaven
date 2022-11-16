using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters
{
    /// <summary>
    /// A simple character controller that moves the player.
    /// </summary>
    public class PlayerCharacterController : MonoBehaviour
    {
        [SerializeField] private float _movementSpeed;
        private CharacterController _controller;
        private Vector2 _movement;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if(_movement.x != 0 || _movement.y != 0)
                Move(_movement.x, _movement.y);
        }

        /// <summary>
        /// Update the movement values whenever the player inputs movement keys
        /// </summary>
        /// <param name="context"></param>
        public void OnMove(InputAction.CallbackContext context)
        {
            _movement = context.action.ReadValue<Vector2>();
        }

        /// <summary>
        /// Move the CharacterController in a given direction multiplied by movement speed
        /// </summary>
        /// <param name="x">Horizontal movement (Left/Right)</param>
        /// <param name="z">Horizontal movement (Forwards/Backwards)</param>
        private void Move(float x, float z)
        {
            var direction = new Vector3(x, 0f, z);
            var velocity = direction * _movementSpeed;
            _controller.SimpleMove(velocity);
        }
    }
}
