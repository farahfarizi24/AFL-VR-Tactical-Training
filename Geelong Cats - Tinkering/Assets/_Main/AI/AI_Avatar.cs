using com.DU.CE.INT;
using com.DU.CE.LVL;
using com.DU.CE.NET.NCM;
using EPOOutline;
using Normal.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
namespace com.DU.CE.AI
{
    [RequireComponent(typeof(RealtimeView))]
    [RequireComponent(typeof(RealtimeTransform))]
    public class AI_Avatar : RealtimeComponent<NCM_AvatarModel>, INT_ILinkedPinObject
    {
        #region Private Members

        [SerializeField] private SOC_FieldBoard m_boardSock = null;
        [Space]
        [SerializeField] private AI_CustomXRInteractable m_interactable = null;
        [SerializeField] private AI_PathManager m_pathManager = null;
        [SerializeField] private NavMeshAgent m_navMeshAgent = null;
        [SerializeField] private Rigidbody m_rigidBody = null;
        [SerializeField] private SkinnedMeshRenderer m_meshRenderer = null;
        [SerializeField] private GameObject m_uiElementsParent = null;
        [Space]
        [SerializeField] private TextMeshPro m_numberText = null;
        [SerializeField] private Outlinable m_outline = null;

        private int m_teamNumber = 0;
        private ETEAM m_team = 0;
        public List<Vector3> Position = new List<Vector3>();
        public List<Vector3> Rotation = new List<Vector3>();
        private float m_rotationY = 0f;

        #endregion

        public int M_PlayerNumber { get => m_teamNumber; }
        public ETEAM M_Team { get => m_team; }

        public NCM_AvatarModel M_NCModel { get => model; }

        // These methods are used to sync the data to the client models
        // They are called when the value stores in the realtime model changes.
        #region NormCore Callbacks


        protected override void OnRealtimeModelReplaced(NCM_AvatarModel previousModel, NCM_AvatarModel currentModel)
        {
            if (previousModel != null)
            {
                // Unregister from NormCore events
                previousModel.numberDidChange -= InitializeNumber;
                previousModel.teamDidChange -= InitializeTeam;
                previousModel.isSelectedDidChange -= OnSelectChanged;
                previousModel.isActivatedDidChange -= OnActivated;
            }


            if (currentModel != null)
            {
                // If this is a model that has no data set on it, populate it with the current data.
                if (currentModel.isFreshModel)
                {
                    currentModel.team = (int)m_team;
                    currentModel.number = m_teamNumber;
                    currentModel.isSelected = m_interactable.isSelected;
                    currentModel.isActivated = false;

                    // Instantiate a Board Pin
                    m_boardSock.AIInstantiateCall(transform);

                    Activate(currentModel.isActivated);
                }

                // Register for NormCore events
                currentModel.teamDidChange += InitializeTeam;
                currentModel.numberDidChange += InitializeNumber;
                currentModel.isSelectedDidChange += OnSelectChanged;
                currentModel.isActivatedDidChange += OnActivated;

                m_team = (ETEAM)model.team;
                m_teamNumber = model.number;
                m_numberText.SetText(model.number.ToString());
                Activate(currentModel.isActivated);
            }

            //base.OnRealtimeModelReplaced(previousModel, currentModel);
        }


        public void ChangeNetworkActivation(bool _toggle)
        {
            model.isActivated = _toggle;
        }

        private void OnActivated(NCM_AvatarModel model, bool toggle)
        {
            Activate(toggle);
        }

        internal void Activate(bool toggle)
        {
            // Disable Parent
            m_interactable.enabled = toggle;
            m_navMeshAgent.enabled = toggle;
            m_rigidBody.isKinematic = !toggle;
            m_interactable.colliders[0].enabled = toggle;
            m_meshRenderer.enabled = toggle;
            m_uiElementsParent.SetActive(toggle);

            // Set pin status
            m_linkedPin?.SetObjectStatus(toggle);
        }

        private void InitializeTeam(NCM_AvatarModel model, int value)
        {
            m_team = (ETEAM)model.team;

            m_linkedPin.SetupPin(m_team, m_teamNumber);
        }

        // Initialize team number and display it
        private void InitializeNumber(NCM_AvatarModel model, int value)
        {
            m_teamNumber = model.number;
            // Set teamnumber on number overhead display
            m_numberText.SetText(model.number.ToString());

            m_linkedPin.SetupPin(m_team, m_teamNumber);
        }

        internal void OnHoverChanged(bool value)
        {
            if (value)
            {
                m_outline.enabled = true;
                m_outline.OutlineColor = Color.yellow;
                m_numberText.color = Color.yellow;

                LVL_SoundManager.PlayMusic("AIHover");
            }
            else
            {
                m_outline.enabled = false;
                m_numberText.color = Color.white;
            }
        }

        private void OnSelectChanged(NCM_AvatarModel model, bool value)
        {
            m_rigidBody.isKinematic = value;
            m_navMeshAgent.isStopped = true;

            if (value)
            {
                m_outline.enabled = true;
                m_outline.OutlineColor = Color.green;
                m_numberText.color = Color.green;
            }
            else
            {
                m_outline.enabled = false;
                m_numberText.color = Color.white;

                m_linkedPin.UpdatePinPosition();
            }
        }
    
        #endregion


        /// <summary>
        /// This method is accessed by the spawnner to initialize the AI's
        /// team and team number
        /// </summary>
        public void SetTeamInfo(ETEAM team, int number)
        {
            model.team = (int)team;
            model.number = number;
        }


        #region BoadPinObject Interface methods

        private INT_IBoardLinkedPin m_linkedPin;

        void INT_ILinkedPinObject.LinkPin(INT_IBoardLinkedPin pin)
        {
            m_linkedPin = pin;
            m_linkedPin.SetupPin(m_team, m_teamNumber);
            m_linkedPin.SetObjectStatus(model.isActivated);
        }

        Transform INT_ILinkedPinObject.GetRelativeTransform()
        {
            return transform;
        }

        void INT_ILinkedPinObject.SetNavAgentDestination(Vector3 _destinationInXY)
        {
            // Unpause nav mesh agent
            m_navMeshAgent.isStopped = false;

            // Set the destination for the path agent
            m_navMeshAgent.SetDestination(_destinationInXY);
        }

        void INT_ILinkedPinObject.SetTransform(Vector3 _destinationPosition, Vector3 _destinationRotation)
        {
          
            m_linkedPin.UpdateForLoad(_destinationPosition,_destinationRotation);
        }


        void INT_ILinkedPinObject.SetRelativeYRotation(float rotY)
        {
            m_rotationY = rotY;
            gameObject.transform.eulerAngles = new Vector3(0.0f, m_rotationY, 0.0f);
        }

        float INT_ILinkedPinObject.GetRelativeYRotation()
        {
            m_rotationY = gameObject.transform.eulerAngles.y; 
            return m_rotationY;
        }

       

        #endregion
    }
}

