using GonDraz.Events;

namespace GonDraz.StateMachine
{
    public class BaseState<TMachine, TState> : IState
    {
        private readonly Event _enter = new();
        private readonly Event _exit = new();
        private readonly Event _fixedUpdate = new();
        private readonly Event _lateUpdate = new();
        private readonly Event _update = new();

        private TMachine _host;
        public TState PreviousState;

        public virtual void OnEnter()
        {
            _enter.Invoke();
        }

        public virtual void OnUpdate()
        {
            _update.Invoke();
        }

        public virtual void OnLateUpdate()
        {
            _lateUpdate.Invoke();
        }

        public virtual void OnFixedUpdate()
        {
            _fixedUpdate.Invoke();
        }

        public virtual void OnExit()
        {
            _exit.Invoke();
        }

        public TMachine GetHost()
        {
            return _host;
        }

        public virtual void Instance(TMachine host)
        {
            _host = host;
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