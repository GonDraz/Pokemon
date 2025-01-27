using System;
using System.Collections.Generic;
using UnityEngine;

namespace GonDraz.UI.Route
{
    public static partial class RouteManager
    {
        private static readonly Dictionary<Type, Presentation> Presentations = new();

        private static readonly Stack<Type[]> _stackBased = new();

        private static Transform _routeParent;

        public static void SetRouteParent(Transform parent)
        {
            if (!_routeParent)
                _routeParent = parent;
            else
                Debug.LogError("ScreensParent not null");
        }

        public static void Go(params Type[] types)
        {
            ClearPresentations();
            ShowPresentations(types);
            _stackBased.Push(types);
        }

        public static void PushReplacement(params Type[] types)
        {
            var oldType = _stackBased.Peek();
            ClearPresentations();
            ShowPresentations(types);
            _stackBased.Push(oldType);
            _stackBased.Push(types);
        }

        public static void Push(params Type[] types)
        {
            ShowPresentations(types);
            _stackBased.Push(types);
        }

        private static void ShowPresentations(Type[] types)
        {
            foreach (var type in types)
                if (Presentations.TryGetValue(type, out var widget))
                {
                    widget.Show();
                    widget.transform.SetSiblingIndex(_routeParent.childCount);
                }
        }

        private static void HidePresentations(Type[] types)
        {
            foreach (var type in types)
                if (Presentations.TryGetValue(type, out var widget))
                {
                    widget.Hide();
                    widget.transform.SetSiblingIndex(0);
                }
        }

        private static void ClearPresentations()
        {
            foreach (var presentation in Presentations.Values) presentation.Hide();
            _stackBased.Clear();
        }

        public static void Pop()
        {
            if (_stackBased.Count > 1)
            {
                if (_stackBased.TryPop(out var types))
                {
                    HidePresentations(types);
                    ShowPresentations(_stackBased.Peek());
                }
            }
            else
            {
#if !UNITY_EDITOR
                Application.Quit();
#endif
            }
        }
    }
}