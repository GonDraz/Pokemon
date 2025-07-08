using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace GonDraz.Scene
{
    public static class SceneLoaderService
    {
        private static readonly Dictionary<string, SceneStatus> sceneStates = new();
        private static readonly Queue<Func<UniTask>> sceneTaskQueue = new();
        private static readonly SemaphoreSlim processLock = new(1, 1);

        public static async UniTask LoadSceneAsync(string sceneName)
        {
            if (!sceneStates.TryGetValue(sceneName, out var status) || status == SceneStatus.Unloaded)
            {
                sceneStates[sceneName] = SceneStatus.Loading;
                sceneTaskQueue.Enqueue(() => Load(sceneName));
                await ProcessQueue();
            }
        }

        public static async UniTask UnloadSceneAsync(string sceneName)
        {
            if (sceneStates.TryGetValue(sceneName, out var status) && status == SceneStatus.Loaded)
            {
                sceneStates[sceneName] = SceneStatus.Unloading;
                sceneTaskQueue.Enqueue(() => Unload(sceneName));
                await ProcessQueue();
            }
        }

        private static async UniTask Load(string sceneName)
        {
            if (!SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                var op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                if (op != null) await op.ToUniTask();
            }

            sceneStates[sceneName] = SceneStatus.Loaded;
        }

        private static async UniTask Unload(string sceneName)
        {
            if (SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                var op = SceneManager.UnloadSceneAsync(sceneName);
                if (op != null) await op.ToUniTask();
            }

            sceneStates[sceneName] = SceneStatus.Unloaded;
        }

        private static async UniTask ProcessQueue()
        {
            await processLock.WaitAsync();

            try
            {
                while (sceneTaskQueue.Count > 0)
                {
                    var taskFunc = sceneTaskQueue.Dequeue();
                    await taskFunc();
                    await UniTask.Yield();
                }
            }
            finally
            {
                processLock.Release();
            }
        }

        public static bool IsSceneLoaded(string sceneName)
        {
            return sceneStates.TryGetValue(sceneName, out var state) && state == SceneStatus.Loaded;
        }

        private enum SceneStatus
        {
            Unloaded,
            Loading,
            Loaded,
            Unloading
        }
    }
}