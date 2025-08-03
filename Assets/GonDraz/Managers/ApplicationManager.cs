using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GonDraz.Events;
using GonDraz.Interfaces;
using Map;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GonDraz.Managers
{
    public class ApplicationManager : MonoBehaviour
    {
        private static bool _isApplicationLoadFinished;
        private static readonly List<Component> InitComponents = new List<Component>();


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


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void LoadScene()
        {
            SceneManager.LoadScene(0);
        }


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void RuntimeInit()
        {
            var app = new GameObject { name = nameof(ApplicationManager) };
            app.AddComponent<ApplicationManager>();


            foreach (var manager in ComponentInits())
            {
                Debug.Log("Init : <color=blue>" + manager.Name + "</color>");
                var managerObject = new GameObject { name = manager.Name };
                var component = managerObject.AddComponent(manager) as Component;
                InitComponents.Add(component);
            }

            ApplicationLoadFinishedAsync().Forget();
        }

        private static async UniTaskVoid ApplicationLoadFinishedAsync()
        {
            var tasks = InitComponents.Select((comp, idx) =>
            {
                if (comp is IAsyncInitProgress asyncInit)
                {
                    return asyncInit.InitAsync();
                }

                return UniTask.CompletedTask;
            }).ToArray();
            if (tasks.Length > 0)
                await UniTask.WhenAll(tasks);
            await UniTask.Yield();
            EventManager.ApplicationLoadFinished.Invoke();
            _isApplicationLoadFinished = true;
            Debug.Log("Application Load Finished");
        }
    }
}