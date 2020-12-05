using System;
using UnityEngine;

namespace Infra.FSM
{
    public abstract class FSM<StateEnum> : MonoBehaviour where StateEnum : Enum
    {
        [Header("FSM")]
        private State<StateEnum> m_StateObject = null;

        public StateEnum state => m_State;
        private StateEnum m_State = default;

        protected abstract StateEnum InitialState { get; }

        protected virtual void Awake()
        {
            StartState(InitialState);
        }

        private void StartState(StateEnum state)
        {
            m_State = state;
            m_StateObject = SwitchStatement(m_State);

            m_StateObject.Enter();
            Debug.Log($"[{this.GetType().Name}] ENTERED STATE: {m_State}");
        }

        protected abstract State<StateEnum> SwitchStatement(StateEnum state);

        internal void GoToState(StateEnum state)
        {
            m_StateObject.Exit();
            Debug.Log($"[{this.GetType().Name}] EXITED STATE: {m_State}");

            m_State = state;
            m_StateObject = SwitchStatement(m_State);

            m_StateObject.Enter();
            Debug.Log($"[{this.GetType().Name}] ENTERED STATE: {m_State}");
        }
    }
}