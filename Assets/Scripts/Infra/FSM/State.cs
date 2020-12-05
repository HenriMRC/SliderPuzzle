using System;

namespace Infra.FSM
{
    public abstract class State<StateEnum> where StateEnum : Enum
    {
        private FSM<StateEnum> m_FSM = null;

        public State(FSM<StateEnum> fsm)
        {
            m_FSM = fsm;
        }

        protected void ChangeState(StateEnum newState)
        {
            m_FSM.GoToState(newState);
        }

        protected internal abstract void Enter();

        protected internal abstract void Exit();
    }
}