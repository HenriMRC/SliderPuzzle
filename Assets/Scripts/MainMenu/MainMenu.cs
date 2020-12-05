using Application.States.Messages;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Menu
{
    public class MainMenu : MonoBehaviour, IMessageSender<Messages>
    {
        [SerializeField] private GameObject m_Tooltip = null;
        [SerializeField] private TMP_Text m_TooltipText = null;

        [SerializeField] private TMP_Text m_GridSideText = null;
        [SerializeField] private TMP_Text m_MovesText = null;
        [SerializeField] private TMP_Text m_SpeedText = null;

        [SerializeField] private Button m_GridSide_Plus = null;
        [SerializeField] private Button m_GridSide_Minus = null;
        [SerializeField] private Button m_Moves_Plus = null;
        [SerializeField] private Button m_Moves_Minus = null;
        [SerializeField] private Button m_Speed_Plus = null;
        [SerializeField] private Button m_Speed_Minus = null;

        private Coroutine m_CursorUpdate;

        [SerializeField] private Vector2Int m_GridRange;
        [SerializeField] private Vector2Int m_MovesRange;
        [SerializeField] private Vector2Int m_SpeedRange;

        private int m_GridSide;
        private int m_Moves;
        private int m_Speed;

        public int gridSide => m_GridSide;
        public int moves => m_Moves;
        public int speed => m_Speed;

        private IMessageReceiver<Messages> m_State;

        private void Awake()
        {
            m_GridSide = 3;
            m_Moves = 40;
            m_Speed = 8;

            m_GridSideText.text = m_GridSide.ToString();
            m_MovesText.text = m_Moves.ToString();
            m_SpeedText.text = m_Speed.ToString();

            Dictionary<float, int> dict = new Dictionary<float, int>() { { 0, 1 }, { 1.5f, 10 } };

            m_GridSide_Plus.paceChanges = dict;
            m_GridSide_Minus.paceChanges = dict;
            m_Moves_Plus.paceChanges = dict;
            m_Moves_Minus.paceChanges = dict;
            m_Speed_Plus.paceChanges = dict;
            m_Speed_Minus.paceChanges = dict;

            m_GridSide_Plus.onTick += GridSideIncrease;
            m_GridSide_Minus.onTick += GridSideDecrease;
            m_Moves_Plus.onTick += MoveIncrease;
            m_Moves_Minus.onTick += MoveDecrease;
            m_Speed_Plus.onTick += SpeedIncrease;
            m_Speed_Minus.onTick += SpeedDecrease;
        }

        public void HideTooltip()
        {
            m_Tooltip.SetActive(false);
            StopCoroutine(m_CursorUpdate);
        }

        public void ShowTooltip(string text)
        {
            m_Tooltip.SetActive(true);
            m_TooltipText.text = text;


            (m_Tooltip.transform as RectTransform).sizeDelta = m_TooltipText.GetPreferredValues(text);

            m_CursorUpdate = StartCoroutine(CursorUpdate());
        }

        private IEnumerator CursorUpdate()
        {
            while (true)
            {
                m_Tooltip.transform.position = Input.mousePosition;
                yield return null;
            }
        }

        public void GridSideIncrease(int amount)
        {
            m_GridSide = Mathf.Min(m_GridRange.y, Mathf.Max(m_GridRange.x, m_GridSide + amount));
            m_GridSideText.text = m_GridSide.ToString();
        }

        public void MoveIncrease(int amount)
        {
            m_Moves = Mathf.Min(m_MovesRange.y, Mathf.Max(m_MovesRange.x, m_Moves + amount));
            m_MovesText.text = m_Moves.ToString();
        }

        public void SpeedIncrease(int amount)
        {
            m_Speed = Mathf.Min(m_SpeedRange.y, Mathf.Max(m_SpeedRange.x, m_Speed + amount));
            m_SpeedText.text = m_Speed.ToString();
        }

        public void GridSideDecrease(int amount)
        {
            m_GridSide = Mathf.Min(m_GridRange.y, Mathf.Max(m_GridRange.x, m_GridSide - amount));
            m_GridSideText.text = m_GridSide.ToString();
        }

        public void MoveDecrease(int amount)
        {
            m_Moves = Mathf.Min(m_MovesRange.y, Mathf.Max(m_MovesRange.x, m_Moves - amount));
            m_MovesText.text = m_Moves.ToString();
        }

        public void SpeedDecrease(int amount)
        {
            m_Speed = Mathf.Min(m_SpeedRange.y, Mathf.Max(m_SpeedRange.x, m_Speed - amount));
            m_SpeedText.text = m_Speed.ToString();
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
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

        public void Confirm()
        {
            m_State.ReceiveMessage(Messages.MainMenuConfirm);
        }
    }
}