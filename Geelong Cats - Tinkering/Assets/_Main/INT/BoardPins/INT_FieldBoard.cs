using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

using com.DU.CE.USER;

namespace com.DU.CE.INT
{
    [RequireComponent(typeof(INT_BoardPins))]
    public class INT_FieldBoard : XRBaseInteractable
    {
        [SerializeField] private SOC_StadiumProperties m_StadiumProperties;
        internal SOC_StadiumProperties StadiumPropertiesSock { get => m_StadiumProperties; }

        [SerializeField] private SOC_FieldBoard m_BoardSock = null;
        internal SOC_FieldBoard BoardSock { get => m_BoardSock; }

       public USER_LocalUser m_LocalUser;
        [SerializeField] private InputActionProperty m_RightPlacementInputAction;
        [SerializeField] private InputActionProperty m_LeftPlacementInputAction;
        [SerializeField] private GameObject m_PlaneMesh;

        private SOC_AUserUI m_UserUISock = null;

        private USER_CustomRayInteractor m_tempInteractor = null;
        private USER_CustomRayInteractor m_rightRayInteractor = null;
        private USER_CustomRayInteractor m_leftRayInteractor = null;

        private INT_BoardPins m_boardPins = null;

        #region Monobehaviour Callbacks 

        protected override void Awake()
        {
            base.Awake();


            m_LocalUser = GameObject.FindGameObjectWithTag("coach").GetComponent<USER_LocalUser>();
            m_UserUISock = m_LocalUser.UserSock.UISock;

            m_boardPins = GetComponent<INT_BoardPins>();

              if (m_LocalUser.Role.Equals(EUSERROLE.PLAYER))
               m_StadiumProperties.OnSetupPlayer += SetupPlayer;

           // SetupPlayer();
        }

        private void SetupPlayer()
        {
            bool isBoardOpen = m_StadiumProperties.isBoardOpen;
           // isBoardOpen = true;
            m_boardPins.SetupPlayer(isBoardOpen, m_StadiumProperties.MarkerPositions);

            UIToggleFieldBoard(isBoardOpen);

            m_StadiumProperties.OnSetupPlayer -= SetupPlayer;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            m_StadiumProperties.OnNetworkFieldBoardToggle += UIToggleFieldBoard;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            m_StadiumProperties.OnNetworkFieldBoardToggle -= UIToggleFieldBoard;
        }

        private void UIToggleFieldBoard(bool _status)
        {
            m_PlaneMesh.SetActive(_status);
            m_BoardSock.EagleEyeCamera.enabled = _status;

            if (!_status)
                m_UserUISock.ToggleBoardLocUI(_status, Vector3.zero);
        }

        #endregion



        #region XRInteractable Callbacks

        protected override void OnHoverEntered(HoverEnterEventArgs args)
        {
            base.OnHoverEntered(args);

            m_tempInteractor = args.interactor as USER_CustomRayInteractor;

            if (m_tempInteractor.Hand.Equals(EUSERHAND.RIGHT))
            {
                m_rightRayInteractor = m_tempInteractor;
                m_RightPlacementInputAction.action.RemoveAllBindingOverrides();
                m_RightPlacementInputAction.action.performed += OnPlacementActionRight;
            }
            else
            {
                m_leftRayInteractor = m_tempInteractor;
                m_LeftPlacementInputAction.action.RemoveAllBindingOverrides();
                m_LeftPlacementInputAction.action.performed += OnPlacementActionLeft;
            }

            m_tempInteractor = null;
        }

        protected override void OnHoverExited(HoverExitEventArgs args)
        {
            base.OnHoverExited(args);

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
            m_UserUISock.ToggleBoardLocUI(true, hit.point);
        }


        private void OnPlacementActionLeft(InputAction.CallbackContext obj)
        {
            if (!m_leftRayInteractor)
                return;

            RaycastHit hit;
            m_leftRayInteractor.TryGetCurrent3DRaycastHit(out hit);

            // Show placement UI
            m_UserUISock.ToggleBoardLocUI(true, hit.point);
        }

        #endregion

    }
}