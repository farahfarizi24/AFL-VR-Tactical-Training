using Normal.Realtime;

using com.DU.CE.NET.NCM;

namespace com.DU.CE.INT
{
    public class NET_RugbyBall : RealtimeComponent<NCM_RugbyModel>
    {
        private RealtimeView m_realtimeView = null;
        private RealtimeTransform m_realtimeTransform = null;

        private INT_RugbyBall m_ballManager = null;

        #region MonoBehaviour Methods

        private void Awake()
        {
            m_realtimeView = GetComponent<RealtimeView>();
            m_realtimeTransform = GetComponent<RealtimeTransform>();

            m_ballManager = GetComponent<INT_RugbyBall>();
        }

        #endregion


        #region Normcore Callbacks


        protected override void OnRealtimeModelReplaced(NCM_RugbyModel previousModel, NCM_RugbyModel currentModel)
        {
            base.OnRealtimeModelReplaced(previousModel, currentModel);

            // Unregister from NormCore events
            if (previousModel != null)
            {
                // Unregister from NormCore events
                currentModel.isTREnabledDidChange -= OnBallTrailToggle;
            }

            if (currentModel != null)
            {
                // If this is a model that has no data set on it, populate it with the current data.
                if (currentModel.isFreshModel)
                {
                    currentModel.isTREnabled = false;
                    ToggleBallTrail(currentModel.isTREnabled);
                }

                // Register for NormCore events
                currentModel.isTREnabledDidChange += OnBallTrailToggle;
            }

            //base.OnRealtimeModelReplaced(previousModel, currentModel);
        }

        internal void SetNetworkBallTrail(bool _toggle)
        {
            model.isTREnabled = _toggle;
        }

        private void OnBallTrailToggle(NCM_RugbyModel model, bool value)
        {
            ToggleBallTrail(value);
        }

        internal void ToggleBallTrail(bool _toogle)
        {
            m_ballManager.OnNetworkLineRenderToggle(_toogle);
        }


        internal void CutOwnership()
        {
            m_realtimeView.ClearOwnership();
            m_realtimeTransform.ClearOwnership();
        }


        internal void GetOwnership()
        {
            m_realtimeView.RequestOwnership();
            m_realtimeTransform.RequestOwnership();
        }

        #endregion
    }
}