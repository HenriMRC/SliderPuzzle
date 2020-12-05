using Application.States.Messages;
using TMPro;
using UnityEngine;

namespace Game
{
    public class GameMenu : MonoBehaviour, IMessageSender<Messages>
    {
        [SerializeField] private TMP_Text m_MovesCount = null;

        public int movesCount
        {
            set
            {
                m_MovesCount.text = value.ToString();
            }
        }
        

        private IMessageReceiver<Messages> m_State = null;

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