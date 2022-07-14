using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

using com.DU.CE.USER;
using System;

namespace com.DU.CE.AI
{

    public class AI_CustomXRInteractable : XRGrabInteractable
    {
        [SerializeField] private InputActionProperty m_rightPathPlace;
        [SerializeField] private InputActionProperty m_leftPathPlace;

        private AI_Avatar m_manager;
        private USER_CustomRayInteractor m_interactor = null;
        public GameObject ActionUI;
        //trigger code
        public InputActionReference toggleReference = null;
        protected override void Awake()
        {
            base.Awake();
            toggleReference.action.started += SelectAI;

            m_manager = GetComponentInParent<AI_Avatar>();
        }

        private void OnDestroy()
        {
            toggleReference.action.started -= SelectAI;
        }

        private void SelectAI(InputAction.CallbackContext context)
        {
           // context.ReadValueAsButton
            bool isActive = !ActionUI.gameObject.activeSelf;
             
            ActionUI.gameObject.SetActive(isActive);
            Debug.Log("Trigger is presed");
        }
        #region XR Callbacks


       
        
        
        protected override void OnHoverEntered(HoverEnterEventArgs args)
        {
            m_manager.OnHoverChanged(true);
        }

        protected override void OnHoverExited(HoverExitEventArgs args)
        {
            m_manager.OnHoverChanged(false);
        }

        

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);
            Debug.Log("Grabpressed");
            m_manager.M_NCModel.isSelected = true;

            m_interactor = args.interactor as USER_CustomRayInteractor;

            if (m_interactor.Hand.Equals(EUSERHAND.LEFT))
            {
                m_rightPathPlace.action.performed += OnRigthPathPlaceCrumb;
            }
            else
            {
                m_leftPathPlace.action.performed += OnLeftPlacePathCrumb;
            }
        }

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);

            m_manager.M_NCModel.isSelected = false;

            if (m_interactor.Hand.Equals(EUSERHAND.LEFT) && args.interactor.Equals(m_interactor))
            {
                m_rightPathPlace.action.performed -= OnRigthPathPlaceCrumb;
            }
            else if(m_interactor.Hand.Equals(EUSERHAND.RIGHT) && args.interactor.Equals(m_interactor))
            {
                m_leftPathPlace.action.performed -= OnLeftPlacePathCrumb;
            }

            m_interactor = null;
        }

        #endregion

        private void OnLeftPlacePathCrumb(InputAction.CallbackContext obj)
        {
            throw new NotImplementedException();
        }

        private void OnRigthPathPlaceCrumb(InputAction.CallbackContext obj)
        {
            throw new NotImplementedException();
        }
    }
}