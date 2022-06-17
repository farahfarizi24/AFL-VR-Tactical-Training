using UnityEngine;

using Autohand;

namespace com.DU.CE.USER
{
    [RequireComponent(typeof(Hand))]
    public class USER_HandManager : MonoBehaviour
    {
        [SerializeField] private SOC_UserStateMachine m_UserStateMachine = null;

        private EUSERHAND m_hand;
        private Hand m_AutoHand = null;

        private void Awake()
        {
            m_AutoHand = GetComponent<Hand>();

            m_hand = (m_AutoHand.left) ? EUSERHAND.LEFT : EUSERHAND.RIGHT;
        }

        private void OnEnable()
        {
            m_AutoHand.OnGrabbed += OnAutoHandGrab;
            m_AutoHand.OnReleased += OnAutoHandReleased;
        }

        private void OnDisable()
        {
            m_AutoHand.OnGrabbed -= OnAutoHandGrab;
            m_AutoHand.OnReleased -= OnAutoHandReleased;
        }

        #region AUTOHAND CALLBACKS---------------------

        private void OnAutoHandGrab(Hand hand, Grabbable grabbable)
        {
            m_UserStateMachine.RequestInteractionChange(EINTERACTIONSTATE.ONGRAB, m_hand);
        }

        private void OnAutoHandReleased(Hand hand, Grabbable grabbable)
        {
            m_UserStateMachine.RequestInteractionChange(EINTERACTIONSTATE.ONRELEASE, m_hand);
        }

        #endregion---------------------------------------
    }
}