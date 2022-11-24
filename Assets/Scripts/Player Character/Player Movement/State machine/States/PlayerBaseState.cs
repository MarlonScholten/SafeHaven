using Player_Character.Player_Movement.General_scripts;
using Player_Character.Player_Movement.State_machine.State_machines;

namespace Player_Character.Player_Movement.State_machine.States
{
    /// <summary>
    /// The main abstraction of all player-related states.
    /// </summary>
    /// <remarks>When making a new player state, inherit from this class.</remarks>
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
        /// Switch to another state. Calls <see cref="Exit"/> on the old state, <see cref="Enter"/> on the new state.
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
