using com.DU.CE.AI;
using System.Collections.Generic;
using UnityEngine;

namespace com.DU.CE.INT
{
    [RequireComponent(typeof(INT_BoardPins))]
    public class INT_BoardPins : MonoBehaviour
    {
        [SerializeField] private Transform m_parentofPins = null;
        [SerializeField] private Transform m_parentofMarkerPins = null;
        [SerializeField] private SOC_AI m_aiSock;

        [Header("Debug members")]
        public int noOfPins = 0;
        public int noOfPositionPins = 0;
        public int noOfMarkerPins = 0;

        [SerializeField] private bool m_isBoardEnabled = false;
        [SerializeField] private INT_FieldBoard m_FieldBoard;

        // List of all pins
        [SerializeField] private List<INT_IBaseBoardPin> m_boardPins = new List<INT_IBaseBoardPin>();

        // Array of all the position pins
        [SerializeField] private List<INT_IBoardLinkedPin> m_boardPosPins = new List<INT_IBoardLinkedPin>();


        #region Monobehaviour Callbacks

        private void Awake()
        {
            m_FieldBoard = GetComponent<INT_FieldBoard>();
        }

        private void OnEnable()
        {
            m_FieldBoard.StadiumPropertiesSock.OnNetworkFieldBoardToggle += OnUIToggle;

            m_FieldBoard.BoardSock.OnUIMarkerAdd += OnUIMarkerAdded;
            m_FieldBoard.StadiumPropertiesSock.OnNetworkMarkerAdd += OnNetworkMarkerAdded;

            m_FieldBoard.BoardSock.OnAIInstantiate += InstantiatePositionPin;
        }

        private void OnDisable()
        {
            m_FieldBoard.StadiumPropertiesSock.OnNetworkFieldBoardToggle -= OnUIToggle;

            m_FieldBoard.BoardSock.OnUIMarkerAdd -= OnUIMarkerAdded;
            m_FieldBoard.StadiumPropertiesSock.OnNetworkMarkerAdd -= OnNetworkMarkerAdded;

            m_FieldBoard.BoardSock.OnAIInstantiate -= InstantiatePositionPin;
        }

        //private void LateUpdate()
        //{
        //    if (!m_isInstantiated || m_isPicked)
        //        return;

        //    // Match the pin positions to the AI positions
        //    SetPinPositions();
        //}

        internal void SetupPlayer(bool isBoardOpenOnNetwork, Vector3[] markedPositions)
        {
            foreach(Vector3 pos in markedPositions)
            {
                CreateMarker(pos);
            }

            foreach(AI_Avatar ai in m_aiSock.ais)
            {
                InstantiatePositionPin(ai.transform);
                m_boardPosPins[m_boardPosPins.Count - 1].SetObjectStatus(ai.M_NCModel.isActivated);

                if (ai.M_NCModel.isActivated)
                {
                    Debug.LogFormat("#LVL_AIActivator#-------------------------Found Activated AI: {0} {1}",
                        ai.M_NCModel.team, ai.M_NCModel.number);
                }
            }

            OnUIToggle(isBoardOpenOnNetwork);
        }

        #endregion



        #region Marker Pins Methods



        private void OnUIMarkerAdded(Vector3 _uiPinWorldPosition)
        {
            Vector3 localPosition = transform.InverseTransformPoint(_uiPinWorldPosition);
            m_FieldBoard.StadiumPropertiesSock.AddNewMarkerToNetwork(localPosition);
        }

        private void OnNetworkMarkerAdded(Vector3 _localPinPos)
        {
            Debug.Log("#BoardPins#-------------------------Marker Added" + _localPinPos);

            CreateMarker(_localPinPos);
        }

        private void CreateMarker(Vector3 _localBoardPos)
        {
            // Instantiate the pin under the placement canvas first
            Transform pinTransform = Instantiate(m_FieldBoard.BoardSock.PosMarkerPrefab).transform;
            pinTransform.parent = m_parentofMarkerPins;
            pinTransform.localPosition = _localBoardPos;

            INT_IBaseBoardPin boardPin = pinTransform.GetComponent<INT_IBaseBoardPin>();
            m_boardPins.Add(boardPin);
            boardPin.SwitchPin(m_isBoardEnabled);

            noOfMarkerPins++;
            noOfPins++;
        }


        #endregion



        #region Position Pin Methods

        private void InstantiatePositionPin(Transform _aiTransform)
        {
            Transform pinTransform = Instantiate(m_FieldBoard.BoardSock.PosPinPrefab,
                m_parentofPins.transform).transform;

            INT_IBoardLinkedPin posPin = pinTransform.GetComponent<INT_IBoardLinkedPin>();
            posPin.LinkObject(_aiTransform);
            posPin.UpdatePinPosition();

            // Add spawnned pin to list
            m_boardPins.Add(posPin);
            m_boardPosPins.Add(posPin);

            noOfPositionPins++;
            noOfPins++;
        }


        /// <summary>
        /// This is run every time 
        /// Gets the position from the BoardPinObject and applys the trasformation math
        /// Then set the position of the respective BoardPin accordingly
        /// </summary>
        private void SetPositionPinPositions()
        {
            if (m_boardPosPins.Count < 1)
                return;

            // Update each position pin
            for (int i = 0; i < m_boardPosPins.Count; i++)
            {
                m_boardPosPins[i].UpdatePinPosition();
            }
        }


        #endregion


        private void OnUIToggle(bool _status)
        {
            m_isBoardEnabled = _status;

            TogglePins(_status);
            SetPositionPinPositions();
        }


        private void TogglePins(bool val)
        {
            if (m_boardPins == null)
                return;

            for (int i = 0; i < m_boardPins.Count; i++)
            {
                m_boardPins[i].SwitchPin(val);
            }
        }
    }
}
