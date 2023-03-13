using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

using EPOOutline;

namespace com.DU.CE.INT
{
    public class INT_MarkerPin : XRBaseInteractable, INT_IBaseBoardPin
    {
        [SerializeField] private GameObject m_pin;
        [SerializeField] private Transform m_raycastPoint = null;
        [SerializeField] private SOC_FieldBoard m_BoardSock = null;
        [SerializeField] private MeshRenderer[] m_MeshRenderers = null;
        public Vector3 m_fieldPoint  = Vector3.zero;
        //public GameObject MarkerpinObject;
        // Reference to components
        private Outlinable m_outlineComponent = null;

        //For Mouse drag
        private Vector3 screenPoint;
        private Vector3 offset;


      
        private void Start()
        {
            m_outlineComponent = GetComponent<Outlinable>();
            m_outlineComponent.enabled = false;
            m_fieldPoint = Vector3.zero;

           // GameObject RaycastPoint = GameObject.FindGameObjectWithTag("MarkerPin");
           // m_fieldPoint = RaycastPoint.transform.position;

        }



        #region Mouse Interaction

        private void OnMouseDown()
        {
            //get camera
            GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
            screenPoint = camera.GetComponent<Camera>().WorldToScreenPoint(gameObject.transform.position);
            offset = gameObject.transform.position - camera.GetComponent<Camera>().ScreenToWorldPoint
                (new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));


            Debug.Log("Mouse click, field point" + m_fieldPoint);
        }
            private void OnMouseOver()
        {
            if (m_outlineComponent.enabled == false)
            {

                m_outlineComponent.enabled = true;
                m_outlineComponent.OutlineColor = Color.green;
            }

           
            //int_boardpins UpdatePosition()
            Debug.Log("Mouse over MarkerPins");

        }
        private void OnMouseExit()
        {
            m_outlineComponent.enabled = false;
            //int_boardpins UpdatePosition()
        }

        
        private void OnMouseDrag()
        {
            Vector3 currentscreenpoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
            Vector3 currentPositon = camera.GetComponent<Camera>().ScreenToWorldPoint(currentscreenpoint) + offset;
            transform.position = currentPositon;

            Debug.Log("Mouse drag markerpins");
        }
        private void OnMouseUp()
        {


            if (!m_BoardSock.GetBoardToFieldPosition(m_pin.transform, out m_fieldPoint))
            {
                Debug.LogError("#Marker Pin#--------------RaycastFailed");


                return;
            }

            //So transform M_Fieldpoint

            //   m_BoardSock.OnUIMarkerChanged(m_fieldPoint);
            //   m_BoardSock.AddMarkerOnNetwork(m_fieldPoint.transfom.position);

            Debug.Log("Marker position updated" + m_fieldPoint);
            //Debug.Log("--------------------" + m_fieldPoint);

        }

        #endregion


        #region XRBaseInteractable Callbacks




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
            m_BoardSock.RequestTeleport(m_raycastPoint);
        }

        void INT_IBaseBoardPin.SwitchPin(bool toggle)
        {
            for (int i = 0; i < m_MeshRenderers.Length; i++)
                m_MeshRenderers[i].enabled = toggle;

            colliders[0].enabled = toggle;
        }

        void INT_IBaseBoardPin.UpdatePosWhenHeld()
        {
            throw new System.NotImplementedException();
        }

        void INT_IBaseBoardPin.UpdateRotWhenHeld()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
