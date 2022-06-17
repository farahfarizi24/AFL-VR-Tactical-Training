using UnityEngine;


namespace com.DU.CE.LVL
{
    public class LVL_FieldLines : MonoBehaviour
    {
        #region Private Members

        [SerializeField] SOC_StadiumProperties m_StadiumPropertiesSock = null;

        [Header("Posts")]
        //---------------
        [SerializeField] private GameObject m_LeftPoleCenter = null;
        [SerializeField] private GameObject m_RightPoleCenter = null;

        [Header("Outer Ellipse Members")]
        //-------------------------------
        [SerializeField] private LineRenderer m_BoundaryLineRenderer = null;
        [SerializeField] [Range(50, 150)] private int m_BoundarySegments = 0;

        //[Header("50 Yard Line Members")]
        ////------------------------------
        //[SerializeField] [Range(50, 150)] private int m_50YardLineSegments = 0;
        //[SerializeField] private LineRenderer m_L50YardLineRenderer = null;
        //[SerializeField] private LineRenderer m_R50YardLineRenderer = null;

        #endregion



        #region MonoBehaviour Callbacks

        private void Start()
        {
            //NetworkSetFieldDimensions();
        }

        private void OnEnable()
        {
            m_StadiumPropertiesSock.OnNetworkSetFieldDimensions += NetworkSetFieldDimensions;
        }

        private void OnDisable()
        {
            m_StadiumPropertiesSock.OnNetworkSetFieldDimensions -= NetworkSetFieldDimensions;
        }

        #endregion



        private void NetworkSetFieldDimensions()
        {
            CalculateEllipse(m_StadiumPropertiesSock.FieldWidth, m_StadiumPropertiesSock.FieldLength, m_BoundarySegments, m_BoundaryLineRenderer);
            UpdatePolePositions();
        }



        #region Private Methods

        /// <summary>
        /// Calculates all the points to make an half ellipse
        /// </summary>
        private void CalculateEllipse(float x, float z, int segs, LineRenderer lr)
        {
            // Points for the passed line renderer
            Vector3[] points = new Vector3[segs + 1];

            for (int i = 0; i < segs; i++)
            {
                float angle = ((float)i / (float)segs) * 360 * Mathf.Deg2Rad;
                float tx = Mathf.Sin(angle) * x / 2;
                float tz = Mathf.Cos(angle) * z / 2;

                points[i] = new Vector3(tx, tz, 0.02f);
            }
            // Complete the loop
            points[segs] = points[0];

            lr.positionCount = segs + 1;
            lr.SetPositions(points);
        }

        /// <summary>
        /// Updates the position of the poles on both sides
        /// </summary>
        private void UpdatePolePositions()
        {
            // Get position of the center of poles on both sides
            m_LeftPoleCenter.transform.localPosition = (m_BoundaryLineRenderer.GetPosition(0));
            m_RightPoleCenter.transform.localPosition = (m_BoundaryLineRenderer.GetPosition(m_BoundarySegments / 2));

            // Draw 50 yard lines
            //CalculateHalfEllipse(x_50YardLine, z_50YardLine, m_50YardLineSegments, m_L50YardLineRenderer);
            //CalculateHalfEllipse(x_50YardLine, z_50YardLine, m_50YardLineSegments, m_R50YardLineRenderer);
        }

        #endregion



        //private void OnValidate()
        //{
        //    OnFieldSizeChange();
        //}


        //private void CalculateHalfEllipse(float x, float z, int segs, LineRenderer lr)
        //{
        //    // Making it half an ellipse
        //    int len = segs / 2;

        //    // To make the half ellipse start and end at the boundary
        //    int cutOff = len % 15;

        //    // Points for the passed line renderer
        //    Vector3[] points = new Vector3[(int)len - cutOff];

        //    int posCount = 0;
        //    for (int i = 0; i < len + 1; i++)
        //    {
        //        // Ignore the first and last few points
        //        if (i <= cutOff || i >= len - cutOff)
        //            continue;

        //        float angle = ((float)i / (float)segs) * 360 * Mathf.Deg2Rad;
        //        float tx = Mathf.Sin(angle) * x / 2;
        //        float tz = Mathf.Cos(angle) * z / 2;

        //        points[posCount] = new Vector3(tx, tz, 0.0f);
        //        posCount++;
        //    }

        //    lr.positionCount = posCount;
        //    lr.SetPositions(points);
        //}


        //// Not customizable as its a fixed distance
        //private float x_50YardLine = 110f;
        //private float z_50YardLine = 110f;

        //private void DrawFieldLines()
        //{
        //    // Draw outer boundary
        //    CalculateEllipse(m_StadiumPropertiesSock.FieldWidth, m_StadiumPropertiesSock.FieldLength, m_BoundarySegments, m_BoundaryLineRenderer);

        //    // Draw 50 yard lines
        //    CalculateHalfEllipse(x_50YardLine, z_50YardLine, m_50YardLineSegments, m_L50YardLineRenderer);
        //    CalculateHalfEllipse(x_50YardLine, z_50YardLine, m_50YardLineSegments, m_R50YardLineRenderer);
        //}
    }
}