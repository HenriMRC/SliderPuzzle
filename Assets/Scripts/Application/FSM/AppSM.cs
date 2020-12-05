using Application.States;
using Infra.FSM;
using UnityEngine;
using Menu;
using Game;
using Grid = Game.Grid;

namespace Application.FSM
{
    public class AppSM : FSM<StatesEnum>
    {
        [SerializeField] private MainMenu m_MainMenu = null;
        [SerializeField] private Grid m_Grid = null;
        [SerializeField] private GameMenu m_GameMenu = null;
        [SerializeField] private VictoryMenu m_VictoryMenu = null;

        protected override StatesEnum InitialState => StatesEnum.MainMenu;

        protected override State<StatesEnum> SwitchStatement(StatesEnum state)
        {
            switch(state)
            {
                case StatesEnum.MainMenu:
                    return new OnMainMenuState(this, m_MainMenu);
                case StatesEnum.Shuffle:
                    return new OnShufflingState(this, m_MainMenu, m_Grid);
                case StatesEnum.Playing:
                    return new OnPlayingState(this, m_Grid, m_GameMenu);
                case StatesEnum.Victory:
                    return new OnVictoryState(this, m_Grid, m_VictoryMenu);
                default:
                    throw new System.NotImplementedException($"{state}");
            }
        }
    }
}