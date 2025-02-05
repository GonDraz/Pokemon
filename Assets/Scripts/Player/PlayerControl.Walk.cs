using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public partial class PlayerControl
    {
        [TabGroup("Walk")] [SerializeField] [Range(0.25f, 5f)]
        private float walkSpeed = 3f;

        private class Walk : PlayerState
        {
            internal Vector2 _movement;

            private bool IsWalkable(Vector3 target)
            {
                target.y -= 0.5f;
                return !Physics2D.OverlapCircle(target, 0.25f, Host.solidObjectLayerMask);
            }

            public override void OnUpdate()
            {
                base.OnUpdate();
                if (Host._isMoving) return;
                if (_movement != Vector2.zero)
                {
                    var target = Host.transform.position;
                    target.x += Mathf.CeilToInt(_movement.x);
                    target.y += Mathf.CeilToInt(_movement.y);
                    Host.animator.SetFloat(MoveX, _movement.x);
                    Host.animator.SetFloat(MoveY, _movement.y);
                    Host.animator.SetBool(IsMoving, true);

                    if (IsWalkable(target))
                        Host.StartCoroutine(MoveTo(target, CheckForEncounters));
                    else
                        Host.animator.SetBool(IsMoving, false);
                }
                else
                {
                    NotMovement();
                }
            }

            protected virtual void NotMovement()
            {
                Host.animator.SetBool(IsMoving, false);
                Host.ChangeState<Idle>();
            }

            private void CheckForEncounters()
            {
                if (IsGrass(Host.transform.position)) Debug.Log("đang trên cỏ");
            }

            internal override void Move(InputAction.CallbackContext context)
            {
                var input = context.ReadValue<Vector2>();

                if (input.x != 0) input.x = input.x > 0 ? 1 : -1;

                if (input.y != 0) input.y = input.y > 0 ? 1 : -1;

                if (Mathf.Approximately(_movement.x, input.x))
                    if (input.y != 0)
                        input.x = 0;

                if (Mathf.Approximately(_movement.y, input.y))
                    if (input.x != 0)
                        input.y = 0;

                _movement = input;
            }


            internal override void Sprint(InputAction.CallbackContext context)
            {
                if (context.control.IsPressed())
                    if (Host.TryGetState<Run>(out var state))
                    {
                        state._movement = _movement;
                        Host.ChangeState<Run>();
                    }
            }

            private bool IsWater(Vector3 target)
            {
                target.y -= 0.5f;
                return Physics2D.OverlapCircle(target, 0.25f, Host.waterLayerMask);
            }

            private bool IsGrass(Vector3 target)
            {
                target.y -= 0.5f;
                return Physics2D.OverlapCircle(target, 0.25f, Host.grassLayerMask);
            }

            protected IEnumerator MoveTo(Vector3 target, Action callback = null)
            {
                Host._isMoving = true;
                var speed = MoveSpeed();

                var transformPosition = Host.transform.position;
                while ((target - transformPosition).sqrMagnitude > Mathf.Epsilon)
                {
                    transformPosition =
                        Vector3.MoveTowards(transformPosition, target, speed * Time.deltaTime);
                    Host.transform.position = transformPosition;
                    yield return null;
                }

                Host.transform.position = target;
                Host._isMoving = false;
                callback?.Invoke();
            }

            public virtual float MoveSpeed()
            {
                return Host.walkSpeed;
            }
        }
    }
}