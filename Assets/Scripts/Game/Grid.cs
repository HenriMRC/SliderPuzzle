using Application.States.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Grid : MonoBehaviour, IMessageSender<Messages>
    {
        private const int SLOT_SIZE = 50;

        [SerializeField] private GameObject m_Prefab = null;
        [SerializeField] private GameMenu m_GameMenu = null;

        private int m_GridSide = 4;

        private int m_ShuffleCount = 40;
        private int m_MovesCount = 0;
        private int m_Speed = 0;

        private SliderPiece[,] m_Sliders = null;

        private Vector2[,] m_Positions;

        private Vector2Int m_Empty;

        private RectTransform rectTransform => transform as RectTransform;
        public int movesCount
        {
            get
            {
                return m_MovesCount;
            }
            private set
            {
                m_MovesCount = value;
                m_GameMenu.movesCount = value;
            }
        }

        private IMessageReceiver<Messages> m_State;

        public bool playing
        {
            set
            {
                m_Playing = value;

                Color _color = value ? Color.green : Color.red;

                m_Background.color = _color;
                for (int i = 0; i < m_GridSide; i++)
                    for (int j = 0; j < m_GridSide; j++)
                        if (m_Sliders[i, j] != null)
                            m_Sliders[i, j].color = _color;
            }
        }
        private bool m_Playing = false;

        [SerializeField] private Image m_Background = null;

        public void Configure(int gridSide, int shuffleCount, int shuffleSpeed)
        {
            m_GridSide = gridSide;
            m_ShuffleCount = shuffleCount;
            m_Speed = shuffleSpeed;
            movesCount = 0;

            rectTransform.sizeDelta = m_GridSide * SLOT_SIZE * Vector2.one;

            m_Sliders = new SliderPiece[m_GridSide, m_GridSide];
            m_Positions = new Vector2[m_GridSide, m_GridSide];

            for (int i = 0; i < m_GridSide; i++)
            {
                for (int j = 0; j < m_GridSide; j++)
                {
                    m_Positions[i, j] = (new Vector2(i, -j) - new Vector2(m_GridSide - 1, -m_GridSide + 1) / 2) * SLOT_SIZE;

                    if (i == j && j == m_GridSide - 1)
                        break;

                    m_Sliders[i, j] = Instantiate(m_Prefab, transform).GetComponent<SliderPiece>();
                    m_Sliders[i, j].transform.localPosition = m_Positions[i, j];
                    m_Sliders[i, j].Setup(j * m_GridSide + i + 1, i, j, this);
                    m_Sliders[i, j].gameObject.name = "Number_" + (i + j * m_GridSide + 1).ToString();
                }
            }

            m_Empty = Vector2Int.one * (m_GridSide - 1);
        }

        public void Destroy()
        {
            for (int i = 0; i < m_GridSide; i++)
                for (int j = 0; j < m_GridSide; j++)
                    if (m_Sliders[i, j] != null)
                        Object.Destroy(m_Sliders[i, j].gameObject);
        }

        public Coroutine StartSuffle()
        {
            return StartCoroutine(Shuffle());
        }

        private IEnumerator Shuffle()
        {
            int i = 0;
            Vector2Int lastMove = -Vector2Int.one;

            float time = 0;

            while (i < m_ShuffleCount)
            {
                yield return null;

                time += m_Speed * Time.deltaTime;

                while (time > 1 && i < m_ShuffleCount)
                {
                    lastMove = MoveRandom(lastMove);
                    i++;
                    time--;
                }
            }
        }

        private Vector2Int MoveRandom(Vector2Int lastMove)
        {
            List<Vector2Int> possibilities = new List<Vector2Int>();

            if (m_Empty.x - 1 > -1)
            {
                possibilities.Add(m_Empty + Vector2Int.left);
            }
            if (m_Empty.y - 1 > -1)
            {
                possibilities.Add(m_Empty + Vector2Int.down);
            }
            if (m_Empty.x + 1 < m_GridSide)
            {
                possibilities.Add(m_Empty + Vector2Int.right);
            }
            if (m_Empty.y + 1 < m_GridSide)
            {
                possibilities.Add(m_Empty + Vector2Int.up);
            }

            possibilities.Remove(lastMove);

            if (possibilities.Count != 0)
            {
                Vector2Int choice = possibilities[Random.Range(0, possibilities.Count)];
                m_Sliders[choice.x, choice.y].Move(m_Positions[m_Empty.x, m_Empty.y], m_Empty);
                m_Sliders[m_Empty.x, m_Empty.y] = m_Sliders[choice.x, choice.y];
                m_Sliders[choice.x, choice.y] = null;
                lastMove = m_Empty;
                m_Empty = choice;
            }

            return lastMove;
        }

        public void RequestMove(Vector2Int position, SliderPiece piece)
        {
            if (m_Playing)
                if ((m_Empty.y - position.y == 0 && (m_Empty.x - position.x == 1 || position.x - m_Empty.x == 1)) || (m_Empty.x - position.x == 0 && (m_Empty.y - position.y == 1 || position.y - m_Empty.y == 1)))
                {
                    piece.Move(m_Positions[m_Empty.x, m_Empty.y], m_Empty);
                    m_Sliders[m_Empty.x, m_Empty.y] = m_Sliders[position.x, position.y];
                    m_Sliders[position.x, position.y] = null;
                    m_Empty = position;

                    movesCount++;

                    for (int i = 0; i < m_GridSide; i++)
                        for (int j = 0; j < m_GridSide; j++)
                            if ((m_Sliders[i, j]?.startPosition ?? new Vector2Int(i, j)) != new Vector2Int(i, j))
                                return;

                    m_State.ReceiveMessage(Messages.Victory);
                }
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
    }
}