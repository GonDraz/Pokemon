using GonDraz.Events;

namespace GonDraz.Managers
{
    public abstract class EventManager
    {
        public static Event ApplicationLoadFinished = new("ApplicationLoadFinished");
        public static Event<bool> ApplicationPause = new("ApplicationPause");
    }
}