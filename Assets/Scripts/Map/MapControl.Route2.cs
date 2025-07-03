using System;
using System.Collections.Generic;

namespace Map
{
    public partial class MapControl
    {
        public class Route2 : MapState
        {
            protected override List<Type> ScenesToLoad()
            {
                return new List<Type>();
            }
        }
    }
}