namespace Characters
{
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
        public override void CheckSwitchStates()
        {
            if(Context.IsMoving())
                SwitchState(Factory.Move());
        }
    }
}
