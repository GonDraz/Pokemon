namespace GonDraz.StateMachine
{
    public abstract class
        BaseGlobalStateMachine<TMachine> : BaseStateMachine<TMachine, BaseGlobalStateMachine<TMachine>.BaseGlobalState>
        where TMachine : BaseGlobalStateMachine<TMachine>
    {
        public static TMachine Instance { get; private set; }

        protected void Awake()
        {
            if (Instance)
            {
                Destroy(this);
                return;
            }

            Instance = this as TMachine;
            DontDestroyOnLoad(this);
        }

        protected override void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        public abstract class BaseGlobalState : BaseState<TMachine, BaseGlobalState>
        {
        }
    }
}