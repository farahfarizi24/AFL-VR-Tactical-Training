using UnityEngine;
using com.DU.CE.UI;

namespace com.DU.CE.USER
{
    public class USER_UI : USER_AComponent
    {
        private const string BOARDUI_PREFIX = "BoardLocUI_";
        private const string HANDUI_PREFIX = "HandUI_";

        [SerializeField] private USER_Manager m_Manager = null;
        [SerializeField] private Canvas m_HandUICanvas = null;

        private SOC_AUserUI m_uiSock = null;
        private UI_InteractableOptions m_boardUI = null;
        private UI_InteractableOptions m_fieldUI = null;

        private void OnEnable()
        {
            m_HandUICanvas.enabled = false;

            p_StateMachineSock.OnStateChange += OnUserStateChange;

        }

        protected override void OnUserStateChange(EUSERSTATE oldState, EUSERSTATE newState)
        {
            base.OnUserStateChange(oldState, newState);
        }

        protected override void Initialize(EUSERROLE _role)
        {
            base.Initialize(_role);

            // Get References
            m_uiSock = m_Manager.UserSock.UISock;
            m_uiSock.OnHandUIToggle += OnHandUIToggle;
            m_uiSock.OnBoardLocUIToggle += OnBoardUIToggle;

            if (_role.Equals(EUSERROLE.COACH))
                (m_uiSock as SOC_UserUICoach).OnFieldLocUIToggle += OnFieldUIToggle;

            string UIName;
            GameObject UI;

            // Instantitate HandUI
          UIName = HANDUI_PREFIX + _role.ToString();
         UI = Resources.Load(UIName) as GameObject;
      //      Debug.Log("#USER_UI#-----------------Instantiating: " + UI.name);
            Instantiate(UI, m_HandUICanvas.transform);

            // Instatitate BoardUI
            UIName = BOARDUI_PREFIX + _role.ToString();
            UI = Resources.Load(UIName) as GameObject;
            //Debug.Log("#USER_UI#-----------------Instantiating: " + UI.name);
            m_boardUI = Instantiate(UI).GetComponent<UI_InteractableOptions>();

            m_boardUI.InitializeUI(m_HandUICanvas.worldCamera);
            m_boardUI.ToggleUI(false);

            // Instantiate FieldUI if Coach
            if (_role.Equals(EUSERROLE.COACH))
            {
                UI = Resources.Load("FieldLocUI_COACH") as GameObject;
                //Debug.Log("#USER_UI#-----------------Instantiating: " + UI.name);
                m_fieldUI = Instantiate(UI).GetComponent<UI_InteractableOptions>();

                m_fieldUI.InitializeUI(m_HandUICanvas.worldCamera);
                m_fieldUI.ToggleUI(false);
            }
        }

        private void OnHandUIToggle(bool _toggle)
        {
            m_HandUICanvas.enabled = _toggle;
        }


        private void OnDisable()
        {
            p_StateMachineSock.OnStateChange += OnUserStateChange;

            m_uiSock.OnHandUIToggle -= OnHandUIToggle;
            m_uiSock.OnBoardLocUIToggle -= OnBoardUIToggle;

            if (p_role.Equals(EUSERROLE.COACH))
                (m_uiSock as SOC_UserUICoach).OnFieldLocUIToggle += OnFieldUIToggle;
        }

        private void OnBoardUIToggle(bool _toggle, Vector3 _position)
        {
            if (_toggle)
            {
                m_boardUI.PlaceUI(_position);
                m_fieldUI.ToggleUI(false);
            }
            m_boardUI.ToggleUI(_toggle);
        }


        private void OnFieldUIToggle(bool _toggle, Vector3 _position)
        {
            if (_toggle)
            {
                m_fieldUI.PlaceUI(_position);
            }
            m_fieldUI.ToggleUI(_toggle);
        }
    }
}