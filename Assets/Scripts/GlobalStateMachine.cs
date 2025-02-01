using System;
using GonDraz.Managers;
using GonDraz.StateMachine;
using GonDraz.UI.Route;
using UI.Screens;

public class GlobalStateMachine : BaseGlobalStateMachine<GlobalStateMachine>
{
    private void Start()
    {
    }

    public override Type InitialState()
    {
        return typeof(PreLoaderState);
    }

    private class PreLoaderState : BaseGlobalState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            EventManager.ApplicationLoadFinished += ApplicationLoadFinished;
            RouteManager.Go(typeof(PreLoaderScreen));
        }

        private void ApplicationLoadFinished()
        {
            Host.ChangeState<MenuState>();
        }

        public override void OnExit()
        {
            base.OnEnter();
            EventManager.ApplicationLoadFinished -= ApplicationLoadFinished;
        }
    }

    private class MenuState : BaseGlobalState
    {
        // ReSharper disable Unity.PerformanceAnalysis
        public override void OnEnter()
        {
            base.OnEnter();
            RouteManager.Go(typeof(MenuScreen));
        }
    }

    public class InGameState : BaseGlobalState
    {
        // ReSharper disable Unity.PerformanceAnalysis
        public override void OnEnter()
        {
            base.OnEnter();
            RouteManager.Go(typeof(InGameScreen));
        }
    }


    private class SettingState : BaseGlobalState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            RouteManager.Go(typeof(SettingsScreen));
        }
    }
}