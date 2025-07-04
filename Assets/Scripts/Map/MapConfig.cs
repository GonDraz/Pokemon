using System.Collections.Generic;
using GonDraz.Scene;
using UnityEngine;

namespace Map
{
    [CreateAssetMenu(fileName = "map-config", menuName = "Map Config", order = 0)]
    public class MapConfig : ScriptableObject
    {
        [SerializeField] private List<SceneField> mapScenes;
        
        public List<SceneField> GetMapScenes()
        {
            return mapScenes;
        }

    }
}