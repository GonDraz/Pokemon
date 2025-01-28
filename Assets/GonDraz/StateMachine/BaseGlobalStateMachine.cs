namespace GonDraz.StateMachine
{
    public abstract class
        BaseGlobalStateMachine<TMachine> : BaseStateMachine<TMachine, BaseGlobalStateMachine<TMachine>.BaseGlobalState>
        where TMachine : BaseGlobalStateMachine<TMachine>
    {
        public static TMachine Instance { get; private set; }

        protected override void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }

            Instance = this as TMachine;
            base.Awake();
        }

        public abstract class BaseGlobalState : BaseState<TMachine, BaseGlobalState>
        {
        }
    }
}