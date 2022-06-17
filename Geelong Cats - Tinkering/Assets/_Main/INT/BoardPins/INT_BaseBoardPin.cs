using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

using EPOOutline;
using TMPro;

namespace com.DU.CE.INT
{
    public abstract class INT_BaseBoardPin : XRBaseInteractable, INT_IBaseBoardPin
    {
        [SerializeField] protected SOC_FieldBoard m_BoardSock = null;
        [Space]
        [SerializeField] private MeshRenderer[] m_meshRenders = null;
        [SerializeField] private SphereCollider m_collider = null;
        [SerializeField] private TextMeshPro m_hoverText = null;

        protected XRBaseInteractable p_xrInteractor;

        private Outlinable m_outline = null;

        void INT_IBaseBoardPin.SwitchPin(bool val)
        {
            TogglePin(val);
        }

        void INT_IBaseBoardPin.UpdatePosWhenHeld()
        {
            throw new System.NotImplementedException();
        }

        void INT_IBaseBoardPin.UpdateRotWhenHeld()
        {
            throw new System.NotImplementedException();
        }

        protected abstract void TogglePin(bool _toggle);

        protected override void Awake()
        {
            base.Awake();

            m_outline = GetComponent<Outlinable>();
            m_hoverText.enabled = false;
        }


        protected override void OnHoverEntered(HoverEnterEventArgs args)
        {
            base.OnHoverEntered(args);

            m_hoverText.enabled = true;
            m_outline.enabled = true;
        }

        protected override void OnHoverExited(HoverExitEventArgs args)
        {
            base.OnHoverExited(args);

            m_hoverText.enabled = false;
            m_outline.enabled = false;
        }
    }
}