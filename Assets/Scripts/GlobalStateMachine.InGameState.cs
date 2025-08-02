using GonDraz.Managers;
using GonDraz.UI.Route;
using UI.Screens;

public partial class GlobalStateMachine
{
    public class InGameState : BaseGlobalState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            EventManager.GamePause += OnGamePause;
            EventManager.ApplicationPause += OnApplicationPause;
            RouteManager.Go(typeof(InGameScreen));
        }

        public override void OnExit()
        {
            base.OnExit();
            EventManager.GamePause -= OnGamePause;
            EventManager.ApplicationPause -= OnApplicationPause;
        }


        private void OnApplicationPause(bool obj)
        {
            Host.ChangeState<InGamePauseState>();
        }

        private void OnGamePause()
        {
            Host.ChangeState<InGamePauseState>();
        }
    }
}