using UnityEngine;

namespace GonDraz.UI.Route
{
    public static partial class RouteManager
    {
        public static void AddWidgets(Presentation[] widgets)
        {
            foreach (var widget in widgets) AddWidget(widget);
        }

        public static void AddWidget(Presentation screen)
        {
            var type = screen.GetType();
            if (!Presentations.TryAdd(type, screen))
                Debug.LogError($"Cannot add screen [{type}]");
        }
    }
}