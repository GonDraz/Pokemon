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
                if (context.ReadValue<Vector2>() != Vector2.zero)
                {
                    GetHost().ChangeState<Walk>();
                }
            }
        }

        private class Walk : PlayerState
        {
            private Vector2 _movement;
            private bool _isMoving;

            public override void OnUpdate()
            {
                base.OnUpdate();
                if (!_isMoving)
                    if (_movement != Vector2.zero)
                    {
                        var target = GetHost().transform.position;
                        target.x += _movement.x;
                        target.y += _movement.y;
                        GetHost().animator.SetFloat(MoveX, _movement.x);
                        GetHost().animator.SetFloat(MoveY, _movement.y);

                        GetHost().StartCoroutine(Moving(target));
                    }

            }

            internal override void Move(InputAction.CallbackContext context)
            {
                _movement = context.ReadValue<Vector2>();
                if (_movement == Vector2.zero)
                {
                    GetHost().ChangeState<Idle>();
                }
            }

            private IEnumerator Moving(Vector3 target)
            {
                _isMoving = true;
                
                var transformPosition = GetHost().transform.position;
                while ((target - transformPosition).sqrMagnitude > Mathf.Epsilon)
                {
                    transformPosition =
                        Vector3.MoveTowards(transformPosition, target, GetHost().moveSpeed * Time.deltaTime);
                    GetHost().animator.SetBool(IsMoving, _isMoving);
                    GetHost().transform.position = transformPosition;
                    yield return null;
                }

                GetHost().transform.position = target;
                _isMoving = false;
                GetHost().animator.SetBool(IsMoving, _isMoving);
            }
        }

        public override Type InitialState()
        {
            return typeof(Idle);
        }
    }
}