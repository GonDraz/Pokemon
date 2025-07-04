using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace GonDraz.Scene
{
    [Serializable]
    public class SceneField
    {
        [SerializeField] private Object sceneAsset;

        [SerializeField] private string sceneName = "";

        public string SceneName => sceneName;

        public static implicit operator string(SceneField sceneField)
        {
            return sceneField.SceneName;
        }

        public void LoadScene()
        {
            var isSceneLoaded = false;
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var loadedScene = SceneManager.GetSceneAt(i);
                if (loadedScene.name == SceneName)
                {
                    isSceneLoaded = true;
                }
            }

            if (!isSceneLoaded)
            {
                SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
            }
        }

        public void UnloadScene()
        {
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var loadedScene = SceneManager.GetSceneAt(i);
                if (loadedScene.name == SceneName)
                {
                    SceneManager.UnloadSceneAsync(SceneName);
                }
            }
        }
    }
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(SceneField))]
    public class SceneFieldPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, GUIContent.none, property);
            var sceneAsset = property.FindPropertyRelative("sceneAsset");
            var sceneName = property.FindPropertyRelative("sceneName");
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            if (sceneAsset != null)
            {
                sceneAsset.objectReferenceValue = EditorGUI.ObjectField(position, sceneAsset.objectReferenceValue,
                    typeof(SceneAsset), false);

                if (sceneAsset.objectReferenceValue)
                    sceneName.stringValue = (sceneAsset.objectReferenceValue as SceneAsset)?.name;
            }

            EditorGUI.EndProperty();
        }
    }
#endif
}