using GonDraz.Events;

namespace GonDraz.Managers
{
    public partial class EventManager
    {
        public static Event ApplicationLoadFinished = new("ApplicationLoadFinished");
        public static Event<bool> ApplicationPause = new("ApplicationPause");
        public static Event GamePause = new("GamePause");
    }
}