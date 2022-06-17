using UnityEngine;

using Normal.Realtime;
using Normal.Realtime.Serialization;

using com.DU.CE.NET.NCM;

namespace com.DU.CE.NET
{
    public class NET_StadiumProperties : RealtimeComponent<NCM_StadiumProperties>
    {
        [SerializeField] private SOC_StadiumProperties m_StadiumPropertiesSock;

        private RealtimeView m_realtimeView = null;

        private void Awake()
        {
            m_realtimeView = GetComponent<RealtimeView>();
        }

        private void OnEnable()
        {
            m_StadiumPropertiesSock.OnChangeNetworkFieldLength += SetNetworkFieldLength;
            m_StadiumPropertiesSock.OnChangeNetworkFieldWidth += SetNetworkFieldWidth;

            m_StadiumPropertiesSock.RequestMarkerAddToNetwork += OnRequestMarkerAddToNetwork;

            m_StadiumPropertiesSock.OnBoardToggle += NetworkToggleBoard;

            m_StadiumPropertiesSock.OnSetupPlayer += SetupPlayer;
        }

        private void OnDisable()
        {
            m_StadiumPropertiesSock.OnChangeNetworkFieldLength -= SetNetworkFieldLength;
            m_StadiumPropertiesSock.OnChangeNetworkFieldWidth -= SetNetworkFieldWidth;

            m_StadiumPropertiesSock.RequestMarkerAddToNetwork -= OnRequestMarkerAddToNetwork;

            m_StadiumPropertiesSock.OnBoardToggle -= NetworkToggleBoard;
        }


        protected override void OnRealtimeModelReplaced(NCM_StadiumProperties previousModel, NCM_StadiumProperties currentModel)
        {
            if (previousModel != null)
            {
                // Unregister from NormCore events
                previousModel.fieldWidthDidChange -= OnFieldWidthChange;
                previousModel.fieldLengthDidChange -= OnFieldLengthChange;

                previousModel.markedPositions.modelAdded -= OnMarkerAdded;

                previousModel.boardOpenDidChange -= OnBoardToggle;
            }

            if (currentModel != null)
            {
                // If this is a model that has no data set on it, populate it with the current filed lines script
                if (currentModel.isFreshModel)
                {
                    currentModel.fieldLength = m_StadiumPropertiesSock.FieldLength;
                    currentModel.fieldWidth = m_StadiumPropertiesSock.FieldWidth;

                    Vector3[] markedPositions = m_StadiumPropertiesSock.MarkerPositions;
                    // Set marked positions
                    for (int i = 0; i < markedPositions.Length; i++)
                    {
                        NCM_PositionMarkerModel tempMarker = new NCM_PositionMarkerModel();
                        tempMarker.position = markedPositions[i];
                        currentModel.markedPositions.Add(tempMarker);
                    }
                    currentModel.boardOpen = false;
                }

                // Register for NormCore events
                currentModel.fieldLengthDidChange += OnFieldLengthChange;
                currentModel.fieldWidthDidChange += OnFieldWidthChange;

                currentModel.markedPositions.modelAdded += OnMarkerAdded;

                currentModel.boardOpenDidChange += OnBoardToggle;

                SetupPlayer();
            }

            //base.OnRealtimeModelReplaced(previousModel, currentModel);
        }


        private void SetupPlayer()
        {
            // Update Field Dimensions
            UpdateFieldLength(model.fieldLength);
            UpdateFieldWidth(model.fieldWidth);

            // Update Board
            BoardToggle(model.boardOpen);

            int noOfMarkedPositions = model.markedPositions.Count;
            // Update Markers
            foreach(NCM_PositionMarkerModel model in model.markedPositions)
            {
                AddMarker(model.position);
            }

            m_StadiumPropertiesSock.OnSetupPlayer += SetupPlayer;
        }


        #region Board -----------------------------------------------------

        private void NetworkToggleBoard(bool _toggle)
        {
            model.boardOpen = _toggle;
        }

        private void OnBoardToggle(NCM_StadiumProperties model, bool value)
        {
            //Debug.Log("#Stadium Properties#------------------Board Toggled");

            BoardToggle(value);
        }

        private void BoardToggle(bool _toggle)
        {
            m_StadiumPropertiesSock.NetworkBoardToggle(_toggle);
        }

        #endregion -----------------------------------------------------



        #region FieldLines ----------------------------------------

        internal void SetNetworkFieldLength(int _length)
        {
            model.fieldLength = _length;
        }
        internal void SetNetworkFieldWidth(int _width)
        {
            model.fieldWidth = _width;
        }


        private void OnFieldLengthChange(NCM_StadiumProperties model, int value)
        {
            UpdateFieldLength(value);
        }
        private void OnFieldWidthChange(NCM_StadiumProperties model, int value)
        {
            UpdateFieldWidth(value);
        }


        private void UpdateFieldLength(int _length)
        {
            m_StadiumPropertiesSock.NetworkSetFieldLength(_length);
        }
        private void UpdateFieldWidth(int _width)
        {
            m_StadiumPropertiesSock.NetworkSetFieldWidth(_width);
        }

        #endregion ----------------------------------------------



        #region Marked Positions ----------------------------------------

        private void OnRequestMarkerAddToNetwork(Vector3 _position)
        {
            NCM_PositionMarkerModel newMarker = new NCM_PositionMarkerModel();
            newMarker.position = _position;

            model.markedPositions.Add(newMarker);
        }

        private void OnMarkerAdded(RealtimeArray<NCM_PositionMarkerModel> array, NCM_PositionMarkerModel model, bool remote)
        {
            AddMarker(model.position);
        }

        public void AddMarker(Vector3 _newMarkerPoint)
        {
            //Debug.Log("#Stadium Properties#------------------Marker Added");

            m_StadiumPropertiesSock.NetworkAddMarker(_newMarkerPoint);
        }

        #endregion -----------------------------------------------------

    }
}