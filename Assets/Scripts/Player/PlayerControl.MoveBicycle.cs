using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public partial class PlayerControl
    {
        [SerializeField] [Range(2f, 20f)] private float moveBicycleSpeed = 12f;

        private class MoveBicycle : Walk
        {
            protected override void NotMovement()
            {
                Host.ChangeState<BicycleStandStill>();
            }

            public override float MoveSpeed()
            {
                return Host.moveBicycleSpeed;
            }

            internal override void Sprint(InputAction.CallbackContext context)
            {
                if (!context.control.IsPressed()) Host.ChangeState<Walk>();
            }
        }
    }
}