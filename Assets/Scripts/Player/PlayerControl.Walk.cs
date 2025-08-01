﻿using Cysharp.Threading.Tasks;
using GonDraz.Events;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public partial class PlayerControl
    {
        [TabGroup("Walk")] [SerializeField] [Range(0.25f, 5f)]
        private float walkSpeed = 3f;


        public class Walk : PlayerState
        {
            private const float TargetYOffset = 0.5f;

            internal Vector2 Movement;

            private bool IsWalkable(Vector3 target)
            {
                target.y -= TargetYOffset;
                return !Physics2D.OverlapBox(target, Vector2.one * 0.95f, 0f, Host.solidObjectLayerMask);
            }

            public override void OnUpdate()
            {
                base.OnUpdate();
                if (Host._isMoving) return;
                if (Movement != Vector2.zero)
                {
                    var target = Host.transform.position;
                    target.x += Mathf.CeilToInt(Movement.x);
                    target.y += Mathf.CeilToInt(Movement.y);

                    target.x = Mathf.Floor(target.x) + 0.5f;
                    target.y = Mathf.Round(target.y);

                    Host.animator.SetFloat(MoveX, Movement.x);
                    Host.animator.SetFloat(MoveY, Movement.y);
                    Host.animator.SetBool(IsMoving, true);

                    if (IsWalkable(target))
                        MoveTo(target, new GEvent(CheckForEncounters)).Forget();
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

                if (Mathf.Approximately(Movement.x, input.x))
                    if (input.y != 0)
                        input.x = 0;

                if (Mathf.Approximately(Movement.y, input.y))
                    if (input.x != 0)
                        input.y = 0;

                Movement = input;
            }


            internal override void Sprint(InputAction.CallbackContext context)
            {
                if (!context.control.IsPressed()) return;
                if (!Host.TryGetState<Run>(out var state)) return;
                state.Movement = Movement;
                Host.ChangeState<Run>();
            }

            private bool IsWater(Vector3 target)
            {
                target.y -= TargetYOffset;
                return Physics2D.OverlapBox(target, Vector2.one * 0.95f, 0f, Host.waterLayerMask);
            }

            private bool IsGrass(Vector3 target)
            {
                target.y -= TargetYOffset;
                return Physics2D.OverlapBox(target, Vector2.one * 0.95f, 0f, Host.grassLayerMask);
            }

            private async UniTaskVoid MoveTo(Vector3 target, GEvent callback = null)
            {
                Host._isMoving = true;
                var speed = MoveSpeed();
                var startPos = Host.transform.position;

                target.x = Mathf.Floor(target.x) + 0.5f;
                target.y = Mathf.Round(target.y);

                var time = 0f;
                var duration = 1f / speed;

                while (time < duration)
                {
                    time += Time.fixedDeltaTime;
                    var t = Mathf.Clamp01(time / duration);
                    var newPos = Vector3.Lerp(startPos, target, t);
                    Host.rigidbody.MovePosition(newPos);
                    await UniTask.WaitForFixedUpdate();
                }

                Host.rigidbody.MovePosition(target);
                Host._isMoving = false;
                callback?.Invoke();
            }

            protected virtual float MoveSpeed()
            {
                return Host.walkSpeed;
            }
        }
    }
}