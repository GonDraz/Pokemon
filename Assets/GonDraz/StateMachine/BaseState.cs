using System;

namespace GonDraz.StateMachine
{
    public class BaseState<TMachine, TState> : IState
    {
        public TMachine Host;
        public TState PreviousState;

        public virtual void OnEnter()
        {
            Enter?.Invoke();
        }

        public virtual void OnUpdate()
        {
            Update?.Invoke();
        }

        public virtual void OnLateUpdate()
        {
            LateUpdate?.Invoke();
        }

        public virtual void OnFixedUpdate()
        {
            FixedUpdate?.Invoke();
        }

        public virtual void OnExit()
        {
            Exit?.Invoke();
        }

        public event Action Enter;
        public event Action Exit;
        public event Action FixedUpdate;
        public event Action LateUpdate;
        public event Action Update;

        public virtual void OnPause()
        {
        }

        public virtual void OnResume()
        {
        }

        public virtual void Instance(TMachine host)
        {
            Host = host;
        }

        public virtual void OnEnter(TState previousState)
        {
            SetPreviousState(previousState);
            OnEnter();
        }

        public void SetPreviousState(TState previousState)
        {
            PreviousState = previousState;
        }

        public override string ToString()
        {
            return $"{GetName()} (Host: {Host?.GetType().Name}, PreviousState: {PreviousState})";
        }

        public string GetName()
        {
            return GetType().Name;
        }
    }
}