using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters
{
    public class PlayerCharacterController : MonoBehaviour
    {
        private CharacterController _controller;
        [SerializeField]
        private float movementSpeed;
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

        public void Move(InputAction.CallbackContext context)
        {
            
        }
        
        /// <summary>
        /// Move the CharacterController in a given direction multiplied by movement speed
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        private void Move(float x, float z)
        {
            var direction = new Vector3(x, 0f, z);
            var velocity = direction * movementSpeed;
            _controller.SimpleMove(velocity);
        }
    }
}
