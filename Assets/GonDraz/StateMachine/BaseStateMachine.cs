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
        [SerializeField] [ReadOnly] private string currentStateName;
        [SerializeField] [ReadOnly] private string previousStateName;
        private TState _currentState;

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
            if (GetCurrentState() == null) ChangeState(InitialState(), false);
        }

        private void Update()
        {
            _currentState.OnUpdate();
        }

        private void FixedUpdate()
        {
            _currentState.OnFixedUpdate();
        }

        private void LateUpdate()
        {
            _currentState.OnLateUpdate();
        }

        public TState GetCurrentState()
        {
            return _currentState;
        }

        public bool TryGetState<TType>(out TType state) where TType : TState
        {
            if (States.TryGetValue(typeof(TType), out var value))
            {
                state = (TType)value;
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
                ChangeState(state, canBack);
                currentStateName = _currentState?.GetType().Name;
                previousStateName = _currentState?.PreviousState?.GetType().Name;
            }
        }


        public void ChangeState<T1>(bool canBack = true) where T1 : IState
        {
            ChangeState(typeof(T1), canBack);
        }

        public void ChangeState(TState state, bool canBack = true)
        {
            if (_currentState == null)
            {
                _currentState = state;
                // Debug.Log("Change state to <color=red>" + CurrentState.GetType().Name + "</color>");
                _currentState.OnEnter();
                return;
            }

            if (state.GetType().FullName == _currentState.GetType().FullName)
                return;

            _currentState.OnExit();

            // Debug.Log("Change state from <color=red>" + CurrentState.GetType().Name + "</color> to <color=green>" +
            //           state.GetType().Name + "</color>");

            var previousState = _currentState;
            _currentState = state;
            if (!canBack) previousState = null;
            _currentState.OnEnter(previousState);
        }

        public bool CanBack()
        {
            if (_currentState.PreviousState == null) return false;

            return _currentState.PreviousState.GetType().FullName != _currentState.GetType().FullName;
        }

        public void BackToPreviousState()
        {
            if (!CanBack()) return;

            _currentState.OnExit();

            // Debug.Log("Back state from <color=red>" + CurrentState.GetType().Name + "</color> to <color=green>" +
            //           CurrentState.PreviousState.GetType().Name + "</color>");

            _currentState = _currentState.PreviousState;
            _currentState.OnEnter();
        }

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