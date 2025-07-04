using System;
using GonDraz.StateMachine;
using GonDraz.UI.Route;
using Managers;
using UI.Screens;
using EventManager = GonDraz.Managers.EventManager;

public class GlobalStateMachine : BaseGlobalStateMachine<GlobalStateMachine>
{
    private void Start()
    {
    }

    protected override Type InitialState()
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
        public override void OnEnter()
        {
            base.OnEnter();
            RouteManager.Go(typeof(MenuScreen));
        } // ReSharper disable Unity.PerformanceAnalysis
    }

    public class InGameState : BaseGlobalState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            RouteManager.Go(typeof(InGameScreen));
        } // ReSharper disable Unity.PerformanceAnalysis
    }

    public class InGamePauseState : BaseGlobalState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            RouteManager.Go(typeof(InGamePauseScreen));
        } // ReSharper disable Unity.PerformanceAnalysis
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