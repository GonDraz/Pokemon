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
        }
    }
}