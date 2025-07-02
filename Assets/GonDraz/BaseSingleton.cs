namespace GonDraz
{
    public abstract class BaseSingleton<T> : Base where T : Base
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }

            Instance = this as T;
            if (IsDontDestroyOnLoad()) DontDestroyOnLoad(this);
        }

        protected virtual void OnApplicationQuit()
        {
            Instance = null;
            Destroy(gameObject);
        }

        protected abstract bool IsDontDestroyOnLoad();
    }
}