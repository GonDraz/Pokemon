using GonDraz.Events;

namespace GonDraz.StateMachine
{
    public class BaseState<TMachine, TState> : IState
    {
        public Event Enter = new();
        public Event Exit = new();
        public Event FixedUpdate = new();
        public Event LateUpdate = new();
        public TState PreviousState;
        public Event Update = new();

        public TMachine Host { get; private set; }

        public virtual void OnEnter()
        {
            Enter.Invoke();
        }

        public virtual void OnUpdate()
        {
            Update.Invoke();
        }

        public virtual void OnLateUpdate()
        {
            LateUpdate.Invoke();
        }

        public virtual void OnFixedUpdate()
        {
            FixedUpdate.Invoke();
        }

        public virtual void OnExit()
        {
            Exit.Invoke();
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
    }
}