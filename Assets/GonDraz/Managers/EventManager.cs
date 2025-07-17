using GonDraz.Events;

namespace GonDraz.Managers
{
    public partial class EventManager
    {
        public static GEvent ApplicationLoadFinished = new("ApplicationLoadFinished");
        public static GEvent<bool> ApplicationPause = new("ApplicationPause");
        public static GEvent GamePause = new("GamePause");
    }
}