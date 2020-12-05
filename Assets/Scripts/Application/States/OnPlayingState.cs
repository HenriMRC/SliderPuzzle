using Game;
using Infra.FSM;

namespace Application.States
{
    public class OnPlayingState : State<StatesEnum>, IMessageReceiver<Messages.Messages>
    {
        private Grid m_Grid;
        private GameMenu m_GameMenu;

        public OnPlayingState(FSM<StatesEnum> fsm, Grid grid, GameMenu gameMenu) : base(fsm)
        {
            m_Grid = grid;
            m_GameMenu = gameMenu;
        }

        public void ReceiveMessage(Messages.Messages message)
        {
            switch(message)
            {
                case Messages.Messages.RestartGame:
                    ChangeState(StatesEnum.Shuffle);
                    break;
                case Messages.Messages.ReturnToMainMenu:
                    ChangeState(StatesEnum.MainMenu);
                    break;
                case Messages.Messages.Victory:
                    ChangeState(StatesEnum.Victory);
                    break;
                default:
                    throw new System.NotImplementedException();
            }
        }

        protected override void Enter()
        {
            m_Grid.playing = true;
            m_GameMenu.gameObject.SetActive(true);

            m_Grid.AddReceiver(this);
            m_GameMenu.AddReceiver(this);
        }

        protected override void Exit()
        {
            m_Grid.Destroy();
            m_Grid.playing = false;
            m_Grid.gameObject.SetActive(false);
            m_GameMenu.gameObject.SetActive(false);

            m_Grid.RemoveReceiver(this);
            m_GameMenu.RemoveReceiver(this);
        }
    }
}