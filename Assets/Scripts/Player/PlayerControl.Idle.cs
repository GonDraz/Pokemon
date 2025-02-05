using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public partial class PlayerControl
    {
        private class Idle : PlayerState
        {
            internal override void Move(InputAction.CallbackContext context)
            {
                if (context.ReadValue<Vector2>() != Vector2.zero) Host.ChangeState<Walk>();
            }

            internal override void Sprint(InputAction.CallbackContext context)
            {
                if (context.control.IsPressed()) Host.ChangeState<Run>();
            }
            
            internal override void Crouch(InputAction.CallbackContext context)
            {
                Host.animator.SetBool(IsBicycle, true);
                Host.ChangeState<BicycleStandStill>();
            }
        }
    }
}