using UnityEngine;

namespace Characters
{
    public class PlayerMoveState : PlayerBaseState
    {
        public PlayerMoveState(PlayerController context, PlayerStateFactory factory) 
            : base(context, factory){}

        public override void EnterState()
        {
            // No logic needed here yet.
        }

        /// <summary>
        /// Move the character if we don't need to switch states.
        /// </summary>
        public override void UpdateState()
        {
            CheckSwitchStates();
            Move(Context.Movement);
        }

        public override void ExitState()
        {
            // No logic needed here yet.
        }
        
        /// <summary>
        /// If the player is not providing input, we should switch to the idle state.
        /// </summary>
        public override void CheckSwitchStates()
        {
            if(!Context.IsMoving())
                SwitchState(Factory.Idle());
        }

        /// <summary>
        /// Move the player character in a direction, based on input, with the Context's movement speed.
        /// </summary>
        /// <param name="movement">The movement input regarding the x and z axis</param>
        private void Move(Vector2 movement)
        {
            var direction = new Vector3(movement.x, 0f, movement.y);
            var velocity = direction * Context.MovementSpeed;
            Context.CharacterController.SimpleMove(velocity);
        }
    }
}
