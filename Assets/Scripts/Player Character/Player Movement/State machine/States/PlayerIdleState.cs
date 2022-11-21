namespace PlayerMovement
{
    /// <summary>
    /// The Idle state for when no input is currently being received.
    /// </summary>
    public class PlayerIdleState : PlayerBaseState
    {
        public PlayerIdleState(PlayerController context, PlayerStateFactory factory)
            : base(context, factory){}

        public override void EnterState()
        {
            // No logic needed here yet.
        }

        public override void UpdateState()
        {
            CheckSwitchStates();
        }

        public override void ExitState()
        {
            // No logic needed here yet.
        }

        /// <summary>
        /// If the player is inputting movement, we should switch to the movement state.
        /// </summary>
        public override bool CheckSwitchStates()
        {
            if (Context.IsMoving() && (Context.IsGrounded() || Context.CanMoveInAir))
            {
                SwitchState(Factory.Move());
                return true;
            }

            return false;
        }
    }
}
