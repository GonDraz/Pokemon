using System;
using System.Collections;
using System.Collections.Generic;
using GonDraz.Managers;
using Map;
using UnityEngine;

namespace Managers
{
    public class ApplicationManager : MonoBehaviour
    {
        private static bool _isApplicationLoadFinished;

        private void Awake()
        {
            foreach (var manager in ComponentInits())
            {
                Debug.Log("Init : <color=blue>" + manager.Name + "</color>");
                var managerObject = new GameObject { name = manager.Name };
                managerObject.AddComponent(manager);
            }
        }

        public void Start()
        {
            StartCoroutine(ApplicationLoadFinished());
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (_isApplicationLoadFinished)
                EventManager.ApplicationPause.Invoke(!hasFocus);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (_isApplicationLoadFinished)
                EventManager.ApplicationPause.Invoke(pauseStatus);
        }

        private static List<Type> ComponentInits()
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
            var app = new GameObject { name = nameof(ApplicationManager) };
            app.AddComponent<ApplicationManager>();
        }

        private IEnumerator ApplicationLoadFinished()
        {
            yield return new WaitForEndOfFrame();
            EventManager.ApplicationLoadFinished.Invoke();
            _isApplicationLoadFinished = true;
            Debug.Log("Application Load Finished");
        }
    }
}