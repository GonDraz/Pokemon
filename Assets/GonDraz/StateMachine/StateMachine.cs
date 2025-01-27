using UnityEngine;

namespace GonDraz.StateMachine
{
    public class StateMachine
    {
        private BaseState _currentState;

        public void OnUpdate()
        {
            _currentState?.OnUpdate();
        }

        public void OnLateUpdate()
        {
            _currentState?.OnLateUpdate();
        }

        public void OnFixedUpdate()
        {
            _currentState?.OnFixedUpdate();
        }

        public void ChangeState(BaseState state, bool canBack = true)
        {
            if (_currentState == null)
            {
                _currentState = state;
                _currentState.OnEnter();
                Debug.Log("Change state to <color=red>" + _currentState.GetType().Name + "</color>");
                return;
            }

            if (state.GetType().FullName == _currentState.GetType().FullName)
                return;

            _currentState.OnExit();

            Debug.Log("Change state from <color=red>" + _currentState.GetType().Name + "</color> to <color=green>" +
                      state.GetType().Name + "</color>");

            var previousState = _currentState;
            _currentState = state;
            if (!canBack) previousState = null;
            _currentState.OnEnter(previousState);
        }

        public void BackToPreviousState()
        {
            if (_currentState.PreviousState == null)
            {
                Debug.Log("Previous state is null <color=red>" + _currentState.GetType().Name + "</color>");
                return;
            }

            if (_currentState.PreviousState.GetType().FullName == _currentState.GetType().FullName)
                return;

            _currentState.OnExit();

            Debug.Log("Back state from <color=red>" + _currentState.GetType().Name + "</color> to <color=green>" +
                      _currentState.PreviousState.GetType().Name + "</color>");

            _currentState = _currentState.PreviousState;
            _currentState.OnEnter();
        }
    }
}