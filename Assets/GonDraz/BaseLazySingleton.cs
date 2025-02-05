using System;
using UnityEditor;
using UnityEngine;

namespace GonDraz
{
    public abstract class BaseLazySingleton<T> : Base where T : BaseLazySingleton<T>
    {
        private static readonly Lazy<T> LazyInstance = new(CreateSingleton);
        public static T Inst => LazyInstance.Value;

        private static T CreateSingleton()
        {
            var obj = new GameObject("(Singleton)" + typeof(T).Name);
            DontDestroyOnLoad(obj);
            return obj.AddComponent<T>();
        }
#if UNITY_EDITOR
        protected virtual void Reset()
        {
            EditorUtility.DisplayDialog("LazySingletons should not be manually instantiated",
                "Lazyloaded Singletons will load when first accessed. Instances that are part of scenes or prefabs do nothing.",
                "Ok");
            DestroyImmediate(this);
        }
#endif
    }
}