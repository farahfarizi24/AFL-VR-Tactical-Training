using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.DU.CE.INT;
using com.DU.CE.LVL;
using UnityEngine.UI;
using com.DU.CE.AI;
using System.Xml.Serialization;
using System.IO;
using TMPro;
using System;
using System.Linq;


public class ScenarioCreation_Function : MonoBehaviour
    {
    [SerializeField]private int IsPlayerReady = 0;
    public bool ScenarioCompletedToggle = false;
    public bool ScenarioRunToggle = false;
        public SOC_AI AISock = null;
        public Transform HomeTeamLocation = null;
        public Transform AwayTeamLocation = null;
        private int AwayAICounter = 0;
        private int HomeAICounter = 0;
    string filePath;
    public RunScenario RunScenarioScript;
    public bool isReviewing;
    public GameObject ScenarioEditorSelection;


    /// <summary>
    ///  Under is the list of variable to be stored for each AI
    ///
    public Action<ScenarioData> OnChangeScenario;
 public List<GameObject> AIObject = new List<GameObject>();
    private List<Vector3> AwayAIInitialPosition = new List<Vector3>();
         private List<Vector3> AwayAIFinalPosition = new List<Vector3>();
     private List<Vector3> AwayAIInitialRotation = new List<Vector3>();
        private List<Vector3> AwayAIFinalRotation = new List<Vector3>();

       private List<Vector3> HomeAIInitialPosition = new List<Vector3>();
   private List<Vector3> HomeAIFinalPosition = new List<Vector3>();
     private List<Vector3> HomeAIInitialRotation = new List<Vector3>();
      private List<Vector3> HomeAIFinalRotation = new List<Vector3>();
    public Action<GameObject, Vector3, Vector3> OnChangePlayerPosition;

    private List<String> AwayAIRole = new List<string>();
        private List<String> HomeAIRole = new List<string>();
        //Role FB, BP HB, CB, FF, FP, HF, CHF, MF (these are the recognised string

        public int ScenarioNumber;
        public int BallTargetAINum;//denotes which AI is the target for final ball position,
                                   //this will be counted the same with their Counter number

    public ScenarioCreation_UI UICreationScript;
    public GameObject ScenarioEditorObj;
    /// <summary>
    /// Button 
    /// </summary>;
    public Button BackButton;
    public Button ReviewButton;
    public Button HomeCreate;
        public Button AwayCreate;
        public Button SaveFinalPosition;
        public Button SaveInitialPosition;
        public Button RunScenarioBTN;
        public Button BallTarget;
    public Button MarkPlayer;
   // public Button MarkerPin;
    public Button SaveEntireScenario;
    public bool BallTargetting;
    public bool PlayerReferenceTarrgetting;
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
            RunScenarioBTN.onClick.AddListener(QuickRunScenario);
            BallTarget.onClick.AddListener(SetBallTarget);
            MarkPlayer.onClick.AddListener(SetMarkerPlayer);
        BackButton.onClick.AddListener(BackToSelection);
        ReviewButton.onClick.AddListener(ReviewMode);
            SaveEntireScenario.onClick.AddListener(delegate { SaveScenario(ScenarioNumber); });

        BallTargetting = false;
        PlayerReferenceTarrgetting = false;

        }


    #region Back to Selection

    public void BackToSelection()
    {
        ScenarioEditorObj.SetActive(false);
        UICreationScript.CreateNewScenario();
    }

    public void ReviewMode()
    {
        isReviewing = true;
        LoadScenario(ScenarioNumber);

        ScenarioRunToggle = true;
    }

    #endregion
    #region Marker Player

    public void SetMarkerPlayer()
    {
        if (PlayerReferenceTarrgetting!= true)
        {
            ResetPlayerMarker();
            PlayerReferenceTarrgetting = true;
            MarkPlayer.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
        }
        else
        {
            //Reset the marker symbol on top of player//
            PlayerReferenceTarrgetting = false;
            MarkPlayer.GetComponent<Image>().color = new Color32(0, 43, 92, 255);
        }
    }

    public void ResetPlayerMarker()
    {
        var homeplayers = GameObject.FindGameObjectsWithTag("Home");
        var awayPlayers = GameObject.FindGameObjectsWithTag("Away");

        foreach (var player in homeplayers)
        {
            player.GetComponent<AI_Avatar>().ResetPlayerReference();
        }
        foreach (var player in awayPlayers)
        {
            player.GetComponent<AI_Avatar>().ResetPlayerReference();
        }
    }
    #endregion







    public void SetBallTarget()
        {
        if (BallTargetting != true)
        {
            ResetBallTarget();
            BallTargetting = true;
            BallTarget.GetComponent<Image>().color = new Color32(0,0,0,255);
        }
        else
        {//remove all the ball highlight from other players

            ResetPlayerHighlight();
            BallTargetting = false;
            BallTarget.GetComponent<Image>().color = new Color32(0,43,92,255);
        }
        }

    public void ResetPlayerHighlight()
    {
        var homeplayers = GameObject.FindGameObjectsWithTag("Home");
        var awayPlayers = GameObject.FindGameObjectsWithTag("Away");

        foreach (var player in homeplayers)
        {
            player.GetComponent<AI_Avatar>().UnsetHighlight();
        }
        foreach (var player in awayPlayers)
        {
            player.GetComponent<AI_Avatar>().UnsetHighlight();
        }
    }

    public void SetPlayerHighlight()
    {
        var homeplayers = GameObject.FindGameObjectsWithTag("Home");
        var awayPlayers = GameObject.FindGameObjectsWithTag("Away");

        foreach (var player in homeplayers)
        {
            
            if (player.GetComponent<AI_Avatar>().BallReceiver == true)
            {
                player.GetComponent<AI_Avatar>().UnlinkedSetHighlight();
                Debug.Log("Set highlight");
            }

            if (player.GetComponent<AI_Avatar>().IsPositionReference == true)
            {
                player.GetComponent<AI_Avatar>().NonlinkedPlayerReference();
            }
        }
        foreach (var player in awayPlayers)
        {
          
            if (player.GetComponent<AI_Avatar>().BallReceiver == true)
            {
                player.GetComponent<AI_Avatar>().UnlinkedSetHighlight();
                Debug.Log("Set highlight");
            }

            if (player.GetComponent<AI_Avatar>().IsPositionReference == true)
            {
                player.GetComponent<AI_Avatar>().NonlinkedPlayerReference();
            }
        }
    }
    public void ResetBallTarget()
    {
        var homeplayers = GameObject.FindGameObjectsWithTag("Home");
        var awayPlayers = GameObject.FindGameObjectsWithTag("Away");

        foreach(var player in homeplayers)
        {
            player.GetComponent<AI_Avatar>().ResetBallReceiver();
        }
        foreach(var player in awayPlayers)
        {
            player.GetComponent<AI_Avatar>().ResetBallReceiver();
        }
      
    }

        public void QuickRunScenario()
        {
       
        LoadScenario(ScenarioNumber);
      
        ScenarioRunToggle = true; 
    
    
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
                if (HomePlayers[i].GetComponent<AI_Avatar>().AvatarPosition.Count != 0)
                {
                    HomePlayers[i].GetComponent<AI_Avatar>().AvatarPosition[0] = HomeAIInitialPosition[i];
                    HomePlayers[i].GetComponent<AI_Avatar>().AvatarRotation[0] = HomeAIInitialRotation[i];

                }
                else
                {

                    HomePlayers[i].GetComponent<AI_Avatar>().AvatarPosition.Add(HomeAIInitialPosition[i]);
                    HomePlayers[i].GetComponent<AI_Avatar>().AvatarRotation.Add(HomeAIInitialRotation[i]);
                }
               

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
                if (AwayPlayers[i].GetComponent<AI_Avatar>().AvatarPosition.Count() !=0)
                {
                    AwayPlayers[i].GetComponent<AI_Avatar>().AvatarPosition[0] = AwayAIInitialPosition[i];
                    AwayPlayers[i].GetComponent<AI_Avatar>().AvatarRotation[0] = AwayAIInitialRotation[i];

                }
                else
                {
                    AwayPlayers[i].GetComponent<AI_Avatar>().AvatarPosition.Add(AwayAIInitialPosition[i]);
                    AwayPlayers[i].GetComponent<AI_Avatar>().AvatarRotation.Add(AwayAIInitialRotation[i]);
                }


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

                if (HomePlayers[i].GetComponent<AI_Avatar>().AvatarPosition.Count() > 1)
                {
                     HomePlayers[i].GetComponent<AI_Avatar>().AvatarPosition[1] = HomeAIFinalPosition[i];
                   HomePlayers[i].GetComponent<AI_Avatar>().AvatarRotation[1] = HomeAIFinalRotation[i];
                

                }
                else
                {
                    HomePlayers[i].GetComponent<AI_Avatar>().AvatarPosition.Add(HomeAIFinalPosition[i]);
                    HomePlayers[i].GetComponent<AI_Avatar>().AvatarRotation.Add(HomeAIFinalRotation[i]);
                }



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

                if (AwayPlayers[i].GetComponent<AI_Avatar>().AvatarPosition.Count() > 1)
                {
                    AwayPlayers[i].GetComponent<AI_Avatar>().AvatarPosition[1] = AwayAIFinalPosition[i];
                    AwayPlayers[i].GetComponent<AI_Avatar>().AvatarRotation[1] = AwayAIFinalRotation[i];
    
                }
                else
                {

                    AwayPlayers[i].GetComponent<AI_Avatar>().AvatarPosition.Add(AwayAIFinalPosition[i]);
                    AwayPlayers[i].GetComponent<AI_Avatar>().AvatarRotation.Add(AwayAIFinalRotation[i]);
                }
                  
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
        else
        {
            Debug.Log("scenario" + scenarioNum.ToString() + " is loaded");
        }
        //currentScenarioNum = scenarioNum;
        OnChangeScenario?.Invoke(scenario);
        analyseScenario(scenario);

        SetPlayers(scenario);
    }

    public void ScenarioLookup(int scenarioNum)
    {
        var scenarios = loadData();
        var scenario = scenarios.scenarios[scenarioNum];
        if (scenario == null)
        {
            Debug.Log("scenario" + scenarioNum.ToString() + " is not existed.");
            RunScenarioScript.AddScenarioToQueue(scenarioNum, false);
            return;
        }
        else
        {
            RunScenarioScript.AddScenarioToQueue(scenarioNum, true);
            Debug.Log("scenario" + scenarioNum.ToString() + " is existed");
            return;
        }
    }

    //This function analyse how many active players are present and load them into each avatar
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
        
        foreach (var player in scenario.homeplayers)
        {
            var playerObject = GameObject.Find(player.name);

            playerObject.GetComponent<INT_ILinkedPinObject>().SetScenarioTransform(
                player.position[0], player.rotation[0], player.position[1], player.rotation[1]
                );

            playerObject.GetComponent<AI_Avatar>().IsPositionReference = player.PlayerReference;
            playerObject.GetComponent<AI_Avatar>().BallReceiver = player.BallReceiver;
            if (ScenarioEditorObj.activeSelf == true)
            {
                SetPlayerHighlight();
            }

        }
        foreach (var player in scenario.awayplayers)
        {
            var playerObject = GameObject.Find(player.name);
            playerObject.GetComponent<INT_ILinkedPinObject>().SetScenarioTransform(
          player.position[0], player.rotation[0], player.position[1], player.rotation[1]

         );
            playerObject.GetComponent<AI_Avatar>().IsPositionReference = player.PlayerReference;
            playerObject.GetComponent<AI_Avatar>().BallReceiver = player.BallReceiver;
            if (ScenarioEditorObj.activeSelf == true)
            {
                SetPlayerHighlight();
            }
            
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
        AwayAICounter = 0;
        HomeAICounter = 0;
        AIObject.Clear();
        foreach (var player in scenario.homeplayers)
        {
            var playerObject = GameObject.Find(player.name);

           OnChangePlayerPosition?.Invoke(playerObject, player.position[0], player.rotation[0]);
            playerObject.GetComponent<INT_ILinkedPinObject>().SetInitPosition();
            HomeAICounter += 1;
            AIObject.Add(playerObject);
        }
        foreach (var player in scenario.awayplayers)
        {
            var playerObject = GameObject.Find(player.name);
        OnChangePlayerPosition?.Invoke(playerObject, player.position[0], player.rotation[0]);


            playerObject.GetComponent<INT_ILinkedPinObject>().SetInitPosition();
            AwayAICounter += 1;
            AIObject.Add(playerObject);
            
        }
     //Uncomment to make scenario automatically being run
      //  ScenarioRunToggle = true;
       

    }




    private void Update()
    {
        if (ScenarioRunToggle)
        {
            CheckIfAIReady();
            //you want to check index on whether they have finished the prep or not
        }
        if (ScenarioCompletedToggle)
        {
            CheckIfScenarioFinished();
        }

    }
    //When run button is pressed


    #region Scenario Run Methods
    private void CheckIfScenarioFinished()
    {
        IsPlayerReady = 0;


        for (int i = 0; i < AIObject.Count(); i++)
        {
            if (AIObject[i].GetComponent<AI_Avatar>().NavMeshCount == true)
            {
                IsPlayerReady = IsPlayerReady + 1;
            }


        }

        if (IsPlayerReady == AIObject.Count())
        {
            ScenarioCompletedToggle=false;
            StartCoroutine(PerformFinalCountdown());
            LVL_SoundManager.PlayMusic("EndWhistle");
            

        }

    }

    //This method is run to put the final timer before scenario is finished

    float ActionCountdown=10;
    public GameObject TimerTxt;
    
    IEnumerator PerformFinalCountdown()
    {
        yield return new WaitForSeconds(5);

        foreach (var player in AIObject)
        {
            var playerObject = GameObject.Find(player.name);

          
            playerObject.GetComponent<AI_Avatar>().isScenarioRunning = false;
            isReviewing = false;
        }
            /* if (TimerTxt.activeSelf != true)
             {

                 TimerTxt.SetActive(true);
                 for (ActionCountdown = 10; ActionCountdown > 0; ActionCountdown -= Time.deltaTime)
                 {
                     string time = ActionCountdown.ToString("F1");
                     string text = "Time left: " + time;
                    // TimerTxt.GetComponent<TextMeshProUGUI>().SetText(text);

                     yield return null;
                 }
                 TimerTxt.SetActive(false);*/
            AnalysePerformance();


    }

    private void AnalysePerformance()
    {
        Debug.Log("Is ball being held by incorrect person");
       
        SetPlayerHighlight();
        
    }

    #endregion

    #region Initial position methods
    private void CheckIfAIReady()
    {
        IsPlayerReady = 0;
        for (int i = 0; i < AIObject.Count(); i++)
        {
            if (AIObject[i].GetComponent<AI_Avatar>().NavMeshCount == true)
            {
                IsPlayerReady = IsPlayerReady + 1;
            }
         
           
        }

        if (IsPlayerReady == AIObject.Count())
        {
            DeployScenario();
            ScenarioRunToggle = false;
        }
    }
    private void DeployScenario()
    {
        StartCoroutine(StartScenarioCoroutine());
        
    }

    IEnumerator StartScenarioCoroutine()
    {
        yield return new WaitForSeconds(3);
        Debug.Log("Start Scenario Coroutine");
        LVL_SoundManager.PlayMusic("StartWhistle");
        foreach (var player in AIObject)
        {
            var playerObject = GameObject.Find(player.name);

         OnChangePlayerPosition?.Invoke(playerObject, player.GetComponent<AI_Avatar>().AvatarPosition[1], 
             player.GetComponent<AI_Avatar>().AvatarRotation[1]);
            playerObject.GetComponent<INT_ILinkedPinObject>().SetFinalPosition();
            if (!isReviewing)
            playerObject.GetComponent<AI_Avatar>().isScenarioRunning = true;
      
        }
        ScenarioCompletedToggle = true;
  

        
        
    }

    #endregion
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

