using UnityEngine;
using UnityEngine.InputSystem;

namespace com.DU.CE.USER
{
    public class USER_UISwitcher : MonoBehaviour
    {
        [SerializeField] private bool m_IsUIOpen;

        [Header("Input Actions")]
        [SerializeField] private InputActionProperty m_GripAction;

        [Header("References")]
        [SerializeField] private USER_LocalUser m_LocalUser = null;

        private SOC_AUserUI m_uiSock = null;

        private bool m_canOpenUI = false;

        private void Awake()
        {
            m_uiSock = m_LocalUser.UserSock.UISock;
            ToggleUI(false);
        }

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == m_uiSock.HANDLAYER)
            {
                m_GripAction.action.performed += OnUIGripped;
                m_canOpenUI = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == m_uiSock.HANDLAYER)
            {
                m_GripAction.action.performed -= OnUIGripped;
                m_canOpenUI = false;
            }
        }

        private void OnUIGripped(InputAction.CallbackContext obj)
        {
            if (m_IsUIOpen)// || !m_canOpenUI)
                return;

            m_IsUIOpen = true;
            ToggleUI(true);
            m_GripAction.action.canceled += OnUIReleased;
        }

        private void OnUIReleased(InputAction.CallbackContext obj)
        {
            if (!m_IsUIOpen)
                return;

            m_IsUIOpen = false;
            ToggleUI(false);
            m_GripAction.action.canceled -= OnUIReleased;
            Debug.Log("Released UI for coach");
        }


        private void ToggleUI(bool _toggle)
        {
            m_LocalUser.UserSock.UISock.ToggleHandUI(_toggle);
            Debug.Log("Toggle UI for coach");
        }
    }
}



