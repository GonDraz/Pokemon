using GonDraz.Events;

namespace GonDraz.StateMachine
{
    public class BaseState : IState
    {
        private readonly Event _enter = new();
        private readonly Event _exit = new();
        private readonly Event _fixedUpdate = new();
        private readonly Event _lateUpdate = new();
        private readonly Event _update = new();
        public BaseState PreviousState;

        public virtual void OnEnter()
        {
            _enter.Invoke();
        }

        public virtual void OnEnter(BaseState previousState)
        {
            SetPreviousState(previousState);
            OnEnter();
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

        public void SetPreviousState(BaseState previousState)
        {
            PreviousState = previousState;
        }
    }
}