using GonDraz.Managers;
using GonDraz.UI;

namespace UI.Screens
{
    public class InGameScreen : Presentation
    {
        public override void Subscribe()
        {
            base.Subscribe();
            EventManager.GamePause += OnGamePause;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            EventManager.GamePause -= OnGamePause;
        }

        private void OnGamePause()
        {
            GlobalStateMachine.Instance.ChangeState<GlobalStateMachine.InGamePauseState>();
        }
    }
}