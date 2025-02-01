using System;
using GonDraz.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Animator))]
    public partial class PlayerControl : BaseStateMachine<PlayerControl, PlayerControl.PlayerState>
    {
        [SerializeField] internal Animator animator;
        [SerializeField] private LayerMask solidObjectLayerMask;

#if UNITY_EDITOR

        private void OnValidate()
        {
            animator = GetComponent<Animator>();
        }
#endif

        public void OnMovePlayerInput(InputAction.CallbackContext context)
        {
            GetCurrentState().Move(context);
        }

        public override Type InitialState()
        {
            return typeof(None);
        }

        public abstract class PlayerState : BaseState<PlayerControl, PlayerState>
        {
            internal virtual void Move(InputAction.CallbackContext obj)
            {
            }

            protected bool IsWalkable(Vector3 target)
            {
                return !Physics2D.OverlapCircle(target, 0.1f, Host.solidObjectLayerMask);
            }
        }
        
        public class None : PlayerState
        {
            public override void OnEnter()
            {
                base.OnEnter();
                
                Host.ChangeState<Idle>();
                // GlobalStateMachine.Instance.InitialState().
            }
        }
    }
}