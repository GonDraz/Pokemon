using GonDraz.UI.Route;
using UI.Screens;

public partial class GlobalStateMachine
{
    private class MenuState : BaseGlobalState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            RouteManager.Go(typeof(MenuScreen));
        }
    }
}