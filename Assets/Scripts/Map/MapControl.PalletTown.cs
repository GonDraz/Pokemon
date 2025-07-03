using System;
using System.Collections.Generic;

namespace Map
{
    public partial class MapControl
    {
        public class PalletTown : MapState
        {
            protected override List<Type> ScenesToLoad()
            {
                return new List<Type>
                {
                    typeof(Route1),
                    typeof(Route21)
                };
            }
        }
    }
}