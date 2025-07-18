﻿using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public partial class PlayerControl
    {
        [TabGroup("Run")] [SerializeField] [Range(1f, 12f)]
        private float runSpeed = 5f;

        public class Run : Walk
        {
            public override void OnEnter()
            {
                base.OnEnter();
                Host.animator.SetBool(IsRun, true);
            }

            public override void OnExit()
            {
                base.OnExit();
                Host.animator.SetBool(IsRun, false);
            }

            protected override float MoveSpeed()
            {
                return Host.runSpeed;
            }

            internal override void Sprint(InputAction.CallbackContext context)
            {
                if (context.control.IsPressed()) return;
                if (!Host.TryGetState<Walk>(out var state)) return;
                state.Movement = Movement;
                Host.ChangeState<Walk>();
            }
        }
    }
}