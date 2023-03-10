using PlayerCharacter.Movement;
using UnityEngine;

namespace PlayerCharacter.States
{
    /// <summary>
    /// Author: Marlon Scholten <br/>
    /// Modified by: --- <br/>
    /// Description: The move state for the player character that calculates movement and rotation
    /// </summary>
    /// <list type="table">
    ///	    <listheader>
    ///         <term>On what GameObject</term>
    ///         <term>Type</term>
    ///         <term>Name of type</term>
    ///         <term>Description</term>
    ///     </listheader>
    ///     <item>
    ///         <term>None</term>
    ///		    <term>None</term>
    ///         <term>None</term>
    ///		    <term>This is an independent state script</term>
    ///	    </item>
    /// </list>
    public class PlayerMoveState : PlayerBaseState
    {
        private float _turnSmoothVelocity;
        
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

            var rotation = CalculateRotation(Context.MovementInput, out var calculatedAngle);
            SetContextRotation(rotation);
            var movement = CalculateMovement(calculatedAngle);
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
        /// Calculate the movement of the player and apply movement speed.
        /// </summary>
        private Vector3 CalculateMovement(float rotationAngle)
        {
            var direction = Quaternion.Euler(0f, rotationAngle, 0f) * Vector3.forward;
            var movement = direction * Context.MovementSpeed;
            return movement;
        }

        /// <summary>
        /// Calculate the rotation of the player character based on camera orientation.
        /// </summary>
        /// <param name="movementInput">The input related to movement</param>
        /// <param name="calculatedAngle">The undamped/smoothed angle determining our direction</param>
        /// <returns>A smoothed angle determining our rotation</returns>
        private Quaternion CalculateRotation(Vector2 movementInput, out float calculatedAngle)
        {
            var direction = new Vector3(movementInput.x, 0f, movementInput.y);
            var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Context.PlayerCamera.transform.eulerAngles.y;
            var angle = Mathf.SmoothDampAngle(Context.transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, Context.SmoothTurnTime);
            calculatedAngle = targetAngle;
            return Quaternion.Euler(0f, angle, 0f);
        }

        /// <summary>
        /// Set the movement of the context.
        /// </summary>
        private void SetContextMovement(Vector3 movement)
        {
            Context.Movement = movement;
        }
        
        /// <summary>
        /// Set the rotation of the context.
        /// </summary>
        private void SetContextRotation(Quaternion rotation)
        {
            Context.Rotation = rotation;
        }
    }
}
