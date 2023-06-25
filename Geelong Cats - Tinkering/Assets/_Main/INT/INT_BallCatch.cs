using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using com.DU.CE.USER;

namespace com.DU.CE.AI
{
    public class INT_BallCatch : XRGrabInteractable
    {
        [SerializeField] private InputActionProperty m_rightHand;
        [SerializeField] private InputActionProperty m_leftHand;

        // private AI_Avatar m_manager;
        private USER_CustomRayInteractor m_interactor = null;
        public Collision checkCollisionRight;
        public Collision checkCollisionLeft;
        public bool isOnHover = false;
        public InputActionReference toggleReference = null;
        public InputActionReference RightGrab;
        public InputActionReference LeftGrab;
        public bool BallisOwned;
        public bool isHoveringRight = false;
        public bool isHoveringLeft = false;
        protected override void Awake()
        {
            base.Awake();
            toggleReference.action.started += GetBall;

         //   RightGrab.action.started += RightGrabPress;
          //  LeftGrab.action.started += LeftGrabPress;


        }


        private void OnDestroy()
        {
            toggleReference.action.started -= GetBall;

         //   RightGrab.action.started -= RightGrabPress;
         //   LeftGrab.action.started -= LeftGrabPress;
        }

       /* private void RightGrabPress(InputAction.CallbackContext context)
        {
            Debug.Log("ENTERED");
           
            if (isHoveringRight)
            {
                BallisOwned=true;
                Debug.Log("PRESS ENTERED");
            }

        }
        private void LeftGrabPress(InputAction.CallbackContext context)
        {
            Debug.Log("ENTERED");

            if (isHoveringLeft)
            {

                BallisOwned = true;
                Debug.Log("PRESS ENTERED");
            }
        }
       */
    

        private void OnCollisionEnter(Collision collision)
        {

            if (collision.gameObject.tag == "RightReticule")
            {
                Debug.Log("Collision right");
                checkCollisionRight = collision;
                isHoveringRight = true;
            }
            if (collision.gameObject.tag == "LeftReticule")
            {
                Debug.Log("Collision left");
                checkCollisionRight = collision;
                isHoveringLeft = true;
            }
        }

        /*  private void OnCollisionExit(Collision collision)
          {

              if (collision.gameObject.tag == "RightReticule")
              {
                  Debug.Log("RightReticuleCOllision");
                  checkCollisionRight = collision;
                  isHoveringRight = false;
              }
              if (collision.gameObject.tag == "LeftReticule")
              {
                  Debug.Log("LefttReticuleCOllision");
                  checkCollisionRight = collision;
                  isHoveringLeft = false;
              }
          }*/
        private void GetBall(InputAction.CallbackContext context)
        { 
            if (isOnHover)
            {

                // PARENT THE BALL

                Debug.Log("Grabbing ball");
            }

        }
        #region XR Callbacks


       
        protected override void OnHoverEntered(HoverEnterEventArgs args)
        {
            //  m_manager.OnHoverChanged(true);
            Debug.Log("Ball Hover");
            isOnHover = true;
        }

        protected override void OnHoverExited(HoverExitEventArgs args)
        {
            //  m_manager.OnHoverChanged(false);
            isOnHover = false;
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {

            base.OnSelectEntered(args);
            Debug.Log("Ball Grab pressed");


            m_interactor = args.interactor as USER_CustomRayInteractor;

            if (m_interactor.Hand.Equals(EUSERHAND.LEFT))
            {
                isHoveringLeft = true;
                Debug.Log("On left hand grab"); 
                m_leftHand.action.performed += OnLeftHandGrab;
            }
            else
            {
                isHoveringRight = true;
                Debug.Log("On right hand grab"); m_rightHand.action.performed += OnRigthHandGrab;
               
            }
        }

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);

            if (m_interactor.Hand.Equals(EUSERHAND.LEFT))
            {
         
                m_rightHand.action.performed -= OnRigthHandGrab;
            }
            else
            {
              
                m_leftHand.action.performed -= OnLeftHandGrab;
            }
            m_interactor = null;
        }

        private void OnRigthHandGrab(InputAction.CallbackContext obj)
        {
            throw new NotImplementedException();


        }

        private void OnLeftHandGrab(InputAction.CallbackContext obj)
        {
            throw new NotImplementedException();
        }
        #endregion

    }

}