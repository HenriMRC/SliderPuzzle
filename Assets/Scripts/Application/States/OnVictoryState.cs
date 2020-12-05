using Game;
using Infra.FSM;

namespace Application.States
{
    public class OnVictoryState : State<StatesEnum>, IMessageReceiver<Messages.Messages>
    {
        private Grid m_Grid;
        private VictoryMenu m_VictoryMenu;

        public OnVictoryState(FSM<StatesEnum> fsm, Grid grid, VictoryMenu victoryMenu) : base(fsm)
        {
            m_Grid = grid;
            m_VictoryMenu = victoryMenu;
        }


        public void ReceiveMessage(Messages.Messages message)
        {
            switch (message)
            {
                case Messages.Messages.RestartGame:
                    ChangeState(StatesEnum.Shuffle);
                    break;
                case Messages.Messages.ReturnToMainMenu:
                    ChangeState(StatesEnum.MainMenu);
                    break;
                default:
                    throw new System.NotImplementedException();
            }
        }

        protected override void Enter()
        {
            m_VictoryMenu.gameObject.SetActive(true);
            m_VictoryMenu.Write(m_Grid.movesCount);
            m_VictoryMenu.AddReceiver(this);
        }

        protected override void Exit()
        {
            m_VictoryMenu.gameObject.SetActive(false);
            m_VictoryMenu.RemoveReceiver(this);
        }
    }
}