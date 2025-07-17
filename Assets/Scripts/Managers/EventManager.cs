using GonDraz.Events;

namespace GonDraz.Managers
{
    public partial class EventManager
    {
        public static GEvent<string> MapTriggerEnter = new("MapTriggerEnter");
    }
}