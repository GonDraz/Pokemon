using UnityEngine;

namespace Player
{
    public partial class PlayerControl
    {
        [SerializeField] [Range(2f, 20f)] private float bicycleSpeed = 12f;

        private class MoveBicycle : Walk
        {
            protected override void NotMovement()
            {
                Host.ChangeState<BicycleStandStill>();
            }

            public override float MoveSpeed()
            {
                return Host.bicycleSpeed;
            }
        }
    }
}