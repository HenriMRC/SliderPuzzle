using Application.States;
using Infra.FSM;
using Menu;
using System.Collections;
using UnityEngine;
using Grid = Game.Grid;

public class OnShufflingState : State<StatesEnum>
{
    private Grid m_Grid;

    public OnShufflingState(FSM<StatesEnum> fsm, MainMenu mainMenu, Grid grid) : base(fsm)
    {
        m_Grid = grid;
        m_Grid.Configure(mainMenu.gridSide, mainMenu.moves, mainMenu.speed);
    }

    protected override void Enter()
    {
        m_Grid.gameObject.SetActive(true);

        m_Grid.StartCoroutine(EnterCoroutine());
    }

    private IEnumerator EnterCoroutine()
    {
        yield return new WaitForSeconds(1);

        yield return m_Grid.StartSuffle();

        ChangeState(StatesEnum.Playing);
    }

    protected override void Exit()
    {

    }
}
