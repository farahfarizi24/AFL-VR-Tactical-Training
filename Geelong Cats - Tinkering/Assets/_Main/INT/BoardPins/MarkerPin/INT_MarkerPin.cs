using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

using EPOOutline;

namespace com.DU.CE.INT
{
    public class INT_MarkerPin : XRBaseInteractable, INT_IBaseBoardPin
    {
        [SerializeField] private Transform m_raycastPoint = null;
        [SerializeField] private SOC_FieldBoard m_BoardSock = null;
        [SerializeField] private MeshRenderer[] m_MeshRenderers = null;

        // Reference to components
        private Outlinable m_outlineComponent = null;

        private void Start()
        {
            m_outlineComponent = GetComponent<Outlinable>();
            m_outlineComponent.enabled = false;
        }


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
