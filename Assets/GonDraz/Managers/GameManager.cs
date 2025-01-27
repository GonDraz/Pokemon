using System;
using System.Collections.Generic;
using UnityEngine;

namespace GonDraz.Managers
{
    public class GameManager : MonoBehaviour
    {
        private void Awake()
        {
        }

        public void Start()
        {
            ApplicationLoadFinished();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            EventManager.ApplicationPause.Invoke(!hasFocus);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            EventManager.ApplicationPause.Invoke(pauseStatus);
        }

        protected static List<Type> ComponentInits()
        {
            return new List<Type>
            {
                // typeof(GlobalStateMachine)
            };
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RuntimeInit()
        {
            var app = new GameObject { name = nameof(GameManager) };
            app.AddComponent<GameManager>();
            foreach (var manager in ComponentInits())
            {
                Debug.Log("Init : <color=blue>" + manager.Name + "</color>");
                app.AddComponent(manager);
            }
        }

        private void ApplicationLoadFinished()
        {
            EventManager.ApplicationLoadFinished.Invoke();
        }
    }
}