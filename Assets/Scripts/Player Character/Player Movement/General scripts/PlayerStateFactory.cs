using System;
using System.Collections.Generic;
using PlayerCharacter.Movement;

namespace PlayerCharacter.States
{ 
    /// <summary>
    /// Author: Marlon Scholten <br/>
    /// Modified by: --- <br/>
    /// Description: This class initializes all the necessary player-related states so they can easily be called by other code.
    /// </summary>
    /// <remarks>Make sure to add any new states to the dictionary and write a function for easy access.</remarks>
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
    ///		    <term>This is an independent factory script</term>
    ///	    </item>
    /// </list>
    public class PlayerStateFactory
    {
        private readonly Dictionary<String, PlayerBaseState> _states;

        public PlayerStateFactory(PlayerController currentContext)
        {
            _states = new Dictionary<string, PlayerBaseState>
            {
                {"Idle", new PlayerIdleState(currentContext, this)},
                {"Move", new PlayerMoveState(currentContext, this)}
            };
        }

        /// <summary>
        /// Returns the Idle state
        /// </summary>
        /// <returns>The Idle state</returns>
        public PlayerBaseState Idle()
        {
            return _states["Idle"];
        }

        /// <summary>
        /// Returns the Move state
        /// </summary>
        /// <returns>The Move state</returns>
        public PlayerBaseState Move()
        {
            return _states["Move"];
        }
    }
}
