using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GonDraz.Scene
{
    public class SceneLoadTrigger : MonoBehaviour
    {
        [TabGroup("Load")] [SerializeField] private SceneField[] scenesToLoad;
        [TabGroup("Unload")] [SerializeField] private SceneField[] scenesToUnLoad;

        private GameObject _player;

        private void Awake()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == _player)
            {
                UnloadScenes();
                LoadScenes();
            }
        }

        [Button]
        [TabGroup("Load")]
        private void LoadScenes()
        {
            foreach (var scene in scenesToLoad)
            {
                var isSceneLoaded = false;
                for (var i = 0; i < SceneManager.sceneCount; i++)
                {
                    var loadedScene = SceneManager.GetSceneAt(i);
                    if (loadedScene.name == scene.SceneName)
                    {
                        isSceneLoaded = true;
                        break;
                    }
                }

                if (!isSceneLoaded) SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            }
        }

        [Button]
        [TabGroup("Unload")]
        private void UnloadScenes()
        {
            foreach (var scene in scenesToUnLoad)
                for (var i = 0; i < SceneManager.sceneCount; i++)
                {
                    var loadedScene = SceneManager.GetSceneAt(i);
                    if (loadedScene.name == scene.SceneName) SceneManager.UnloadSceneAsync(scene);
                }
        }
    }
}