using UnityEngine;
using UnityEngine.InputSystem;

using Autohand;

namespace com.DU.CE.USER
{
    public class USER_Teleporter : MonoBehaviour
    {
        public InputActionProperty TeleportAction;
        public Teleporter Hand;

        //bool m_teleporting = false;

        private void OnEnable()
        {
            TeleportAction.action.performed += OnTeleporting;
            TeleportAction.action.canceled += OnTeleport;
        }

        private void OnDisable()
        {
            TeleportAction.action.performed -= OnTeleporting;
            TeleportAction.action.canceled -= OnTeleport;
        }

        private void OnTeleporting(InputAction.CallbackContext obj)
        {
            Hand.StartTeleport();
            //m_teleporting = true;
        }

        private void OnTeleport(InputAction.CallbackContext obj)
        {
            Hand.Teleport();
            //m_teleporting = false;
        }
    }
}
