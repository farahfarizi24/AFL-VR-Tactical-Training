using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

using com.DU.CE.USER;

namespace com.DU.CE.INT
{
    public class INT_Field : XRBaseInteractable
    {
        [SerializeField] private InputActionProperty m_RightPlacementInputAction;
        [SerializeField] private InputActionProperty m_LeftPlacementInputAction;
        [SerializeField] private SOC_UserUICoach m_CoachUISock = null;

        private USER_CustomRayInteractor m_tempInteractor = null;
        private USER_CustomRayInteractor m_rightRayInteractor = null;
        private USER_CustomRayInteractor m_leftRayInteractor = null;

        #region XRInteractable Callbacks

        protected override void OnHoverEntered(HoverEnterEventArgs args)
        {
            m_tempInteractor = args.interactor as USER_CustomRayInteractor;

            if (m_tempInteractor.Hand.Equals(EUSERHAND.RIGHT))
            {
                m_rightRayInteractor = m_tempInteractor;
                //m_RightPlacementInputAction.action.RemoveBindingOverride(0);
                m_RightPlacementInputAction.action.performed += OnPlacementActionRight;
            }
            else
            {
                m_leftRayInteractor = m_tempInteractor;
                //m_LeftPlacementInputAction.action.RemoveBindingOverride(0);
                m_LeftPlacementInputAction.action.performed += OnPlacementActionLeft;
            }

            m_tempInteractor = null;
        }


        protected override void OnHoverExited(HoverExitEventArgs args)
        {
            m_tempInteractor = args.interactor as USER_CustomRayInteractor;

            if (m_tempInteractor.Hand.Equals(EUSERHAND.RIGHT))
            {
                m_rightRayInteractor = null;
                m_RightPlacementInputAction.action.performed -= OnPlacementActionRight;
            }
            else
            {
                m_leftRayInteractor = null;
                m_LeftPlacementInputAction.action.performed -= OnPlacementActionLeft;
            }

            m_tempInteractor = null;
        }


        private void OnPlacementActionRight(InputAction.CallbackContext obj)
        {
            if (!m_rightRayInteractor)
                return;

            RaycastHit hit;
            m_rightRayInteractor.TryGetCurrent3DRaycastHit(out hit);

            // Show placement UI
            m_CoachUISock.ToggleFieldLocUI(true, hit.point);
        }


        private void OnPlacementActionLeft(InputAction.CallbackContext obj)
        {
            if (!m_leftRayInteractor)
                return;

            RaycastHit hit;
            m_leftRayInteractor.TryGetCurrent3DRaycastHit(out hit);

            // Show placement UI
            m_CoachUISock.ToggleFieldLocUI(true, hit.point);
        }


        #endregion
    }
}