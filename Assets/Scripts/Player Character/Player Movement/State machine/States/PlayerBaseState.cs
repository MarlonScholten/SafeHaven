using Player_Character.Player_Movement.General_scripts;
using Player_Character.Player_Movement.State_machine.State_machines;

namespace Player_Character.Player_Movement.State_machine.States
{
    /// <summary>
    /// Author: Marlon Scholten <br/>
    /// Modified by: --- <br/>
    /// Description: The main abstraction of all player-related states.
    /// </summary>
    /// <remarks>When making a new player state, inherit from this class.</remarks>
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
    public abstract class PlayerBaseState
    {
        protected PlayerController Context;
        protected PlayerStateFactory Factory;

        public PlayerBaseState(PlayerController currentContext, PlayerStateFactory playerStateFactory)
        {
            Context = currentContext;
            Factory = playerStateFactory;
        }

        /// <summary>
        /// Logic that's called whenever you enter this state
        /// </summary>
        public abstract void EnterState();
        
        /// <summary>
        /// Logic that needs to be called often or every frame while in this state
        /// </summary>
        public abstract void UpdateState();
        
        /// <summary>
        /// Logic that needs to be executed whenever you exit this state.
        /// </summary>
        /// <remarks>Use it for cleaning up any code, coroutines or references you don't need when not in this state</remarks>
        public abstract void ExitState();
        
        /// <summary>
        /// Check to see if this state should swap to another state.
        /// </summary>
        /// <remarks>Usually runs within the <see cref="UpdateState"/> function</remarks>
        /// <returns>true if we are switching states</returns>
        public abstract bool CheckSwitchStates();

        /// <summary>
        /// Switch to another state. Calls <see cref="ExitState"/> on the old state, <see cref="EnterState"/> on the new state.
        /// </summary>
        /// <param name="newState">The state you want to switch to</param>
        protected void SwitchState(PlayerBaseState newState)
        {
            ExitState();
            newState.EnterState();
            Context.CurrentState = newState;
        }
    }
}
