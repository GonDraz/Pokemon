using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GonDraz.StateMachine
{
    public abstract class BaseStateMachine<TMachine, TState> : Base where TMachine : BaseStateMachine<TMachine, TState>
        where TState : BaseState<TMachine, TState>
    {
        [SerializeField, ReadOnly] private string currentStateName;
        [SerializeField, ReadOnly] private string previousStateName;
        private readonly StateMachine<TMachine, TState> _stateMachine = new();

        private Dictionary<Type, TState> _states;

        private Dictionary<Type, TState> States
        {
            get
            {
                if (_states != null) return _states;
                _states = new Dictionary<Type, TState>();

                var types = typeof(TMachine).GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Public)
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
            if (States.TryGetValue(type, out var state))
            {
                _stateMachine.ChangeState(state, canBack);
                currentStateName = _stateMachine.CurrentState?.GetType().Name;
                previousStateName = _stateMachine.CurrentState?.PreviousState?.GetType().Name;
            }
        }

        public void ChangeState<T1>(bool canBack = true) where T1 : IState
        {
            ChangeState(typeof(T1), canBack);
        }

        public bool CanBack()
        {
            return _stateMachine.CanBack();
        }

        public void BackToPreviousState()
        {
            _stateMachine.BackToPreviousState();
        } // ReSharper disable Unity.PerformanceAnalysis
        public void RegisterEvent<TS>(EventState eventState, Action action) where TS : TState
        {
            if (States.TryGetValue(typeof(TS), out var state))
                switch (eventState)
                {
                    case EventState.Enter:
                        state.Enter += action;
                        break;
                    case EventState.Exit:
                        state.Exit += action;
                        break;
                    case EventState.FixedUpdate:
                        state.FixedUpdate += action;
                        break;
                    case EventState.LateUpdate:
                        state.LateUpdate += action;
                        break;
                    case EventState.Update:
                        state.Update += action;
                        break;
                }
            else
                Debug.LogError("Error RegisterEvent: " + eventState + " Can't register Action: " + action);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void UnregisterEvent<TS>(EventState eventState, Action action) where TS : TState
        {
            if (States.TryGetValue(typeof(TS), out var state))
                switch (eventState)
                {
                    case EventState.Enter:
                        state.Enter -= action;
                        break;
                    case EventState.Exit:
                        state.Exit -= action;
                        break;
                    case EventState.FixedUpdate:
                        state.FixedUpdate -= action;
                        break;
                    case EventState.LateUpdate:
                        state.LateUpdate -= action;
                        break;
                    case EventState.Update:
                        state.Update -= action;
                        break;
                }
            else
                Debug.LogError("Error UnregisterEvent: " + eventState + " Can't unregister Action: " + action);
        }
    }
}