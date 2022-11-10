using System;
using System.Collections.Generic;

namespace Characters
{
    /// <summary>
    /// This class initializes all the necessary player-related states so they can easily be called by other code.
    /// </summary>
    public class PlayerStateFactory
    {
        private readonly Dictionary<String, PlayerBaseState> _states;

        public PlayerStateFactory(PlayerController currentContext)
        {
            var context = currentContext;
            _states = new Dictionary<string, PlayerBaseState>
            {
                {"Idle", new PlayerIdleState(context, this)},
                {"Move", new PlayerMoveState(context, this)}
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
