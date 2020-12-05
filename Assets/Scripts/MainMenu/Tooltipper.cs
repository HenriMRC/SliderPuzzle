using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Menu
{
    public class Tooltipper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private MainMenu m_MainMenu = null;
        [SerializeField] private string m_Message = null;

        public void OnPointerEnter(PointerEventData eventData)
        {
            m_MainMenu.ShowTooltip(m_Message);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            m_MainMenu.HideTooltip();
        }
    }
}