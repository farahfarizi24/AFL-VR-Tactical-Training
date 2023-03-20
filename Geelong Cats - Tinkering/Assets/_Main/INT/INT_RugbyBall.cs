using System.Collections;
using UnityEngine;

using Autohand;
using com.DU.CE.USER;

namespace com.DU.CE.INT
{
    [RequireComponent(typeof(Grabbable), typeof(NET_RugbyBall))]
    public class INT_RugbyBall : MonoBehaviour
    {
        [SerializeField] private SOC_UserUICoach m_CoachUI = null;
        [SerializeField] private INT_BallKicker m_BallKicker = null;
        [SerializeField] private TrailRenderer m_trailRenderer = null;

        [Range(0.1f, 5f)] public float HeldCountdown = 1.0f;

        private NET_RugbyBall m_NetComponent = null;
        private Grabbable m_GrabbableComponent = null;

        private bool m_isHeld = false;
        private EUSERHAND m_firstHeldHand = EUSERHAND.NONE;
        private bool m_isHeldByBothHands = false;

        private GameObject playerThatGrabbedBall;


        private Vector3 fieldKickPosition = Vector3.zero;


        private const int m_KICKSPHERE_LAYER = 15;


        #region MonoBehaviour Methods


        private void Awake()
        {
            m_NetComponent = GetComponent<NET_RugbyBall>();
            m_GrabbableComponent = GetComponent<Grabbable>();
        }

        private void OnEnable()
        {
            m_GrabbableComponent.OnGrabEvent += OnRubyBallPickup;
            m_GrabbableComponent.OnReleaseEvent += OnRugbyBallRelease;

            m_CoachUI.OnButtonRugbyBall += OnUICall;
            m_CoachUI.OnToggleBallTrail += OnToggleNetworkTrial;
        }

        private void OnDisable()
        {
            m_GrabbableComponent.OnGrabEvent -= OnRubyBallPickup;
            m_GrabbableComponent.OnReleaseEvent -= OnRugbyBallRelease;

            m_CoachUI.OnButtonRugbyBall -= OnUICall;
            m_CoachUI.OnToggleBallTrail -= OnToggleNetworkTrial;
        }

        private void OnRubyBallPickup(Hand hand, Grabbable grabbable)
        {
            if (!m_isHeld)
            {
                m_isHeld = true;
                m_firstHeldHand = (hand.left) ? EUSERHAND.LEFT : EUSERHAND.RIGHT;

                m_NetComponent.CutOwnership();
                m_NetComponent.GetOwnership();

                GetComponent<Rigidbody>().isKinematic = false;

                Debug.Log("The root gameobject name is: " + hand.gameObject.transform.root.name);

                playerThatGrabbedBall = hand.gameObject.transform.root.gameObject;
                if (playerThatGrabbedBall.GetComponentInChildren<BallKickerManager>())
                {
                    playerThatGrabbedBall.GetComponentInChildren<BallKickerManager>().PopulateBallInfo(this.gameObject, m_firstHeldHand);
                }

                //hand.gameObject.transform.root.name
            }
            else
            {
                m_isHeldByBothHands = true;
            }
        }

        private void OnRugbyBallRelease(Hand hand, Grabbable grabbable)
        {
            playerThatGrabbedBall = hand.gameObject.transform.root.gameObject;

            if (playerThatGrabbedBall.GetComponentInChildren<BallKickerManager>())
            {
                playerThatGrabbedBall.GetComponentInChildren<BallKickerManager>().DroppedBall();
            }

            EUSERHAND releasingHand = (hand.left) ? EUSERHAND.LEFT : EUSERHAND.RIGHT;
            m_isHeld = (releasingHand != m_firstHeldHand);

            // Start Countdown to set ball back to normal
            if (m_isHeld)
                StartCoroutine(CountdownToNormal());
        }

        IEnumerator CountdownToNormal()
        {
            yield return new WaitForSeconds(HeldCountdown);
            m_isHeldByBothHands = false;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == m_KICKSPHERE_LAYER)
            {
                //  If the ball is no longer held and was recently held by both arms
                if (!m_isHeld && m_isHeldByBothHands)
                {
                    m_BallKicker.LaunchBall(fieldKickPosition);
                }
            }
        }

        #endregion

        internal void OnUICall(Transform m_rugbyBallSpawnPoint)
        {
            m_NetComponent.GetOwnership();

            //GetComponent<Rigidbody>().isKinematic = true;
            transform.position = m_rugbyBallSpawnPoint.position;
            transform.rotation = m_rugbyBallSpawnPoint.rotation;
        }

        internal void OnToggleNetworkTrial(bool _toggle)
        {
            m_NetComponent.SetNetworkBallTrail(_toggle);
        }

        internal void OnNetworkLineRenderToggle(bool _toggle)
        {
            //Enable trail renderer
           // m_trailRenderer.enabled = _toggle;
        }
    }
}