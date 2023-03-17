using UnityEngine;

using com.DU.CE.LVL;
using com.DU.CE.NET;
using com.DU.CE.INT;

namespace com.DU.CE.USER
{
    public class USER_Manager : MonoBehaviour
    {
        [SerializeField] private SOC_LvlStateMachine m_LvlStateMachineSock = null;
        [SerializeField] private SOC_NetManager m_NetManagerSock = null;
        [SerializeField] private SOC_UserStateMachine m_UserStateMachineSock = null;
        [SerializeField] private SOC_StadiumProperties m_StadiumPropSock = null;
        [Space]
        [SerializeField] private USER_Teleporter m_Teleporter = null;
        [SerializeField] private USER_Mover m_Mover = null;
        [Space]
        [SerializeField] private SOC_FieldBoard m_BoardSock = null;
        [SerializeField] private Camera m_EagleEyeCamera = null;

        private SOC_User m_localUserSock = null;
        public SOC_User UserSock { get => m_localUserSock; }

        public bool m_isLockedInPlace = false;


        private void Awake()
        {
            m_BoardSock.EagleEyeCamera = m_EagleEyeCamera;
        }

        private void OnEnable()
        {
            m_LvlStateMachineSock.OnStateChange += OnLevelStateChange;

            m_UserStateMachineSock.OnStateChange += OnUserStateChange;
            m_UserStateMachineSock.OnHandStateChange += OnHandStateChange;
            //m_localUserSock.OnRequestTeleport += OnTeleportRequest;
            m_BoardSock.OnTeleportRequest += OnTeleportRequest;
        }

        private void OnDisable()
        {
            m_LvlStateMachineSock.OnStateChange -= OnLevelStateChange;
            m_UserStateMachineSock.OnStateChange -= OnUserStateChange;

            m_UserStateMachineSock.OnHandStateChange -= OnHandStateChange;

            m_BoardSock.OnTeleportRequest -= OnTeleportRequest;
            m_localUserSock.OnRequestTeleport -= OnTeleportRequest;
        }


        private void OnLevelStateChange(ELVLSTATE _OldState, ELVLSTATE _NewState)
        {
            switch (_NewState)
            {
                case ELVLSTATE.ONCONNECT_COACH:

                    m_UserStateMachineSock.SetPlayerRole(EUSERROLE.COACH);

                    break;

                case ELVLSTATE.ONCONNECT_PLAYER:

                    m_UserStateMachineSock.SetPlayerRole(EUSERROLE.PLAYER);

                    break;

                case ELVLSTATE.SETUP_USER:

                    m_localUserSock = m_NetManagerSock.LocalAvatar.GetComponent<USER_LocalUser>().UserSock;

                    break;

                case ELVLSTATE.SETUP_USERCOMPONENTS:

                    m_localUserSock.OnRequestTeleport += OnTeleportRequest;

                    m_UserStateMachineSock.OnRequestStateChange(EUSERSTATE.SETUP_COMPONENTS);
                    break;
            }
        }

        private void OnUserStateChange(EUSERSTATE _OldState, EUSERSTATE _NewState)
        {
            switch (_NewState)
            {
                case EUSERSTATE.SETUP_COMPONENTS:

                    if (m_localUserSock.UserRole.Equals(EUSERROLE.PLAYER))
                        m_StadiumPropSock.SetupPlayer();

                    break;
            }
        }

        private void OnHandStateChange(HandInfo _handInfo)
        {
            int interactor = (int)((float)_handInfo.InteractorState * 0.1);

            // Filter out only ray interactors
            if (interactor == 2)
            {
                // On pickup action
                if ((int)_handInfo.InteractorState % 10 == 1)
                {
                    //Debug.Log("#User_Manager#--------------------Rayinteractor state pickup");

                    // If already locked in place return
                    if (m_isLockedInPlace)
                        return;

                    m_isLockedInPlace = true;
                    m_Teleporter.enabled = false;
                    //m_Mover.enabled = false;
                }
                // On drop action
                else if ((int)_handInfo.InteractorState % 10 == 2)
                {
                    //Debug.Log("#User_Manager#--------------------Rayinteractor state drop");

                    // If already locked in place return
                    if (!m_isLockedInPlace)
                        return;

                    m_isLockedInPlace = false;
                    m_Teleporter.enabled = true;
                    //m_Mover.enabled = true;
                }
            }
        }

        private void OnTeleportRequest(Vector3 _position, Quaternion _rotation)
        {
            //Debug.Log("#Eureka#-------------------------|||" + _position);
            this.transform.position = _position;
            this.transform.rotation = _rotation;
        }
    }
}
