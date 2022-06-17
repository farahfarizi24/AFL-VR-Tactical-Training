using System.Collections;

using UnityEngine;
using Random = UnityEngine.Random;

using com.DU.CE.AI;

namespace com.DU.CE.LVL
{
    [RequireComponent(typeof(LVL_AIActivator))]
    public class LVL_AISpawner : LVL_ASpawner
    {
        public const int NoOfAIPerTeam = 8;

        // Spawn Params
        [Header("Spawner Params")]
        [SerializeField] private Transform m_HomeParentSpawnLoc = null;
        [SerializeField] private Transform m_AwayParentSpawnLoc = null;
        [SerializeField] private Vector3 m_SpawnPosOffset = Vector3.zero;

        private Transform[] m_HomeTeamPool;
        public Transform[] HomePool { get => m_HomeTeamPool; }

        private Transform[] m_AwayTeamPool;
        public Transform[] AwayPool { get => m_AwayTeamPool; }

        private string[] m_homePrefabNames = null;

        private string[] m_awayPrefabNames = null;

        private struct m_STeamInfo
        {
            public ETEAM Team;

            public string TeamFolderName;
            public string[] PrefabNames;
            public Transform Parent;

            public int totalNo;
        }

        private m_STeamInfo m_homeTeamInfo;
        private m_STeamInfo m_awayTeamInfo;


        private LVL_AIActivator m_aiActivator = null;

        private void Awake()
        {
            // Initialize Random Seed
            int seed = System.DateTime.Now.Millisecond;
            Random.InitState(seed);

            // Fill avatar prefab's name arrays
            m_homePrefabNames = GetPrefabsNames(Resources.LoadAll<GameObject>("HOME"));
            m_awayPrefabNames = GetPrefabsNames(Resources.LoadAll<GameObject>("AWAY"));

            m_HomeTeamPool = new Transform[NoOfAIPerTeam];
            m_AwayTeamPool = new Transform[NoOfAIPerTeam];

            if (m_homePrefabNames == null || m_awayPrefabNames == null)
                Debug.LogError("No prefabs of players found in resources");

            m_aiActivator = GetComponent<LVL_AIActivator>();
        }


        private string[] GetPrefabsNames(in GameObject[] prefabList)
        {
            string[] ListOfNames = new string[prefabList.Length];

            for (int i = 0; i < prefabList.Length; i++)
            {
                ListOfNames[i] = prefabList[i].name;
            }

            return ListOfNames;
        }


        public void SpawnTeams()
        {
            Debug.Log("#LVL_AvatarSpawnner#-------------------------Spwanning AI");

            SetupHomeTeamInfo();
            SpawnTeam(ref m_homeTeamInfo);

            SetupAwayTeamInfo();
            SpawnTeam(ref m_awayTeamInfo);
        }


        private void SpawnTeam(ref m_STeamInfo teamInfo)
        {
            int noOfPlayersToSpawn = NoOfAIPerTeam;

            ETEAM team = teamInfo.Team;

            Vector3 spawnPosition = teamInfo.Parent.position;
            spawnPosition += m_SpawnPosOffset;

            int reverseSpawnLoc = 1;
            bool reversed = false;

            for (int i = 0; i < noOfPlayersToSpawn; i++)
            {
                teamInfo.totalNo = i;

                SpawnAI(teamInfo, spawnPosition);

                // Check if half have spawned inorder to start spawnning on the other side
                if (i == (noOfPlayersToSpawn / 2) - 1 && !reversed)
                {
                    reverseSpawnLoc = -1;
                    spawnPosition = teamInfo.Parent.position;
                    reversed = true;
                }

                // Add the offset
                spawnPosition += (m_SpawnPosOffset * reverseSpawnLoc);
            }
        }

        private void SpawnAI(m_STeamInfo TeamInfo, Vector3 spawnLocation)
        {
            // Choose random prefab
            string nameOfPrefabToSpawn = TeamInfo.PrefabNames[(Random.Range(0, m_homePrefabNames.Length - 1))];

            // Spawn ai
            Transform temp = p_NetSpawnerSock.InstantiateNetObject(
                                TeamInfo.TeamFolderName + "/" + nameOfPrefabToSpawn,    // Prefab file location
                                spawnLocation,                                          // Location
                                TeamInfo.Parent.rotation,                               // Rotation
                                true,                                                   // Yes it is owned by coach
                                true                                                    // Prevent other clients from calling RequestOwnership() on the root RealtimeView.
                            ).transform;

            SetupAI(TeamInfo, temp);
        }

        private void SetupAI(m_STeamInfo teamInfo, Transform ai)
        {
            int playerNum = teamInfo.totalNo;
            ai.GetComponent<AI_Avatar>().SetTeamInfo(teamInfo.Team, playerNum);
            ai.GetComponent<AI_Avatar>().ChangeNetworkActivation(false);

            if (teamInfo.Team == ETEAM.HOME)
                m_HomeTeamPool[playerNum] = ai;
            else
                m_AwayTeamPool[playerNum] = ai;

            // Set parent of AI
            ai.parent = teamInfo.Parent;
            ai.localRotation = Quaternion.identity;

            ai.name = teamInfo.TeamFolderName + (playerNum);
        }


        protected override IEnumerator OnPlayerSetup()
        {
            //Find all avatars and parent them under the referenced gameobject
            m_aiActivator.SetupForPlayer();

            yield return new WaitForSeconds(1);

            p_LevelStateMachineSock.OnRequestStateChange(ELVLSTATE.SETUP_USERCOMPONENTS);

            this.gameObject.SetActive(false);
        }

        // Called by parent class
        protected override IEnumerator OnCoachSetup()
        {
            SpawnTeams();

            yield return null;

            p_LevelStateMachineSock.OnRequestStateChange(ELVLSTATE.SETUP_USERCOMPONENTS);
        }

        private void SetupHomeTeamInfo()
        {
            m_homeTeamInfo.Team = ETEAM.HOME;
            m_homeTeamInfo.TeamFolderName = "HOME";
            m_homeTeamInfo.PrefabNames = m_homePrefabNames;
            m_homeTeamInfo.Parent = m_HomeParentSpawnLoc;
            m_homeTeamInfo.totalNo = 0;
        }

        private void SetupAwayTeamInfo()
        {
            m_awayTeamInfo.Team = ETEAM.AWAY;
            m_awayTeamInfo.TeamFolderName = "AWAY";
            m_awayTeamInfo.PrefabNames = m_awayPrefabNames;
            m_awayTeamInfo.Parent = m_AwayParentSpawnLoc;
            m_awayTeamInfo.totalNo = 0;
        }
    }
}
