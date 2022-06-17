using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using EPOOutline;

namespace com.DU.CE.INT
{
    public class INT_CameraPin : TeleportationAnchor
    {
        public float OriginOffsetX = 0.00515f;
        public float OriginOffsetY = 0.00585f;
        public float OriginOffsetZ = 0.005f;
        
        //[SerializeField] private GameObject m_rayCastPoint = null;

        private XRBaseInteractor m_xrInteractor = null;
        private Vector3 m_controllePreviousPosition = Vector3.zero;
        private Vector3 m_fieldPoint = Vector3.zero;

        private INT_BoardPins m_pinManager = null;
        private INT_IBoardLinkedPin m_pin;

        private Outlinable m_outlineComponent = null;
        private SphereCollider m_sphereCollider = null;
        private MeshRenderer[] m_meshRenders = null;


        protected override void Awake()
        {
            base.Awake();

            // Get all references
            m_pin = GetComponent<INT_IBoardLinkedPin>();

            m_outlineComponent = GetComponent<Outlinable>();
            m_sphereCollider = GetComponentInChildren<SphereCollider>();
            m_meshRenders = GetComponentsInChildren<MeshRenderer>();
        }


        #region XRInteractable Callbacks

        /// <summary>
        /// Things to do when the object is held by the XR interactor
        /// </summary>
        /// <param name="updatePhase"></param>
        public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
        {
            if (!m_xrInteractor)
                return;

            m_pin.UpdatePosWhenHeld();
            //m_pin.UpdateRotWhenHeld();
        }

        protected override void OnHoverEntered(HoverEnterEventArgs args)
        {
            m_outlineComponent.enabled = true;
        }

        protected override void OnHoverExited(HoverExitEventArgs args)
        {
            m_outlineComponent.enabled = false;
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);

            //m_posMarkButton.OnButtonDown += OnMarkButtonDown;
            //m_posMarkButton.OnButtonUp += OnMarkButtonUp;

            // Change outline to selected colour
            m_outlineComponent.OutlineColor = Color.green;

            // Set reference to XR Ray Interactor
            m_xrInteractor = args.interactor;

            Vector3 controllerPosition = m_xrInteractor.transform.position;
            m_controllePreviousPosition = transform.root.InverseTransformPoint(controllerPosition);
        }


        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);

            //m_posMarkButton.OnButtonDown -= OnMarkButtonDown;
            //m_posMarkButton.OnButtonUp -= OnMarkButtonUp;

            // Change outline to highlight colour
            m_outlineComponent.OutlineColor = Color.yellow;

            m_controllePreviousPosition = Vector2.zero;

            // Remove reference of interactor
            m_xrInteractor = null;

            // Update field point
            //m_pinManager.RaycastToField(m_rayCastPoint.transform, out m_fieldPoint);

            //Debug.Log("--------------------" + m_fieldPoint);
            //m_linkedObject.SetRelativeYRotation(transform.localRotation.y);
        }

        #endregion


        #region IPin interface methods

        public void InitializePin(INT_ILinkedPinObject _linkedObject, INT_BoardPins _manager)
        {
            m_pinManager = _manager;
        }

        public void SwitchPin(bool val)
        {
            m_sphereCollider.enabled = val;
            for (int i = 0; i < m_meshRenders.Length; i++)
            {
                m_meshRenders[i].enabled = val;
            }
        }



        //void PL_IBoardPin.UpdatePosWhenHeld()
        //{
        //    // Get controller position
        //    Vector3 controllerCurrentPosition = m_xrInteractor.transform.localPosition;

        //    //Debug.Log("#CamerPin# ---------------------------\n" + controllerCurrentPosition);

        //    Vector3 rateOfChangeOfPosition = controllerCurrentPosition - m_controllePreviousPosition;
        //    m_controllePreviousPosition = controllerCurrentPosition;

        //    Vector3 newPosition = (transform.localPosition + (rateOfChangeOfPosition));

        //    //// Change the origin to the pin's space
        //    //Vector3 newRelativeHandPosition =
        //    //    transform.root.InverseTransformDirection(controllerCurrentPosition);

        //    //// Calculate the difference 
        //    //Vector3 handDifference = (newRelativeHandPosition - m_controllePreviousPosition);
        //    //// Set the new position as the previous position
        //    //m_controllePreviousPosition = newRelativeHandPosition;

        //    //Debug.Log("#CamerPin# ---------------------------\n" + newPosition);

        //    transform.localPosition = Vector3.MoveTowards(transform.localPosition, newPosition, m_lerpSmoothening * 100.0f * Time.deltaTime);

        //    //Vector3 clampedPosition = new Vector3(
        //    //    Mathf.Clamp(ZYswitchedPosition.x, -0.42f, 0.42f),
        //    //    Mathf.Clamp(ZYswitchedPosition.y, -0.55f, 0.55f),
        //    //    ZYswitchedPosition.z);
        //}

        //void PL_IBoardPin.UpdateRotWhenHeld()
        //{
        //    float newHandZRotation = m_xrInteractor.transform.localRotation.z;

        //    //Debug.Log("#BoardPin# ---------------------------\n" + newHandZRotation);

        //    transform.Rotate(0f, 0f, newHandZRotation / 0.25f);
        //}

        #endregion
    }
}
