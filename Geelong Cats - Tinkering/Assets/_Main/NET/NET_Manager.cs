using UnityEngine;

using Normal.Realtime;

using com.DU.CE.LVL;
using com.DU.CE.USER;
using UnityEngine.SceneManagement;

namespace com.DU.CE.NET
{
    public class NET_Manager : MonoBehaviour
    {
        public GameObject CoachInterface;
        public GameObject ConnectionUI;
        [SerializeField] private GameObject m_CoachNCLocalAvatarPrefab;
        [SerializeField] private GameObject m_PlayerNCLocalAvatarPrefab;

        [Header("Socks")]
        [SerializeField] private SOC_LvlStateMachine m_LvlStateMachine;

        [Header("Network Socks)")]
        [SerializeField] private SOC_NetManager m_NetManagerSock;

        // ----------------- Set on Awake
        public Realtime NCRealtime { get; private set; }
        public RealtimeAvatarManager NCAvatarManager { get; private set; }
        // ------------------------------

        private Room m_ncRoom = null;

        private int m_noOfPlayersInRoom = 0;
        private Transform[] playersInRoom;

        #region Monobehaviour Methods

        private void Awake()
        {
            // Get references
            NCRealtime = GameObject.FindObjectOfType<Realtime>();
            NCAvatarManager = NCRealtime.GetComponent<RealtimeAvatarManager>();

            // Initialize network socks
            m_NetManagerSock.Inititalize(this);
        }

        private void OnEnable()
        {
            //Register to Realtime
            NCRealtime.didConnectToRoom += OnConnectedToRoom;
            NCRealtime.didConnectToRoom += OnDisconnectedFromRoom;

            NCAvatarManager.avatarCreated += OnPlayerAvatarCreated;
            NCAvatarManager.avatarDestroyed += OnPlayerAvatarDestroyed;
        }

        private void OnDisable()
        {
            //Register to Realtime
            NCRealtime.didConnectToRoom -= OnConnectedToRoom;
            NCRealtime.didConnectToRoom -= OnDisconnectedFromRoom;

            NCAvatarManager.avatarCreated -= OnPlayerAvatarCreated;
            NCAvatarManager.avatarDestroyed -= OnPlayerAvatarDestroyed;
        }


        #endregion


        #region Realtime Callbacks

        private void OnConnectedToRoom(Realtime realtime)
        {
            NCRealtime = realtime;
            m_ncRoom = NCRealtime.room;

            m_ncRoom.connectionStateChanged += OnConnectionStatusChanged;

            m_LvlStateMachine.OnRequestStateChange(ELVLSTATE.ONCONNECT_SETUP);
        }


        private void OnConnectionStatusChanged(Room room, Room.ConnectionState previousConnectionState, Room.ConnectionState connectionState)
        {
            Debug.Log("#Network Manager#----------Connection status changed from: " 
                + previousConnectionState +" to: " + connectionState);
        }

        private void OnDisconnectedFromRoom(Realtime realtime)
        {
            //TODO:
        }

        private void OnPlayerAvatarCreated(RealtimeAvatarManager avatarManager, RealtimeAvatar avatar, bool isLocalAvatar)
        {
            m_noOfPlayersInRoom = NCAvatarManager.avatars.Count;

            Debug.Log("#Network Manager#----------No. of users in room after OnPlayerAvatarCreated: " + m_noOfPlayersInRoom);

            if (avatar.GetComponent<NET_LocalAvatar>().UserRole.Equals(EUSERROLE.COACH))
            {
                if(m_NetManagerSock.Coach == null)
                {
                    m_NetManagerSock.SetCoach(avatar.transform);
                   
                }
       
                else
                {
                    //m_NetManagerSock.AddCoachToList(avatar.transform);
                    ///////CHANGE THIS SO WE CAN HAVE MORE THAN ONCE COACH
                     NCRealtime.Disconnect();
                     m_NetManagerSock.Disconnect(1, "A coach is already in room");
                    return;
                }
            }
            else
            {
                m_NetManagerSock.AddPlayerToList(avatar.transform);
            }

            // Local avatar always added in last
            if (isLocalAvatar)
            {
                if (m_NetManagerSock.Coach == null)
                {
                    NCRealtime.Disconnect();
                    m_NetManagerSock.Disconnect(0, "No Coach in the room");
                    return;
                }

                m_NetManagerSock.SetLocalAvatar(avatar.transform);
                m_LvlStateMachine.OnRequestStateChange(ELVLSTATE.SETUP_USER);
                return;
            }
        }

        private void OnPlayerAvatarDestroyed(RealtimeAvatarManager avatarManager, RealtimeAvatar avatar, bool isLocalAvatar)
        {
            // If Coach Disconnects
            if (avatar.GetComponent<NET_LocalAvatar>().UserRole.Equals(EUSERROLE.COACH))
            {
                //remove this so player can enter without coach
                NCRealtime.Disconnect();
                SceneManager.LoadScene(0);
            }

            else
                Debug.Log("#Network Manager#----------Player destroyed");
        }

        #endregion



        #region Internal Methods - for SOC_PlayerProperties

        public void SetNCLocalAvatarPrefab(EUSERROLE prefabRole)
        {
            NCAvatarManager.localAvatarPrefab = (prefabRole == EUSERROLE.COACH) ?
                m_CoachNCLocalAvatarPrefab : m_PlayerNCLocalAvatarPrefab;
        }


        internal void ConnectToRoom(string roomName)
        {
            // Connect to the room inputed by the user
            NCRealtime.Connect(roomName);
        }

        #endregion
    }
}
