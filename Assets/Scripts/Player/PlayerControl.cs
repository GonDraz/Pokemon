using System;
using GonDraz.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Animator))]
    public partial class PlayerControl : BaseStateMachine<PlayerControl, PlayerControl.PlayerState>
    {
        private static readonly int MoveX = Animator.StringToHash("MoveX");
        private static readonly int MoveY = Animator.StringToHash("MoveY");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int IsRun = Animator.StringToHash("IsRun");
        private static readonly int IsBicycle = Animator.StringToHash("IsBicycle");

        [SerializeField] internal Animator animator;

        [SerializeField] private LayerMask solidObjectLayerMask;

        [SerializeField] private LayerMask grassLayerMask;

        [SerializeField] private LayerMask waterLayerMask;
        private bool _isMoving;

#if UNITY_EDITOR

        private void OnValidate()
        {
            animator = GetComponent<Animator>();
        }
#endif

        public void OnSprintPlayerInput(InputAction.CallbackContext context)
        {
            GetCurrentState().Sprint(context);
        }

        public void OnMovePlayerInput(InputAction.CallbackContext context)
        {
            GetCurrentState().Move(context);
        }

        public void OnCrouchPlayerInput(InputAction.CallbackContext context)
        {
            GetCurrentState().Crouch(context);
        }

        public override Type InitialState()
        {
            return typeof(None);
        }

        public abstract class PlayerState : BaseState<PlayerControl, PlayerState>
        {
            internal virtual void Move(InputAction.CallbackContext context)
            {
            }

            internal virtual void Sprint(InputAction.CallbackContext context)
            {
            }

            internal virtual void Crouch(InputAction.CallbackContext context)
            {
            }

            public override void OnEnter()
            {
                base.OnEnter();
                GlobalStateMachine.Instance.RegisterEvent<GlobalStateMachine.InGameState>(EventState.Exit,
                    OnInGameExit);
            }

            public override void OnExit()
            {
                base.OnExit();
                GlobalStateMachine.Instance.UnregisterEvent<GlobalStateMachine.InGameState>(EventState.Exit,
                    OnInGameExit);
            }

            private void OnInGameExit()
            {
                Host.ChangeState<None>();
            }
        }

        public class None : PlayerState
        {
            public override void OnEnter()
            {
                base.OnEnter();
                GlobalStateMachine.Instance.RegisterEvent<GlobalStateMachine.InGameState>(EventState.Enter,
                    OnStartGame);
            }

            public override void OnExit()
            {
                base.OnExit();
                GlobalStateMachine.Instance.UnregisterEvent<GlobalStateMachine.InGameState>(EventState.Enter,
                    OnStartGame);
            }

            private void OnStartGame()
            {
                if (Host.CanBack())
                    Host.BackToPreviousState();
                else
                    Host.ChangeState<Idle>(false);
            }
        }
    }
}