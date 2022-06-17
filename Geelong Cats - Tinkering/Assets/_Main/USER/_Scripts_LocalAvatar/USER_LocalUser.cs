using UnityEngine;
using RootMotion.FinalIK;

using TMPro;

namespace com.DU.CE.USER
{
    public class USER_LocalUser : MonoBehaviour
    {
        public EUSERROLE Role { get => m_UserSock.UserRole; }

        [SerializeField] private SOC_User m_UserSock = null;
        public SOC_User UserSock { get => m_UserSock; }

        [SerializeField] private VRIK m_IKResolver = null;
        [SerializeField] private TextMeshPro m_NameTextField = null;

        private void OnEnable()
        {
            m_UserSock.UISock.OnUIRescaleRig += RescaleIK;
        }

        private void OnDisable()
        {
            m_UserSock.UISock.OnUIRescaleRig -= RescaleIK;
        }

        internal void RescaleIK()
        {
            Debug.Log("#USER_LocalUser#-------------------------RescaleIK");

            //Compare the height of the head target to the height of the head bone, multiply scale by that value.
            float sizeF = (m_IKResolver.solver.spine.headTarget.position.y - m_IKResolver.references.root.position.y) / 
                (m_IKResolver.references.head.position.y - m_IKResolver.references.root.position.y);

            m_IKResolver.references.root.localScale *= sizeF * m_UserSock.m_IKScaleMLP;
        }

        internal void UpdatePlayerName(string name)
        {
            m_NameTextField.text = name;
        }
    }
}