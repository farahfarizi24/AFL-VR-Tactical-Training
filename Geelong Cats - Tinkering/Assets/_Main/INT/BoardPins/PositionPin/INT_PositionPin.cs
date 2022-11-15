using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

using TMPro;
using EPOOutline;

namespace com.DU.CE.INT
{
    public class INT_PositionPin : XRBaseInteractable, INT_IBoardLinkedPin
    {

        [SerializeField] private SOC_FieldBoard m_BoardSock = null;
        [Space]
        [SerializeField] private GameObject m_pinHead = null;
        [SerializeField] private GameObject m_pinMesh = null;
        [SerializeField] private GameObject m_pinArrow = null;
        public GameObject PinBody = null;
        [Space]
        [SerializeField] private TextMeshPro m_tmpTeamNumber = null;
        [SerializeField] private MeshRenderer[] m_meshRenders = null;

        // Reference to IBoardPinObject
        private INT_IBoardLinkedPin m_pin { get => this; }
        private INT_ILinkedPinObject m_linkedObject = null;

        private bool m_islinkedObjectActive = false;
        private bool m_isBoardOpen = false;
        private bool isRotating = false;
        private bool ActionBeingPerformed = false;
        // Point on the field the pin points to
        private Vector3 m_fieldPoint = Vector3.zero;
        private XRBaseInteractor m_xrInteractor = null;
        private Vector3 m_previousHandPosition = Vector3.zero;

        // Reference to components
        private Outlinable m_outlineComponent = null;

        //For Mouse drag
        private Vector3 screenPoint;
        private Vector3 offset;
        private GameObject CameraObject;
        public ScenarioCreation_Function ScenarioScript = null;
        public GameObject ScenarioS;
        protected override void Awake()
        {
            base.Awake();

            // Get all references
            m_outlineComponent = GetComponent<Outlinable>();

            m_pin.SwitchPin(false);

            m_tmpTeamNumber.enabled = false;
            m_pinArrow.SetActive(false);
        
          //  ScenarioScript = GameObject.FindGameObjectWithTag("CoachUI").GetComponent<ScenarioCreation_Function>();
            // m_linkedObject.SetRelativeYRotation(m_pinArrow.transform.localRotation.y);

        }

        
        #region XRBaseInteractable Callbacks

        protected override void OnHoverEntered(HoverEnterEventArgs args)
        {
          
          //  m_pinArrow.transform.localRotation = Quaternion.Euler(0f, m_linkedObject.GetRelativeYRotation(), 0f);
            m_outlineComponent.enabled = true;
            m_tmpTeamNumber.enabled = true;
            m_pinArrow.SetActive(true);
        }

