using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Menu
{
    [RequireComponent(typeof(Image))]
    public class Button : UIBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private State state
        {
            set
            {
                m_State = value;
                switch (value)
                {
                    case State.None:
                        m_Image.color = m_Basic;
                        break;
                    case State.Hovering:
                        m_Image.color = m_Hover;
                        break;
                    case State.Pressed:
                        m_Image.color = m_Pressed;
                        break;
                }
            }
        }

        private State m_State = State.None;

        private float m_TickTimer = 0;
        private float m_TickTime = 0.25f;
        private int m_TickCount = 0;

        [SerializeField] private Image m_Image;
        [SerializeField] private Color m_Basic = Color.white;
        [SerializeField] private Color m_Hover = Color.white;
        [SerializeField] private Color m_Pressed = Color.white;

        public event Action<int> onTick;

        public Dictionary<float, int> paceChanges;

#if UNITY_EDITOR
        protected override void Reset()
        {
            m_Image = GetComponent<Image>();
            m_Image.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        }
#endif

        private IEnumerator Tick()
        {
            m_TickCount++;
            onTick(1);

            yield return null;


            while (m_State == State.Pressed)
            {
                int tickMultiplier = 1;

                if (paceChanges != null)
                {
                    IEnumerator keys = paceChanges.Keys.GetEnumerator();

                    keys.Reset();

                    int count = 0;
                    while (keys.MoveNext() && (float)keys.Current < m_TickTimer)
                        count++;

                    IEnumerator values = paceChanges.Values.GetEnumerator();
                    values.Reset();
                    values.MoveNext();
                    for (int i = 1; i < count; i++)
                        values.MoveNext();

                    tickMultiplier = (int)values.Current;
                }

                tickMultiplier = Mathf.Max(tickMultiplier, 1);

                m_TickTimer += Time.deltaTime;

                if (m_TickTimer > m_TickTime * m_TickCount)
                {
                    int ticks = (int)((m_TickTimer - m_TickTime * m_TickCount) / m_TickTime);
                    m_TickCount += ticks;
                    onTick(ticks * tickMultiplier);
                }

                yield return null;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            switch (m_State)
            {
                case State.None:
                    break;
                case State.Hovering:
                    StartCoroutine(Tick());
                    break;
                case State.Pressed:
                    break;
            }

            state = State.Pressed;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (m_State != State.Pressed)
                return;

            StopCoroutine(Tick());

            state = State.Hovering;

            m_TickTimer = 0;
            m_TickCount = 0;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            state = State.Hovering;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            state = State.None;
        }


        private enum State
        {
            None = 0,
            Hovering = 1,
            Pressed = 2
        }
    }
}