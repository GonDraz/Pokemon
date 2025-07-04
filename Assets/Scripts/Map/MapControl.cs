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

                foreach (SceneField mapScene in Host.mapScenes)
                {
                    foreach (var scene in scenes)
                    {
                        if (scene.Name == mapScene.SceneName)
                        {
                            mapScene.LoadScene();
                        }
                    }
                }
                
                foreach (SceneField mapScene in Host.mapScenes)
                {
                    bool sceneLoad = false;
                    foreach (var scene in scenes)
                    {
                        if (scene.Name == mapScene.SceneName)
                        {
                            sceneLoad = true;
                        }
                    }
                    if(!sceneLoad) 
                    {
                        mapScene.UnloadScene();
                    }
                }
            }

            protected abstract List<Type> ScenesToLoad();
        }
    }
}