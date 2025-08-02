using System;
using GonDraz.StateMachine;

public partial class GlobalStateMachine : BaseGlobalStateMachine<GlobalStateMachine>
{
    private void Start()
    {
    }

    protected override Type InitialState()
    {
        return typeof(PreLoaderState);
    }
}