using UnityEngine;

namespace GonDraz.UI.Route
{
    public class Route : MonoBehaviour
    {
        private bool _onPress;

        private void Awake()
        {
            var widgets = GetComponentsInChildren<Presentation>();
            RouteManager.AddWidgets(widgets);
            RouteManager.SetRouteParent(transform);
        }

        private void LateUpdate()
        {
            BackButtonPressed();
        }

        public void BackButtonPressed()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                if (!_onPress) RouteManager.Pop();
                _onPress = true;
            }
            else
            {
                _onPress = false;
            }
        }
    }
}