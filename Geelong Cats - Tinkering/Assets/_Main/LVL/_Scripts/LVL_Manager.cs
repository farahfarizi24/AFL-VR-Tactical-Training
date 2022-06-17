using UnityEngine;

using com.DU.CE.NET;

namespace com.DU.CE.LVL
{
    public class LVL_Manager : MonoBehaviour
    {
        [Header("SOCs")]
        [SerializeField] private SOC_LvlStateMachine m_StateMachineSock = null;
        [SerializeField] private SOC_NetManager m_NetManagerSock = null;
        [SerializeField] private SOC_StadiumProperties m_StadiumPropertiesSock = null;

        #region Monobehaviour Callbacks

        private void OnEnable()
        {
            m_StateMachineSock.OnStateChange += HandleStateChange;
        }

        private void OnDisable()
        {
            m_StateMachineSock.OnStateChange -= HandleStateChange;
        }

        #endregion


        private void HandleStateChange(ELVLSTATE oldState, ELVLSTATE newState)
        {
            switch (newState)
            {
                case ELVLSTATE.SETUP_USER:

                    // Switch to role setup according to player input
                    SetupUser();

                    break;

                case ELVLSTATE.SETUP_USERCOMPONENTS:

                    // Spawn field
                    m_StadiumPropertiesSock.SetupFieldLines(EUSERROLE.COACH);

                    break;
            }
        }

        private void SetupUser()
        {
            if(m_NetManagerSock.LocalAvatar.GetComponent<NET_LocalAvatar>().UserRole == EUSERROLE.COACH)
            {
                m_StateMachineSock.OnRequestStateChange(ELVLSTATE.ONCONNECT_COACH);
            }
            else
            {
                m_StateMachineSock.OnRequestStateChange(ELVLSTATE.ONCONNECT_PLAYER);
            }
        }
    }
}