using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace com.DU.CE.USER
{
    public class USER_CustomRayInteractor : XRRayInteractor
    {
        [SerializeField] private EUSERHAND m_Hand;
        [SerializeField] private SOC_UserStateMachine m_UserStateMachineSock;

        public EUSERHAND Hand { get => m_Hand; }

        public float DefaultLineLength;
        public float HoveredLineLength;

        public Vector3 FieldPosition { get => GetCurrentFieldPoint(); }

        private const int m_FIELD_LAYER = 9;

        private XRInteractorLineVisual m_interactorLineVisual = null;

        protected override void Awake()
        {
            base.Awake();

            m_interactorLineVisual = GetComponent<XRInteractorLineVisual>();
            m_interactorLineVisual.lineLength = DefaultLineLength;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            m_UserStateMachineSock.OnHandStateChange += OnInteractionStateChange;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            m_UserStateMachineSock.OnHandStateChange -= OnInteractionStateChange;
        }

        private void OnInteractionStateChange(HandInfo _handInfo)
        {
            // Diable select when opposite hand is held and enable when deselected
            if (_handInfo.Hand == m_Hand)
                return;

            //Debug.Log("#USER_CustomRayInteractor#--------------------Opposite hand " + _handInfo.Hand);

            if (_handInfo.Hand == EUSERHAND.NONE)
            {
                Debug.Log("#USER_CustomRayInteractor#--------------Allow Select " + m_Hand);

                allowSelect = true;
                allowHover = true;
            }

            // If opposite hand
            int t = (int)_handInfo.Hand + (int)m_Hand;

            if (t == 0)
            {
                int action = (int)_handInfo.InteractorState % 10;

                // If held
                if (action == 1)
                {
                    Debug.Log("#USER_CustomRayInteractor#--------------Disallow Select " + m_Hand);
                    allowSelect = false;
                    allowHover = false;
                }
            }
            
        }

        protected override void OnHoverEntered(HoverEnterEventArgs args)
        {
            base.OnHoverEntered(args);
            m_interactorLineVisual.lineLength = HoveredLineLength;
        }


        protected override void OnHoverExited(HoverExitEventArgs args)
        {
            base.OnHoverExited(args);
            m_interactorLineVisual.lineLength = DefaultLineLength;
        }


        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);

            m_interactorLineVisual.enabled = false;

            m_UserStateMachineSock.RequestInteractionChange(EINTERACTIONSTATE.ONSELECT, m_Hand);
        }


        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);

            m_interactorLineVisual.enabled = true;

            m_UserStateMachineSock.RequestInteractionChange(EINTERACTIONSTATE.ONDESELECT, m_Hand);
        }


        private Vector3 GetCurrentFieldPoint()
        {
            TryGetCurrent3DRaycastHit(out RaycastHit hit);
            Collider temp = hit.collider;

            if (temp != null)
            {
                if (temp.gameObject.layer == m_FIELD_LAYER)
                    return hit.point;
            }

            return Vector3.zero;
        }
    }
}
