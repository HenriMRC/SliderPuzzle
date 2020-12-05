using Infra.FSM;
using Menu;
using System;

namespace Application.States
{
    public class OnMainMenuState : State<StatesEnum>, IMessageReceiver<Messages.Messages>
    {
        private MainMenu m_MainMenu;

        public OnMainMenuState(FSM<StatesEnum> fsm, MainMenu mainMenu) : base(fsm)
        {
            m_MainMenu = mainMenu;
        }

        public void ReceiveMessage(Messages.Messages message)
        {
            switch (message)
            {
                case Messages.Messages.MainMenuConfirm:
                    ChangeState(StatesEnum.Shuffle);
                    break;
                default:
                    throw new NotImplementedException(message.ToString());
            }
        }

        protected override void Enter()
        {
            m_MainMenu.AddReceiver(this);
            m_MainMenu.SetActive(true);
        }

        protected override void Exit()
        {
            m_MainMenu.SetActive(false);
        }
    }
}