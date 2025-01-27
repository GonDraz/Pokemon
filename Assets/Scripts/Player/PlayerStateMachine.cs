using System;
using System.Collections.Generic;
using GonDraz.StateMachine;

namespace Player
{
    public class PlayerStateMachine : BaseStateMachine<PlayerStateMachine.PlayerState>
    {
        public abstract class PlayerState : BaseState
        {
        }

        private class Idle : PlayerState
        {
        }

        private class Walk : PlayerState
        {
        }

        protected override bool IsDontDestroyOnLoad()
        {
            return false;
        }

        public override Dictionary<Type, PlayerState> States()
        {
            return new Dictionary<Type, PlayerState>
            {
                { typeof(Idle), new Idle() },
                { typeof(Walk), new Walk() }
            };
        }

        public override Type InitialState()
        {
            return typeof(Idle);
        }
    }
}