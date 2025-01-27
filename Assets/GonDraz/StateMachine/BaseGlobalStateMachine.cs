using System;

namespace GonDraz.StateMachine
{
    public abstract class BaseGlobalStateMachine<T> : BaseStateMachine<T>
        where T : BaseGlobalStateMachine<T>.BaseGlobalState
    {
        protected override bool IsDontDestroyOnLoad()
        {
            return false;
        }

        public new static void ChangeState(Type type, bool canBack = true)
        {
            Instance?.ChangeState(type, canBack);
        }

        public new static void ChangeState<T1>(bool canBack = true) where T1 : IState
        {
            Instance?.ChangeState<T1>(canBack);
        }

        public new static void BackToPreviousState()
        {
            Instance?.BackToPreviousState();
        }

        public abstract class BaseGlobalState : BaseState
        {
        }
    }
}