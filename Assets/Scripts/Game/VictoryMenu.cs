using Application.States.Messages;
using TMPro;
using UnityEngine;

namespace Game
{
    public class VictoryMenu : MonoBehaviour, IMessageSender<Messages>
    {
        private const string MOVE = "Move";
        private const string MOVES = "Moves";

        [SerializeField] private TMP_Text m_MovesCount = null;
        [SerializeField] private TMP_Text m_Moves = null;

        private IMessageReceiver<Messages> m_State = null;

        public void Write(int movesCount)
        {
            if (movesCount == 1)
                m_Moves.text = MOVE;
            else
                m_Moves.text = MOVES;

            m_MovesCount.text = movesCount.ToString();
        }

        public void AddReceiver(IMessageReceiver<Messages> receiver)
        {
            m_State = receiver;
        }

        public void RemoveReceiver(IMessageReceiver<Messages> receiver)
        {
            if (m_State == receiver)
                m_State = null;
        }

        public void ButtonReturn()
        {
            m_State.ReceiveMessage(Messages.ReturnToMainMenu);
        }

        public void ButtonRestart()
        {
            m_State.ReceiveMessage(Messages.RestartGame);
        }
    }
}