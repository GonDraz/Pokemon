using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public partial class PlayerControl
    {
        [TabGroup("MoveBicycle")] [SerializeField] [Range(2f, 20f)]
        private float bicycleSpeed = 10f;

        private class MoveBicycle : Walk
        {
            protected override void NotMovement()
            {
                Host.animator.SetBool(IsMoving, false);
                Host.ChangeState<BicycleStandStill>();
            }

            internal override void Sprint(InputAction.CallbackContext context)
            {
            }

            protected override float MoveSpeed()
            {
                return Host.bicycleSpeed;
            }
        }
    }
}