using UnityEngine;

namespace Characters
{
    /// <summary>
    /// The Move state is active when movement-related input is received.
    /// </summary>
    public class PlayerMoveState : PlayerBaseState
    {
        public PlayerMoveState(PlayerController context, PlayerStateFactory factory) 
            : base(context, factory){}

        public override void EnterState()
        {
            // No logic needed here yet.
        }

        /// <summary>
        /// Set the context movement if we don't have to switch states.
        /// </summary>
        public override void UpdateState()
        {
            if(CheckSwitchStates())
                return;

            var movement = CalculateMovement(Context.MovementInput);
            SetContextMovement(movement);
        }

        public override void ExitState()
        {
            SetContextMovement(Vector3.zero);
        }
        
        /// <summary>
        /// If the player is not providing input, we should switch to the idle state.
        /// </summary>
        /// <returns>true if we are switching states</returns>
        public override bool CheckSwitchStates()
        {
            if (!Context.IsMoving() || (!Context.IsGrounded() && !Context.CanMoveInAir))
            {
                SwitchState(Factory.Idle());
                return true;
            }

            return false;
        }

        /// <summary>
        /// Calculate the movement of the player.
        /// </summary>
        /// <param name="movementInput">The movement input regarding the x and z axis</param>
        private Vector3 CalculateMovement(Vector2 movementInput)
        {
            var direction = new Vector3(movementInput.x, 0f, movementInput.y);
            var movement = direction * Context.MovementSpeed;
            return movement;
        }

        /// <summary>
        /// Set the movement of the context.
        /// </summary>
        private void SetContextMovement(Vector3 movement)
        {
            Context.Movement = movement;
        }
    }
}