        private void OnMouseOver()
        {
            ScenarioS = GameObject.FindGameObjectWithTag("CoachUI");
            m_pinArrow.transform.localEulerAngles = new Vector3(0.0f, m_linkedObject.GetRelativeYRotation(), 0.0f);
            m_outlineComponent.enabled = true;
            m_tmpTeamNumber.enabled = true;
            m_pinArrow.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                
                    m_outlineComponent.OutlineColor = Color.green;
                    m_pinHead.transform.localPosition += (0.015f * Vector3.up);
                    m_pinArrow.SetActive(true);
                    isRotating = true;
               
                //activate it for rotation
            
            }
            }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Space)&& isRotating)
            {
                ActionBeingPerformed = true;
                m_outlineComponent.OutlineColor = Color.green;
                m_pinArrow.SetActive(true);

                // Get Y rotation of the linked object
                float rotY = m_linkedObject.GetRelativeTransform().rotation.y;
                // Apply the rotation to pin
                m_pinArrow.transform.localRotation = Quaternion.Euler(0f, rotY, 0f);
                //get camera object
                GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
                Vector2 positionOnscreen = camera.GetComponent<Camera>().WorldToViewportPoint(gameObject.transform.position);
                 Vector2 mouseOnScreen = (Vector2)camera.GetComponent<Camera>().ScreenToViewportPoint(Input.mousePosition);

                //get angle between two points
                float angle = AngleBetweenTwoPoints(positionOnscreen, mouseOnScreen);

               

                m_pinArrow.transform.localRotation = Quaternion.Euler(0f, angle/0.25f, 0f);
               

                //turn the pin according to the mouse


            }

            float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
            {
                return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
            }

            //this one is where the rotation is updated
            if (Input.GetKeyUp(KeyCode.Space)&& isRotating)
            {
                ActionBeingPerformed = false;
                isRotating = false;
                // Change outline to highlight colour
                m_outlineComponent.OutlineColor = Color.yellow;
                m_pinHead.transform.localPosition -= (0.015f * Vector3.up);
                m_pinArrow.SetActive(false);

                m_previousHandPosition = Vector2.zero;
                // float newHandZRotation = -m_xrInteractor.transform.localRotation.z;

                //  m_pinArrow.transform.Rotate(0f, newHandZRotation / 0.25f, 0f);
                // Update field point
                if (!m_BoardSock.GetBoardToFieldPosition(m_pinHead.transform, out m_fieldPoint))
                {
                    Debug.LogError("#PositionPin#--------------RaycastFailed");
                    return;
                }
                //Debug.Log("--------------------" + m_fieldPoint);

                m_linkedObject.SetNavAgentDestination(m_fieldPoint);
             
                m_linkedObject.SetRelativeYRotation(m_pinArrow.transform.localEulerAngles.y);
                Debug.Log("New Y relative rotation" + m_linkedObject.GetRelativeYRotation());
               

                // m_pinArrow.transform.localEulerAngles = new Vector3(0.0f, m_linkedObject.GetRelativeYRotation(), 0.0f);

            }
        }
      
        private void OnMouseExit()
        {

            m_outlineComponent.enabled = false;
            m_tmpTeamNumber.enabled = false;
            // m_pinArrow.SetActive(false);
            if (!ActionBeingPerformed)
            {
                m_pinArrow.SetActive(false);
            }
        }


        private void OnMouseDown()
        {
            ScenarioScript = GameObject.FindGameObjectWithTag("CoachUI").GetComponent<ScenarioCreation_Function>();
            if (ScenarioScript.BallTargetting == true)
            {
                return;

            }
            else
            {
                m_outlineComponent.OutlineColor = Color.green;
                m_pinHead.transform.localPosition += (0.015f * Vector3.up);
                m_pinArrow.SetActive(true);
                //get camera
                GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
                screenPoint = camera.GetComponent<Camera>().WorldToScreenPoint(gameObject.transform.position);
                offset = gameObject.transform.position - camera.GetComponent<Camera>().ScreenToWorldPoint
                    (new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            }
           
           

        }

        private void OnMouseDrag()
        {
            if (ScenarioScript.BallTargetting == true)
            {
                return;

            }
            else
            {
                ActionBeingPerformed = true;
                Vector3 currentscreenpoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
                GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
                Vector3 currentPositon = camera.GetComponent<Camera>().ScreenToWorldPoint(currentscreenpoint) + offset;
                transform.position = currentPositon;
            }

          
        }

        private void OnMouseUp()
        { // Change outline to highlight colour
            if (ScenarioScript.BallTargetting == true)
            {
                m_linkedObject.SetBallReceiver(true);
                ScenarioScript.SetBallTarget();
                 
            }
            else
            {
                m_outlineComponent.OutlineColor = Color.yellow;
                m_pinHead.transform.localPosition -= (0.015f * Vector3.up);
                m_pinArrow.SetActive(false);
                ActionBeingPerformed = false;
                m_previousHandPosition = Vector2.zero;

                // Remove reference of interactor
                m_xrInteractor = null;

                // Update field point
                if (!m_BoardSock.GetBoardToFieldPosition(m_pinHead.transform, out m_fieldPoint))
                {
                    Debug.LogError("#PositionPin#--------------RaycastFailed");
                    return;
                }
                //Debug.Log("--------------------" + m_fieldPoint);

                m_linkedObject.SetNavAgentDestination(m_fieldPoint);
                m_linkedObject.SetRelativeYRotation(m_pinArrow.transform.localRotation.y);
            }
        }
        protected override void OnHoverExited(HoverExitEventArgs args)
        {
            m_outlineComponent.enabled = false;
            m_tmpTeamNumber.enabled = false;
            m_pinArrow.SetActive(false);
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            //m_boardPins.M_IsPicked = true;

            // Change outline to selected colour
            m_outlineComponent.OutlineColor = Color.green;
            m_pinHead.transform.localPosition += (0.015f * Vector3.up);

            // Set reference to XR Ray Interactor
            m_xrInteractor = args.interactor;

            Vector3 controllerPosition = m_xrInteractor.transform.position;
            m_previousHandPosition = transform.root.InverseTransformPoint(controllerPosition);
        }


        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            // Change outline to highlight colour
            m_outlineComponent.OutlineColor = Color.yellow;
            m_pinHead.transform.localPosition -= (0.015f * Vector3.up);
            m_pinArrow.SetActive(false);

            m_previousHandPosition = Vector2.zero;

            // Remove reference of interactor
            m_xrInteractor = null;

            // Update field point
            if(!m_BoardSock.GetBoardToFieldPosition(m_pinHead.transform, out m_fieldPoint))
            {
                Debug.LogError("#PositionPin#--------------RaycastFailed");
                return;
            }
            //Debug.Log("--------------------" + m_fieldPoint);

            m_linkedObject.SetNavAgentDestination(m_fieldPoint);
            m_linkedObject.SetRelativeYRotation(m_pinArrow.transform.localRotation.y);

        // m_boardPins.M_IsPicked = false;
        }


        /// <summary>
        /// Things to do when the object is held by the XR interactor
        /// </summary>
        /// <param name="updatePhase"></param>
        public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
        {
            if (!m_xrInteractor)
                return;

            m_pinArrow.SetActive(true);
            //Debug.Log("#BoardPin# ---------------------------");

            m_pin.UpdatePosWhenHeld();
            m_pin.UpdateRotWhenHeld();
        }

        #endregion



        #region Board Pin Interface Methods


        void INT_IBaseBoardPin.SwitchPin(bool val)
        {
            m_isBoardOpen = val;

            colliders[0].enabled = m_islinkedObjectActive && val;
            for (int i = 0; i < m_meshRenders.Length; i++)
            {
                m_meshRenders[i].enabled = m_islinkedObjectActive && val;
            }
        }

        void INT_IBoardLinkedPin.SetObjectStatus(bool isActivated)
        {
            m_islinkedObjectActive = isActivated;

            if (!m_isBoardOpen)
                return;

            m_pin.UpdatePinPosition();
            m_pin.SwitchPin(true);
        }

        void INT_IBoardLinkedPin.LinkObject(Transform linkedObject)
        {
            m_linkedObject = linkedObject.GetComponent<INT_ILinkedPinObject>();
            m_linkedObject.LinkPin(this);
        }

        void INT_IBoardLinkedPin.SetPinColour(Color32 color)
        {
            var Body = PinBody.GetComponent<MeshRenderer>();
            Body.material.color = color;
           // PinBody.GetComponent<MeshRenderer>().SetColor("_Color", color);
        }

        void INT_IBoardLinkedPin.UpdateForLoad(Vector3 _destinationPosition, Vector3 destinationRotation)
        {
            Vector3 aiPosition = _destinationPosition;
            // Change the XZ position of the AI to the Xz position of the pin
            Vector3 tempVec = new Vector3(aiPosition.x, 0f, aiPosition.z);

            transform.localPosition = new Vector3(tempVec.x * m_BoardSock.OriginOffsetX, tempVec.y, tempVec.z * m_BoardSock.OriginOffsetZ);


            // Get Y rotation of the linked object
            float rotY = destinationRotation.y;
            // Apply the rotation to pin
            m_pinArrow.transform.localRotation = Quaternion.Euler(0f, rotY, 0f);

          //  transform.localPosition = _destinationPosition;
           // m_pinArrow.transform.localRotation = Quaternion.Euler(destinationRotation);
        }

        void INT_IBoardLinkedPin.UpdatePinPosition()
        {
            // Get position of the linked object
            Vector3 aiPosition = m_linkedObject.GetRelativeTransform().position;
            // Change the XZ position of the AI to the Xz position of the pin
            Vector3 tempVec = new Vector3(aiPosition.x, 0f, aiPosition.z);

            // Apply position with offset
            transform.localPosition = new Vector3(tempVec.x * m_BoardSock.OriginOffsetX, tempVec.y, tempVec.z * m_BoardSock.OriginOffsetZ);

            // Get Y rotation of the linked object
            float rotY = m_linkedObject.GetRelativeTransform().rotation.y;
            // Apply the rotation to pin
            m_pinArrow.transform.localRotation = Quaternion.Euler(0f, rotY, 0f);
        }


        void INT_IBaseBoardPin.UpdatePosWhenHeld()
        {
            // Get controller position
            Vector3 controllerPosition = m_xrInteractor.transform.position;
            // Change the origin to the pin's space
            Vector3 newRelativeHandPosition =
                transform.root.InverseTransformPoint(controllerPosition);

            // Calculate the difference 
            Vector3 handDifference = (m_previousHandPosition - newRelativeHandPosition);
            // Set the new position as the previous position
            m_previousHandPosition = newRelativeHandPosition;

            Vector3 newPosition = (transform.localPosition - (handDifference * m_BoardSock.MoveMultiplier));
            newPosition = new Vector3(
                Mathf.Clamp(newPosition.x, -0.42f, 0.42f),
                0f,
                Mathf.Clamp(newPosition.z, -0.55f, 0.55f));

            transform.localPosition = newPosition;
        }

      
        void INT_IBaseBoardPin.UpdateRotWhenHeld()
        {
            float newHandZRotation = -m_xrInteractor.transform.localRotation.z;

            m_pinArrow.transform.Rotate(0f, newHandZRotation / 0.25f, 0f);
        }

        public void SetupPin(ETEAM team, int playerNo)
        {
            // Set the team number on the pin
            m_tmpTeamNumber.text = playerNo.ToString();

            // Setup material according to "team" info from the linked object
            m_pinMesh.GetComponent<MeshRenderer>().material = (team == ETEAM.HOME) ?
                m_BoardSock.HomeTeamMaterial : m_BoardSock.AwayTeamMaterial;
        }

        #endregion
    }
}
