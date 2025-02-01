using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public partial class PlayerControl
    {
        private static readonly int MoveX = Animator.StringToHash("MoveX");
        private static readonly int MoveY = Animator.StringToHash("MoveY");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        [SerializeField] [Range(0.25f, 5f)] internal float moveSpeed = 3f;

        private class Walk : PlayerState
        {
            private bool _isMoving;
            private Vector2 _movement;

            public override void OnUpdate()
            {
                base.OnUpdate();
                if (_isMoving) return;
                if (_movement != Vector2.zero)
                {
                    var target = Host.transform.position;
                    target.x += Mathf.CeilToInt(_movement.x);
                    target.y += Mathf.CeilToInt(_movement.y);
                    Host.animator.SetFloat(MoveX, _movement.x);
                    Host.animator.SetFloat(MoveY, _movement.y);
                    Host.animator.SetBool(IsMoving, true);

                    if (IsWalkable(target))
                        Host.StartCoroutine(MoveTo(target));
                    else
                        Host.animator.SetBool(IsMoving, false);
                }
                else
                {
                    Host.animator.SetBool(IsMoving, false);
                    Host.ChangeState<Idle>();
                }
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

            private IEnumerator MoveTo(Vector3 target)
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