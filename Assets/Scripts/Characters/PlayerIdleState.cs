using UnityEditor.Rendering.LookDev;
using UnityEngine;

namespace Characters
{
    public class PlayerIdleState : PlayerBaseState
    {
        public PlayerIdleState(PlayerController context, PlayerStateFactory factory)
            : base(context, factory){}

        public override void EnterState()
        {
        }

        public override void UpdateState()
        {
            CheckSwitchStates();
        }

        public override void ExitState()
        {
        }

        public override void CheckSwitchStates()
        {
            if(Ctx.IsMoving())
                SwitchState(Factory.Move());
        }
    }
}
