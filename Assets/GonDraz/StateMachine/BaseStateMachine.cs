using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GonDraz.StateMachine
{
    public abstract class BaseStateMachine<TMachine, TState> : Base where TMachine : BaseStateMachine<TMachine, TState>
        where TState : BaseState<TMachine, TState>
    {
        private readonly StateMachine<TMachine, TState> _stateMachine = new();

        private Dictionary<Type, TState> _states;

        public Dictionary<Type, TState> States
        {
            get
            {
                if (_states != null) return _states;
                _states = new Dictionary<Type, TState>();

                var types = typeof(TMachine).GetNestedTypes(BindingFlags.NonPublic)
                    .Where(t => t.IsSubclassOf(typeof(TState)))
                    .ToArray();
                foreach (var type in types)
                {
                    var instance = (TState)Activator.CreateInstance(type);
                    instance.Instance((TMachine)this);
                    _states.Add(type, instance);
                }

                return _states;
            }
        }

        protected virtual void Awake()
        {
            ChangeState(InitialState(), false);
        }

        private void Update()
        {
            _stateMachine.OnUpdate();
        }

        private void FixedUpdate()
        {
            _stateMachine.OnFixedUpdate();
        }

        private void LateUpdate()
        {
            _stateMachine.OnLateUpdate();
        }

        public TState GetCurrentState()
        {
            return _stateMachine.CurrentState;
        }


        public bool TryGetState<TType>(out TState state) where TType : TState
        {
            if (States.TryGetValue(typeof(TType), out var value))
            {
                state = value;
                return true;
            }

            state = null;
            return false;
        }

        public abstract Type InitialState();

        public void ChangeState(Type type, bool canBack = true)
        {
            if (States.TryGetValue(type, out var state)) _stateMachine.ChangeState(state, canBack);
        }

        public void ChangeState<T1>(bool canBack = true) where T1 : IState
        {
            ChangeState(typeof(T1), canBack);
        }

        public void BackToPreviousState()
        {
            _stateMachine.BackToPreviousState();
        }
    }
}