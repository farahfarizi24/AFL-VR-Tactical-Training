using UnityEngine;

namespace com.DU.CE.USER
{
    public abstract class USER_AComponent : MonoBehaviour
    {
        [SerializeField] protected SOC_UserStateMachine p_StateMachineSock = null;
        private bool m_isInitialized = false;

        private EUSERROLE m_userRole;
        protected EUSERROLE p_role { get => m_userRole; }

        protected virtual void OnUserStateChange(EUSERSTATE _oldState, EUSERSTATE _newState)
        {
            if (_newState.Equals(EUSERSTATE.SETUP_COMPONENTS))
            {
                Initialize(p_StateMachineSock.Role);
            }
        }


        protected virtual void Initialize(EUSERROLE _role)
        {
            if (m_isInitialized)
                return;

            m_userRole = _role;

            m_isInitialized = true;
        }
    }
}
