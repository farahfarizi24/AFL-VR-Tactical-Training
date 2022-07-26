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

    public Action<ScenarioData> OnChangeScenario;
    public Action<GameObject, Vector3, Quaternion> OnChangePlayerPosition;

    public int currentScenarioNum = 1;

    string filePath = Application.persistentDataPath + "/gamedate.txt";

    public void TestingSave()
    {
        SaveScenario(currentScenarioNum);
    }
    public void TestingLoad()
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
        OnChangeScenario?.Invoke(scenario);
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
    private void CreatePlayers(ScenarioContainer scenarios, int scenarioNum)
    {
        foreach (var scenario in scenarios.scenarios)
        {
            if (scenario.ScenarioNumber == scenarioNum)
            {
                foreach (var player in scenario.homeplayers)
                {
                    GameObject newPlayer = Instantiate(homePlayerPrefab);
                    player.initPlayer(newPlayer);

                }
                foreach (var player in scenario.awayplayers)
                {
                    GameObject newPlayer = Instantiate(awayPlayerPrefab);
                    player.initPlayer(newPlayer);
                }
            }
        }
    }
    // Set Position for existing players
    private void SetPlayers(ScenarioData scenario)
    {

        foreach (var player in scenario.homeplayers)
        {
            var playerObject = GameObject.Find(player.name);
            //player.initPlayer(playerObject);
            OnChangePlayerPosition?.Invoke(playerObject, player.position, player.rotation);
        }
        foreach (var player in scenario.awayplayers)
        {
            var playerObject = GameObject.Find(player.name);
            //player.initPlayer(playerObject);
            OnChangePlayerPosition?.Invoke(playerObject, player.position, player.rotation);
        }

    }

    //TODO: create and remove extra players, then move players to scenario setting
    private void initalizePlayers(ScenarioData scenario)
    {


    }
    private void analyseScenario()
    {
        var scenarios = loadData();
        var scenario = scenarios.scenarios[currentScenarioNum - 1];
        //check current players number
        var homeplayers = GameObject.FindGameObjectsWithTag("Home");
        var awayPlayers = GameObject.FindGameObjectsWithTag("Away");

        if (scenario.homeplayers.Count != homeplayers.Length)
        {
            findDiffPlayers(homeplayers, scenario.homeplayers);
        }
        if (scenario.awayplayers.Count != awayPlayers.Length)
        {
            findDiffPlayers(awayPlayers, scenario.awayplayers);
        }
    }
    private void findDiffPlayers(GameObject[] currentPlayers, List<PlayerData> scenarioPlayers)
    {
        var playersName = currentPlayers.Select(player => player.name).ToArray();
        var playersNameDiff = playersName.Except(scenarioPlayers.Select(player => player.name).ToArray()).ToArray();
        if (scenarioPlayers.Count > currentPlayers.Length)
        {
            //create new players using player data by a spawner
            foreach (var playerName in playersNameDiff)
            {
                var playerData = scenarioPlayers.First(player => player.name == playerName);
                var newPlayer = Instantiate(homePlayerPrefab);
                playerData.initPlayer(newPlayer);
            }
        }
        else
        {
            foreach (var playerName in playersNameDiff)
            {
                Destroy(currentPlayers.First(player => player.name == playerName));
            }
        }
    }
    public void TestMovePlayer()
    {
        var homeplayers = GameObject.FindGameObjectsWithTag("Home");
        foreach (var player in homeplayers)
        {
            //player.GetComponent< AI_PathManager>().SetPoints = destination;
            //player.GetComponent<AI_Avatar>().SetDestination(destination);

        }
    }
    private void MovePlayer(Vector3 destination, GameObject player)
    {
        //using the AI_Avatar script to move the player


        //player.GetComponent< AI_PathManager>().SetPoints = destination;
        //player.GetComponent<AI_Avatar>().SetDestination(destination);
    }
}
