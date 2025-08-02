using System;
using System.Collections.Generic;
using System.Linq;
using GonDraz.StateMachine;
using UnityEngine;
using EventManager = GonDraz.Managers.EventManager;

namespace Map
{
    public partial class MapControl : BaseStateMachine<MapControl, MapControl.MapState>
    {
        private MapConfig mapConfig;

        private MapConfig MapConfig
        {
            get
            {
                if (mapConfig == null) mapConfig = Resources.Load<MapConfig>("Map/MapConfig");
                return mapConfig;
            }
        }

        protected override Type InitialState()
        {
            return typeof(PalletTown);
        }

        protected override bool SubscribeUsingOnEnable()
        {
            return true;
        }

        public override void Subscribe()
        {
            base.Subscribe();
            EventManager.MapTriggerEnter += OnMapTriggerEnter;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            EventManager.MapTriggerEnter -= OnMapTriggerEnter;
        }

        private void OnMapTriggerEnter(string screen)
        {
            ChangeState(screen);
        }

        public abstract class MapState : BaseState<MapControl, MapState>
        {
            public override void OnEnter()
            {
                base.OnEnter();

                var requiredScenes = new List<Type> { GetType() };
                requiredScenes.AddRange(ScenesToLoad());

                foreach (var mapScene in from mapScene in Host.MapConfig.GetMapScenes()
                         let shouldBeLoaded = requiredScenes.Any(s => s.Name == mapScene.SceneName)
                         where !shouldBeLoaded
                         select mapScene)
                {
                    _ = mapScene.UnloadSceneAsync();
                }

                foreach (var mapScene in from mapScene in Host.MapConfig.GetMapScenes()
                         let shouldBeLoaded = requiredScenes.Any(s => s.Name == mapScene.SceneName)
                         where shouldBeLoaded
                         select mapScene)
                {
                    _ = mapScene.LoadSceneAsync();
                }
            }

            protected abstract List<Type> ScenesToLoad();
        }
    }
}