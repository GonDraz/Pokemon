using System;
using GonDraz.StateMachine;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public partial class PlayerControl : BaseStateMachine<PlayerControl, PlayerControl.PlayerState>
    {
        private static readonly int MoveX = Animator.StringToHash("MoveX");
        private static readonly int MoveY = Animator.StringToHash("MoveY");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int IsRun = Animator.StringToHash("IsRun");
        private static readonly int IsBicycle = Animator.StringToHash("IsBicycle");

        [TabGroup("General")] [SerializeField] private Rigidbody2D rigidbody;

        [TabGroup("Animator")] [SerializeField]
        private Animator animator;

        [TabGroup("Animator")] [SerializeField]
        private RuntimeAnimatorController maleController;

        [TabGroup("Animator")] [SerializeField]
        private RuntimeAnimatorController femaleController;


        [SerializeField] private LayerMask solidObjectLayerMask;

        [SerializeField] private LayerMask grassLayerMask;

        [SerializeField] private LayerMask waterLayerMask;
        private bool _isMoving;


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

        private enum Gender
        {
            Other,
            Male,
            Female
        }

        public class PlayerState : BaseState<PlayerControl, PlayerState>
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

#if UNITY_EDITOR

        private void OnValidate()
        {
            animator = GetComponent<Animator>();
            rigidbody = GetComponent<Rigidbody2D>();
        }

        [TabGroup("Animator")]
        [Button]
        private void ChangePlayerGender(Gender gender)
        {
            animator.runtimeAnimatorController = gender switch
            {
                Gender.Male => maleController,
                Gender.Female => femaleController,
                _ => maleController
            };
        }

        [Button]
        private void Save()
        {
            ES3AutoSaveMgr.Current.Save();
        }

        [Button]
        private void Load()
        {
            ES3AutoSaveMgr.Current.Load();
        }
#endif
    }
}