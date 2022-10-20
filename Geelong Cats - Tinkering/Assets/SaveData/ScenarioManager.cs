using com.DU.CE.AI;
using com.DU.CE.INT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

[CreateAssetMenu(menuName = "Scenario/manager")]
public class ScenarioManager : ScriptableObject
{
    public GameObject homePlayerPrefab;
    public GameObject awayPlayerPrefab;

    public SOC_AI soc_ai_instance;

    public Action<ScenarioData> OnChangeScenario;
    public Action<GameObject, Vector3, Quaternion> OnChangePlayerPosition;

    public int currentScenarioNum = 1;

    string filePath;
    private void OnEnable()
    {
        filePath = Application.persistentDataPath + "/gamedate.txt";
        Debug.Log(filePath);
    }
    public void TestingSave()
    {
        SelecetScenario(1);
        SaveScenario();
    }
    public void TestingLoad()
    {
        SelecetScenario(1);
        LoadScenario();
    }
    public void SaveScenario()
    {
        SaveScenario(currentScenarioNum);
    }
    public void LoadScenario()
    {
        LoadScenario(currentScenarioNum);
    }
    public void SelecetScenario(int scenarioNum)
    {
        currentScenarioNum = scenarioNum;
    }

    public void SaveScenario(int scenarioNum)
    {
        var scenarioIndex = scenarioNum - 1;
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
    public void LoadScenario(int scenarioNum)
    {
        var scenarios = loadData();
        var scenario = scenarios.scenarios[scenarioNum - 1];
        if (scenario == null)
        {
            Debug.Log("scenario" + scenarioNum.ToString() + " is empty.");
            return;
        }
        currentScenarioNum = scenarioNum;
        OnChangeScenario?.Invoke(scenario);
        analyseScenario(scenario);
        SetPlayers(scenario);
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

    // Set Position for existing players
    private void SetPlayers(ScenarioData scenario)
    {

        foreach (var player in scenario.homeplayers)
        {
            var playerObject = GameObject.Find(player.name);

         //   OnChangePlayerPosition?.Invoke(playerObject, player.position, player.rotation);
         //   playerObject.GetComponent<INT_ILinkedPinObject>().SetNavAgentDestination(player.position);
        }
        foreach (var player in scenario.awayplayers)
        {
            var playerObject = GameObject.Find(player.name);
           // OnChangePlayerPosition?.Invoke(playerObject, player.position, player.rotation);
         //   playerObject.GetComponent<INT_ILinkedPinObject>().SetNavAgentDestination(player.position);
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

    private void analyseScenario(ScenarioData scenario)
    {

        //check current players number
        var homeplayers = findActivedPlayers("Home");
        var awayPlayers = findActivedPlayers("Away");

        if (scenario.homeplayers.Count != homeplayers.Length)
        {
            findDiffPlayers(homeplayers, scenario.homeplayers, true);
            soc_ai_instance?.ChangeHomeTeamSize(scenario.homeplayers.Count);
        }
        if (scenario.awayplayers.Count != awayPlayers.Length)
        {
            findDiffPlayers(awayPlayers, scenario.awayplayers, false);
            soc_ai_instance?.ChangeHomeTeamSize(scenario.awayplayers.Count);
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
                    soc_ai_instance?.UIActivateHomeAIByNum(emptyGO.transform, playerData.number);
                }
                else
                {
                    soc_ai_instance?.UIActivateAwayAIByNum(emptyGO.transform, playerData.number);
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
                    soc_ai_instance?.DisableHomeAIByNum(playerNumber);
                }
                else
                {
                    soc_ai_instance?.DisableAwayAIByNum(playerNumber);
                }
            }
        }
    }
    public void TestMovePlayer()
    {
        var homeplayers = findActivedPlayers("Home");
        foreach (var player in homeplayers)
        {
            //This is world position
            player.GetComponent<INT_ILinkedPinObject>().SetNavAgentDestination(new Vector3(0, 0, 0));
        }
    }
    public void TestCreatePlayer()
    {
        GameObject emptyGO = new GameObject();

        soc_ai_instance?.UIActivateHomeAI(emptyGO.transform);
        Destroy(emptyGO);

    }
}
