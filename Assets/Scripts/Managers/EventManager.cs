using GonDraz.Events;

namespace GonDraz.Managers
{
    public partial class EventManager
    {
        public static Event<string> MapTriggerEnter = new("MapTriggerEnter");
    }
}