using UnityEngine;
using com.DU.CE.NET;

namespace com.DU.CE.USER
{
    [RequireComponent(typeof(USER_LocalUser))]
    public class USER_Player : MonoBehaviour
    {
        [SerializeField] private SOC_NetManager m_NetManagerSock = null;
        
        private USER_LocalUser m_localUser = null;
        private SOC_UserUIPlayer m_PlayerUISock = null;
        private SOC_User m_UserSock = null;

        private void Awake()
        {
            m_localUser = GetComponent<USER_LocalUser>();
            m_UserSock = m_localUser.UserSock;
            m_PlayerUISock = m_UserSock.UISock as SOC_UserUIPlayer;
        }

        private void OnEnable()
        {
            m_PlayerUISock.OnTeleportToCoach += TeleportCoach;
        }

        private void OnDisable()
        {
            m_PlayerUISock.OnTeleportToCoach -= TeleportCoach;
        }

        private void TeleportCoach()
        {
            Transform coach = m_NetManagerSock.Coach; 

            Debug.Log("#USER_Player#-------------------------Teleporting to coach " + coach.position);

            m_UserSock.RequestRigTeleport(coach.position, coach.rotation);
        }
    }
}