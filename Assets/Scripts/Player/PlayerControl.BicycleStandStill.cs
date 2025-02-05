using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public partial class PlayerControl
    {
        private class BicycleStandStill : PlayerState
        {
            internal override void Move(InputAction.CallbackContext context)
            {
                if (context.ReadValue<Vector2>() != Vector2.zero) Host.ChangeState<MoveBicycle>();
            }

            internal override void Crouch(InputAction.CallbackContext context)
            {
                Host.animator.SetBool(IsBicycle, false);
                Host.ChangeState<Idle>();
            }
        }
    }
}