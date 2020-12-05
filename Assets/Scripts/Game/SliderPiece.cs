using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace Game
{
    public class SliderPiece : MonoBehaviour, IPointerClickHandler
    {
        private int number
        {
            set
            {
                m_Number = value;
                m_Text.text = value.ToString();
            }
        }
        public Vector2Int startPosition => m_startPosition;

        internal Color color
        {
            set
            {
                m_Text.color = value;
            }
        }

        private int m_Number = 0;
        private Vector2Int m_startPosition = Vector2Int.zero;
        private Vector2Int m_Position = Vector2Int.zero;
        [SerializeField] private TMP_Text m_Text = null;

        private Grid m_Grid = null;

        public void Setup(int index, int x, int y, Grid grid)
        {
            number = index;
            m_startPosition = m_Position = new Vector2Int(x, y);
            m_Grid = grid;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            m_Grid.RequestMove(m_Position, this);
        }

        public void Move(Vector2 realPosition, Vector2Int newGridPosition)
        {
            transform.localPosition = realPosition;
            m_Position = newGridPosition;
        }
    }
}