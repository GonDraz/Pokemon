using System;
using System.Collections.Generic;

namespace GonDraz.StateMachine
{
    public abstract class BaseStateMachine<T> : BaseSingleton<BaseStateMachine<T>> where T : BaseState
    {
        protected internal readonly StateMachine StateMachine = new();

        protected override void Awake()
        {
            base.Awake();
            ChangeState(InitialState(), false);
        }

        private void Update()
        {
            StateMachine.OnUpdate();
        }

        private void FixedUpdate()
        {
            StateMachine.OnFixedUpdate();
        }

        private void LateUpdate()
        {
            StateMachine.OnLateUpdate();
        }

        public abstract Dictionary<Type, T> States();


        public abstract Type InitialState();

        public void ChangeState(Type type, bool canBack = true)
        {
            if (States().TryGetValue(type, out var state)) StateMachine.ChangeState(state, canBack);
        }

        public void ChangeState<T1>(bool canBack = true) where T1 : IState
        {
            ChangeState(typeof(T1), canBack);
        }

        public void BackToPreviousState()
        {
            StateMachine.BackToPreviousState();
        }
    }
}