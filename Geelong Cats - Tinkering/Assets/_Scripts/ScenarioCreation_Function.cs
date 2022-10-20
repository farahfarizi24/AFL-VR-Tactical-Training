using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.DU.CE.INT;
using UnityEngine.UI;
using com.DU.CE.AI;
using System.Xml.Serialization;
using System.IO;
using com.DU.CE.NET;
using com.DU.CE.USER;
using System;
using System.Linq;

public class ScenarioCreation_Function : MonoBehaviour
    {
        public SOC_AI AISock = null;
        public Transform HomeTeamLocation = null;
        public Transform AwayTeamLocation = null;
        private int AwayAICounter = 0;
        private int HomeAICounter = 0;
    string filePath;
    /// <summary>
    ///  Under is the list of variable to be stored for each AI
    ///
    public Action<ScenarioData> OnChangeScenario;
    [SerializeField] private List<Vector3> AwayAIInitialPosition = new List<Vector3>();
        [SerializeField] private List<Vector3> AwayAIFinalPosition = new List<Vector3>();
        [SerializeField] private List<Vector3> AwayAIInitialRotation = new List<Vector3>();
        [SerializeField] private List<Vector3> AwayAIFinalRotation = new List<Vector3>();

        [SerializeField] private List<Vector3> HomeAIInitialPosition = new List<Vector3>();
        [SerializeField] private List<Vector3> HomeAIFinalPosition = new List<Vector3>();
        [SerializeField] private List<Vector3> HomeAIInitialRotation = new List<Vector3>();
        [SerializeField] private List<Vector3> HomeAIFinalRotation = new List<Vector3>();
    public Action<GameObject, Vector3, Vector3> OnChangePlayerPosition;

    private List<String> AwayAIRole = new List<string>();
        private List<String> HomeAIRole = new List<string>();
        //Role FB, BP HB, CB, FF, FP, HF, CHF, MF (these are the recognised string

        public int ScenarioNumber;
        public int BallTargetAINum;//denotes which AI is the target for final ball position,
                                   //this will be counted the same with their Counter number


        /// <summary>
        /// Button 
        /// </summary>
        public Button HomeCreate;
        public Button AwayCreate;
        public Button SaveFinalPosition;
        public Button SaveInitialPosition;
        public Button RunScenario;
        public Button BallTarget;
    public Button SaveEntireScenario;

        // Start is called before the first frame update
        void Start()
    {
        filePath = Application.persistentDataPath + "/gamedate.txt";
        GameObject HomeLoc = GameObject.FindGameObjectWithTag("HomeSpawnLoc");
            GameObject AwayLoc = GameObject.FindGameObjectWithTag("AwaySpawnLoc");
            HomeTeamLocation = HomeLoc.transform;
            AwayTeamLocation = AwayLoc.transform;
            HomeCreate.onClick.AddListener(CreateHomeAI);
            AwayCreate.onClick.AddListener(CreateAwayAI);
            SaveFinalPosition.onClick.AddListener(delegate { SaveLocationAndPosition("Final"); });
            SaveInitialPosition.onClick.AddListener(delegate { SaveLocationAndPosition("Initial"); });
            RunScenario.onClick.AddListener(QuickRunScenario);
            BallTarget.onClick.AddListener(SetBallTarget);
            SaveEntireScenario.onClick.AddListener(delegate { SaveScenario(ScenarioNumber); });



        }

        private void SetBallTarget()
        {
            throw new NotImplementedException();
        }

        private void QuickRunScenario()
        {
        LoadScenario(ScenarioNumber);
        }





        private void SaveLocationAndPosition(string state)
        {

            if (state == "Initial")
            {
                HomeAIInitialPosition.Clear();
                HomeAIInitialRotation.Clear();
            AwayAIInitialPosition.Clear();
            AwayAIInitialRotation.Clear();
            //first find active homeplayer
            List<GameObject> activePlayers = new List<GameObject>();
                var HomePlayers = GameObject.FindGameObjectsWithTag("Home");
                foreach (var player in HomePlayers)
                {

                    if (player.GetComponent<AI_Avatar>().M_NCModel.isActivated == true)
                    {
                      
                        activePlayers.Add(player);
                    }

                 

                }

                for (int i = 0; i < HomeAICounter; i++)
                {
                
                    HomeAIInitialPosition.Add(activePlayers[i].transform.position);
                   HomeAIInitialRotation.Add(activePlayers[i].transform.eulerAngles);
                    HomePlayers[i].GetComponent<AI_Avatar>().Position.Add(HomeAIInitialPosition[i]);
                    HomePlayers[i].GetComponent<AI_Avatar>().Rotation.Add(HomeAIInitialRotation[i]);

                }

                //Now clear the list from home player and add the away player

                activePlayers.Clear();
                var AwayPlayers = GameObject.FindGameObjectsWithTag("Away");

                foreach (var player in AwayPlayers)
                {

                    if (player.GetComponent<AI_Avatar>().M_NCModel.isActivated == true)
                    {
                        activePlayers.Add(player);
                    }
                }

                for (int i = 0; i < AwayAICounter; i++)
                {
                 
                    AwayAIInitialPosition.Add(activePlayers[i].transform.position);
                    AwayAIInitialRotation.Add(activePlayers[i].transform.eulerAngles);
                    AwayPlayers[i].GetComponent<AI_Avatar>().Position.Add(AwayAIInitialPosition[i]);
                    AwayPlayers[i].GetComponent<AI_Avatar>().Rotation.Add(AwayAIInitialRotation[i]);
                }

                //clear state again
                activePlayers.Clear();
                state = "";
            }

            if (state == "Final")
            {

                HomeAIFinalPosition.Clear();
                HomeAIFinalRotation.Clear();
            AwayAIFinalPosition.Clear();
            AwayAIFinalRotation.Clear();
            //first find active homeplayer
            List<GameObject> activePlayers = new List<GameObject>();
                var HomePlayers = GameObject.FindGameObjectsWithTag("Home");
                foreach (var player in HomePlayers)
                {

                    if (player.GetComponent<AI_Avatar>().M_NCModel.isActivated == true)
                    {
                        activePlayers.Add(player);
                    }
                }

                for (int i = 0; i < HomeAICounter; i++)
                {
                    HomeAIFinalPosition.Add(activePlayers[i].transform.position);
                    HomeAIFinalRotation.Add(activePlayers[i].transform.eulerAngles);
                HomePlayers[i].GetComponent<AI_Avatar>().Position.Add(HomeAIFinalPosition[i]);
                HomePlayers[i].GetComponent<AI_Avatar>().Rotation.Add(HomeAIFinalRotation[i]);


            }

            //Now clear the list from home player and add the away player

            activePlayers.Clear();
                var AwayPlayers = GameObject.FindGameObjectsWithTag("Away");

                foreach (var player in AwayPlayers)
                {

                    if (player.GetComponent<AI_Avatar>().M_NCModel.isActivated == true)
                    {
                        activePlayers.Add(player);
                    }
                }

                for (int i = 0; i < AwayAICounter; i++)
                {
                    AwayAIFinalPosition.Add(activePlayers[i].transform.position);
                    AwayAIFinalRotation.Add(activePlayers[i].transform.eulerAngles);
                AwayPlayers[i].GetComponent<AI_Avatar>().Position.Add(AwayAIFinalPosition[i]);
                AwayPlayers[i].GetComponent<AI_Avatar>().Rotation.Add(AwayAIFinalRotation[i]);
            }

                //clear state again
                activePlayers.Clear();
                state = "";
            }






        }

        /// <summary>
        /// Saving scenario
        /// </summary>
       
    public void SaveScenario(int scenarioNum)
    {
        Debug.Log(filePath);
        var scenarioIndex = scenarioNum;
        var homeplayers = GameObject.FindGameObjectsWithTag("Home");
        var awayPlayers = GameObject.FindGameObjectsWithTag("Away");

        //var players = homeplayers.Concat(awayPlayers).ToArray();

        var scenarios = loadData();
        var scenario = new ScenarioData();
        scenario.ScenarioNumber = scenarioNum;
        scenario.addHomePlayers(homeplayers);
        scenario.addAwayPlayers(awayPlayers);
        scenarios.scenarios[scenarioIndex] = scenario;

        writeData(scenarios);
    }

    private ScenarioContainer loadData()
    {
        if (!File.Exists(filePath))
        {
            return new ScenarioContainer();
        }
        XmlSerializer serializer = new XmlSerializer(typeof(ScenarioContainer));
        StreamReader reader = new StreamReader(filePath);
        ScenarioContainer deserialized = (ScenarioContainer)serializer.Deserialize(reader.BaseStream);
        reader.Close();
        return deserialized;
    }
    private void writeData(ScenarioContainer scenarios)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            FileStream fileStream = File.Create(filePath);
            StreamWriter sw = new StreamWriter(fileStream, new System.Text.UTF8Encoding(false));
            XmlSerializer bf = new XmlSerializer(typeof(ScenarioContainer));
            bf.Serialize(sw, scenarios);
            sw.Close();
            fileStream.Close();
        }


    ////SCENARIO LOAD
    ///
    public void LoadScenario(int scenarioNum)
    {
        var scenarios = loadData();
        var scenario = scenarios.scenarios[scenarioNum];
        if (scenario == null)
        {
            Debug.Log("scenario" + scenarioNum.ToString() + " is empty.");
            return;
        }
        //currentScenarioNum = scenarioNum;
        OnChangeScenario?.Invoke(scenario);
        analyseScenario(scenario);
        SetPlayers(scenario);
    }
    private void analyseScenario(ScenarioData scenario)
    {

        //check current players number
        var homeplayers = findActivedPlayers("Home");
        var awayPlayers = findActivedPlayers("Away");

        if (scenario.homeplayers.Count != homeplayers.Length)
        {
            findDiffPlayers(homeplayers, scenario.homeplayers, true);
            AISock?.ChangeHomeTeamSize(scenario.homeplayers.Count);
        }
        if (scenario.awayplayers.Count != awayPlayers.Length)
        {
            findDiffPlayers(awayPlayers, scenario.awayplayers, false);
            AISock?.ChangeHomeTeamSize(scenario.awayplayers.Count);
        }

    }

    private void findDiffPlayers(GameObject[] currentPlayers, List<PlayerData> scenarioPlayers, bool isHomeTeam)
    {
        var currentPlayersName = currentPlayers.Select(player => player.name).ToArray();
        var scenarioPlayersName = scenarioPlayers.Select(player => player.name).ToArray();

        string[] playersNameDiff;
        if (scenarioPlayers.Count > currentPlayers.Length)
        {
            playersNameDiff = scenarioPlayersName.Except(currentPlayersName).ToArray();
            foreach (var playerName in playersNameDiff)
            {
                var playerData = scenarioPlayers.First(player => player.name == playerName);

                GameObject emptyGO = new GameObject();
                emptyGO.transform.position = new Vector3(-88.4151306f, 1.18533289f, 9.39408302f);
                //    emptyGO.transform.rotation = playerData.rotation;

                if (isHomeTeam)
                {
                    AISock?.UIActivateHomeAIByNum(emptyGO.transform, playerData.number);
                }
                else
                {
                   AISock?.UIActivateAwayAIByNum(emptyGO.transform, playerData.number);
                }
                Destroy(emptyGO);

            }
        }
        else
        {
            playersNameDiff = currentPlayersName.Except(scenarioPlayersName).ToArray();
            foreach (var playerName in playersNameDiff)
            {
                //TODO: go to the leave point and then disable networking
                var playerNumber = currentPlayers.First(player => player.name == playerName).GetComponent<AI_Avatar>().M_NCModel.number;
                if (isHomeTeam)
                {
                    AISock?.DisableHomeAIByNum(playerNumber);
                }
                else
                {
                    AISock?.DisableAwayAIByNum(playerNumber);
                }
            }
        }
    }
    private GameObject[] findActivedPlayers(string tag)
    {
        var allplayers = GameObject.FindGameObjectsWithTag(tag);
        List<GameObject> activedPlayers = new List<GameObject>();
        foreach (var player in allplayers)
        {
            if (player.GetComponent<AI_Avatar>().M_NCModel.isActivated == true)
            {
                activedPlayers.Add(player);
            }
        }
        return activedPlayers.ToArray();
    }


    private void SetPlayers(ScenarioData scenario)
    {

        foreach (var player in scenario.homeplayers)
        {
            var playerObject = GameObject.Find(player.name);

            OnChangePlayerPosition?.Invoke(playerObject, player.position[0], player.rotation[0]);
           playerObject.GetComponent<INT_ILinkedPinObject>().SetTransform(player.position[0], player.rotation[0]);
             playerObject.GetComponent<INT_ILinkedPinObject>().SetNavAgentDestination(player.position[0]);
           
        }
        foreach (var player in scenario.awayplayers)
        {
            var playerObject = GameObject.Find(player.name);
          OnChangePlayerPosition?.Invoke(playerObject, player.position[0], player.rotation[0]);
        
           playerObject.GetComponent<INT_ILinkedPinObject>().SetTransform(player.position[0], player.rotation[0]);
            playerObject.GetComponent<INT_ILinkedPinObject>().SetNavAgentDestination(player.position[0]);
        }

    }

    /// <summary>
    /// /CREATING AI
    /// </summary>
    private void CreateAwayAI()
        {
            AwayAICounter++;
            Vector3 temp = new Vector3(AwayAICounter, AwayTeamLocation.position.y, AwayTeamLocation.position.z);
            AwayTeamLocation.position = temp;
            AISock.UIActivateAwayAI(AwayTeamLocation);
        }

        private void CreateHomeAI()
        {
            HomeAICounter++;
            Vector3 temp = new Vector3(HomeAICounter, HomeTeamLocation.position.y, HomeTeamLocation.position.z);
            HomeTeamLocation.position = temp;
            AISock.UIActivateHomeAI(HomeTeamLocation);
        }

      



    }

