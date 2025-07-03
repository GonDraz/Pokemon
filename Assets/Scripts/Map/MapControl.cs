using System;
using System.Collections.Generic;
using System.Linq;
using GonDraz.Managers;
using GonDraz.Scene;
using GonDraz.StateMachine;
using UnityEngine;

namespace Map
{
    public partial class MapControl : BaseStateMachine<MapControl, MapControl.MapState>
    {
        [SerializeField] private List<SceneField> mapScenes;

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

                List<Type> scenes = new() { GetType() };
                scenes.AddRange(ScenesToLoad());

                foreach (var mapScene in from mapScene in Host.mapScenes
                         from scene in scenes
                         where scene.Name == mapScene.SceneName
                         select mapScene)
                    mapScene.LoadScene();

                foreach (var mapScene in from mapScene in Host.mapScenes
                         from scene in scenes
                         where scene.Name != mapScene.SceneName
                         select mapScene)
                    mapScene.UnloadScene();
            }

            protected abstract List<Type> ScenesToLoad();
        }
    }
}