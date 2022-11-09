using System;
using System.Collections.Generic;

namespace Characters
{
    public class PlayerStateFactory
    {
        private PlayerController _context;
        private Dictionary<String, PlayerBaseState> _states;

        public PlayerStateFactory(PlayerController currentContext)
        {
            _context = currentContext;
            _states = new Dictionary<string, PlayerBaseState>
            {
                {"Idle", new PlayerIdleState(_context, this)},
                {"Move", new PlayerMoveState(_context, this)}
            };
        }

        public PlayerBaseState Idle()
        {
            return _states["Idle"];
        }

        public PlayerBaseState Move()
        {
            return _states["Move"];
        }
    }
}
