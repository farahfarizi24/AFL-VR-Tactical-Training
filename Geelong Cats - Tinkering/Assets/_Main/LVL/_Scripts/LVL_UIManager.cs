using UnityEngine;

using com.DU.CE.NET;
using TMPro;

namespace com.DU.CE.LVL
{ 
    public class LVL_UIManager : MonoBehaviour
    {
        [SerializeField] SOC_LvlStateMachine m_SockStateMachine = null;
        [SerializeField] SOC_NetManager m_NetManagerSock = null;

        [Space]
        [SerializeField] Canvas m_UIParentCanvas = null;
        [SerializeField] Canvas m_ChooseRoleCanvas = null;
        [SerializeField] Canvas m_ChooseRoomCanvas = null;
        [SerializeField] Canvas m_PlayerNameCanvas = null;

        [Space]
        [SerializeField] TextMeshProUGUI m_DebugText = null;

        private void OnEnable()
        {
            m_SockStateMachine.OnStateChange += HandleStateChange;

            m_NetManagerSock.OnDisconnected += OnDisconnected;
        }

        private void OnDisable()
        {
            m_SockStateMachine.OnStateChange -= HandleStateChange;

            m_NetManagerSock.OnDisconnected += OnDisconnected;
        }

        private void OnDisconnected(int _messageID, string _message)
        {
            m_UIParentCanvas.enabled = true;

            m_ChooseRoleCanvas.enabled = true;
            m_ChooseRoomCanvas.enabled = false;
            m_PlayerNameCanvas.enabled = false;

            m_DebugText.enabled = true;
            m_DebugText.text = _message;
        }

        private void HandleStateChange(ELVLSTATE oldState, ELVLSTATE newState)
        {
            switch (newState)
            {
                case ELVLSTATE.ONSTART_SETUP:

                    m_UIParentCanvas.enabled = true;

                    break;

                case ELVLSTATE.ONCONNECT_SETUP:

                    EnableUIAfterConnect();

                    break;
            }
        }

        internal void EnableUIAfterConnect()
        {
            m_ChooseRoleCanvas.enabled = false;
            m_ChooseRoomCanvas.enabled = false;
            m_PlayerNameCanvas.enabled = true;

            m_DebugText.enabled = false;
        }
    }
}