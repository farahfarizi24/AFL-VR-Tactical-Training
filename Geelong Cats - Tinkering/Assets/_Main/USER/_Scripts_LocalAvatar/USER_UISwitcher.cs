using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using System;
namespace com.DU.CE.USER
{
    public class USER_UISwitcher : MonoBehaviour
    {
        [SerializeField] private bool m_IsUIOpen;

        [Header("Input Actions")]
        //[SerializeField] private InputActionProperty UI_Grip;
        public InputActionReference UI_Grip=null;
        [Header("References")]
        [SerializeField] private USER_LocalUser m_LocalUser = null;
      
        private SOC_AUserUI m_uiSock = null;

        private bool m_canOpenUI = false;

        private void Awake()
        {
            m_uiSock = m_LocalUser.UserSock.UISock;
            m_IsUIOpen = false;
            ToggleUI(false);
          
            UI_Grip.action.started += LeftTriggerPress;
        }
        private void OnDestroy()
        {
            UI_Grip.action.started -= LeftTriggerPress;
        }
        private void OnEnable()
        {
        }

        private void OnDisable()
        {
        }

        private void OnTriggerEnter(Collider other)
        {
           /* if (other.gameObject.layer == m_uiSock.HANDLAYER)
            {
                UI_Grip.action.performed += LeftTriggerPress;
                m_canOpenUI = true;
            }*/
        }

        private void OnTriggerExit(Collider other)
        {
           /* if (other.gameObject.layer == m_uiSock.HANDLAYER)
            {
                UI_Grip.action.performed -= OnUIGripped;
                m_canOpenUI = false;
            }*/
        }

        private void LeftTriggerPress(InputAction.CallbackContext obj)
        {
            if (m_IsUIOpen == false )
            {
                m_IsUIOpen = true;
                ToggleUI(true);
                Debug.Log("UI START");
            }
            else
            {
                m_IsUIOpen = false;
                ToggleUI(false);
            }

           /* if (m_IsUIOpen)// || !m_canOpenUI)
                return;

            m_IsUIOpen = true;
            ToggleUI(true);
            UI_Grip.action.canceled += OnUIReleased;*/
        }

        private void OnUIReleased(InputAction.CallbackContext obj)
        {
            if (!m_IsUIOpen)
                return;

            m_IsUIOpen = false;
            ToggleUI(false);
            UI_Grip.action.canceled -= OnUIReleased;
            Debug.Log("Released UI for coach");
        }


        public void ToggleUI(bool _toggle)
        {
            m_LocalUser.UserSock.UISock.ToggleHandUI(_toggle);
            Debug.Log("Toggle UI for coach");
        }
    }
}



