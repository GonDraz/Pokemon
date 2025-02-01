namespace GonDraz.StateMachine
{
    public class StateMachine<TMachine, TState> where TState : BaseState<TMachine, TState>
    {
        public TState CurrentState { get; private set; }

        public void OnUpdate()
        {
            CurrentState?.OnUpdate();
        }

        public void OnLateUpdate()
        {
            CurrentState?.OnLateUpdate();
        }

        public void OnFixedUpdate()
        {
            CurrentState?.OnFixedUpdate();
        }

        public void ChangeState(TState state, bool canBack = true)
        {
            if (CurrentState == null)
            {
                CurrentState = state;
                // Debug.Log("Change state to <color=red>" + CurrentState.GetType().Name + "</color>");
                CurrentState.OnEnter();
                return;
            }

            if (state.GetType().FullName == CurrentState.GetType().FullName)
                return;

            CurrentState.OnExit();

            // Debug.Log("Change state from <color=red>" + CurrentState.GetType().Name + "</color> to <color=green>" +
            //           state.GetType().Name + "</color>");

            var previousState = CurrentState;
            CurrentState = state;
            if (!canBack) previousState = null;
            CurrentState.OnEnter(previousState);
        }

        public bool CanBack()
        {
            if (CurrentState.PreviousState == null) return false;

            return CurrentState.PreviousState.GetType().FullName != CurrentState.GetType().FullName;
        }

        public void BackToPreviousState()
        {
            if (!CanBack()) return;

            CurrentState.OnExit();

            // Debug.Log("Back state from <color=red>" + CurrentState.GetType().Name + "</color> to <color=green>" +
            //           CurrentState.PreviousState.GetType().Name + "</color>");

            CurrentState = CurrentState.PreviousState;
            CurrentState.OnEnter();
        }
    }
}