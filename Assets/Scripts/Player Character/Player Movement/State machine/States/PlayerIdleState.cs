using Player_Character.Player_Movement.State_machine.General_Scripts;
using Player_Character.Player_Movement.State_machine.State_machines;

namespace Player_Character.Player_Movement.State_machine.States
{
    /// <summary>
    /// Author: Marlon Scholten <br/>
    /// Modified by: --- <br/>
    /// Description: The Idle state for when no input is currently being received.
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
