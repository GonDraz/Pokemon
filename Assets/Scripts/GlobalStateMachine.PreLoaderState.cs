using GonDraz.UI.Route;
using UI.Screens;
using EventManager = GonDraz.Managers.EventManager;

public partial class GlobalStateMachine
{
    private class PreLoaderState : BaseGlobalState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            EventManager.ApplicationLoadFinished += ApplicationLoadFinished;
            RouteManager.Go(typeof(PreLoaderScreen));
        }


        public override void OnExit()
        {
            base.OnExit();
            EventManager.ApplicationLoadFinished -= ApplicationLoadFinished;
        }

        private void ApplicationLoadFinished()
        {
            Host.ChangeState<MenuState>();
        }
    }
}