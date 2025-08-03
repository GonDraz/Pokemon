using Cysharp.Threading.Tasks;
using System;
using GonDraz.Events;
using GonDraz.Interfaces;
using GonDraz.StateMachine;

public partial class GlobalStateMachine : BaseGlobalStateMachine<GlobalStateMachine>, IAsyncInitProgress
{
    private void Start()
    {
    }

    protected override Type InitialState()
    {
        return typeof(PreLoaderState);
    }

    public async UniTask InitAsync()
    {

    }
}