using UnityEngine;

namespace Characters
{
    public class PlayerMoveState : PlayerBaseState
    {
        public PlayerMoveState(PlayerController currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory){}

        public override void EnterState(){}

        public override void UpdateState()
        {
            CheckSwitchStates();
            Move(Ctx.Movement);
        }

        public override void ExitState() {}

        public override void CheckSwitchStates()
        {
            if(!Ctx.IsMoving())
                SwitchState(Factory.Idle());
        }

        private void Move(Vector2 movement)
        {
            var direction = new Vector3(movement.x, 0f, movement.y);
            var velocity = direction * Ctx.movementSpeed;
            Ctx.CharacterController.SimpleMove(velocity);
        }
    }
}
