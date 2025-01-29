using System;
using System.Collections;
using GonDraz.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerControl : BaseStateMachine<PlayerControl, PlayerControl.PlayerState>
    {
        private static readonly int MoveX = Animator.StringToHash("MoveX");
        private static readonly int MoveY = Animator.StringToHash("MoveY");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        [SerializeField] [Range(0.25f, 5f)] internal float moveSpeed = 3f;
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
            return typeof(Idle);
        }

        public abstract class PlayerState : BaseState<PlayerControl, PlayerState>
        {
            internal virtual void Move(InputAction.CallbackContext obj)
            {
            }
        }

        private class Idle : PlayerState
        {
            internal override void Move(InputAction.CallbackContext context)
            {
                if (context.ReadValue<Vector2>() != Vector2.zero) Host.ChangeState<Walk>();
            }
        }

        private class Walk : PlayerState
        {
            private bool _isMoving;
            private Vector2 _movement;

            public override void OnEnter()
            {
                base.OnEnter();
                Host.animator.SetBool(IsMoving, true);
            }

            public override void OnExit()
            {
                base.OnExit();
                Host.animator.SetBool(IsMoving, false);
            }

            public override void OnUpdate()
            {
                base.OnUpdate();
                if (!_isMoving)
                    if (_movement != Vector2.zero)
                    {
                        var target = Host.transform.position;
                        target.x += _movement.x;
                        target.y += _movement.y;
                        Host.animator.SetFloat(MoveX, _movement.x);
                        Host.animator.SetFloat(MoveY, _movement.y);

                        Host.StartCoroutine(Moving(target));
                    }
                    else
                    {
                        Host.ChangeState<Idle>();
                    }
            }

            internal override void Move(InputAction.CallbackContext context)
            {
                _movement = context.ReadValue<Vector2>();
            }

            private IEnumerator Moving(Vector3 target)
            {
                _isMoving = true;

                var transformPosition = Host.transform.position;
                while ((target - transformPosition).sqrMagnitude > Mathf.Epsilon)
                {
                    transformPosition =
                        Vector3.MoveTowards(transformPosition, target, Host.moveSpeed * Time.deltaTime);
                    Host.transform.position = transformPosition;
                    yield return null;
                }

                Host.transform.position = target;
                _isMoving = false;
            }
        }
    }
}