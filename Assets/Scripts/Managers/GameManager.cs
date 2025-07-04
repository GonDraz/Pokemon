using System;
using System.Collections.Generic;
using Map;
using UnityEngine;

namespace Managers
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
            GonDraz.Managers.EventManager.ApplicationPause.Invoke(!hasFocus);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            GonDraz.Managers.EventManager.ApplicationPause.Invoke(pauseStatus);
        }

        protected static List<Type> ComponentInits()
        {
            return new List<Type>
            {
                typeof(GlobalStateMachine),
                typeof(MapControl)
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
                var managerObject = new GameObject { name = manager.Name };
                managerObject.AddComponent(manager);
            }
        }

        private void ApplicationLoadFinished()
        {
            GonDraz.Managers.EventManager.ApplicationLoadFinished.Invoke();
        }
    }
}