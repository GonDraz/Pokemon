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