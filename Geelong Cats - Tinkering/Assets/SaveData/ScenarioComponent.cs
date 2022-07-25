using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
public class ScenarioComponent : MonoBehaviour
{
    public GameObject homePlayerPrefab;
    public GameObject awayPlayerPrefab;

    string filePath;

    void Awake()
    {
        //update the field once the persistent path exists.
        //

        ///This means it reads file called gamedata
        filePath = Application.persistentDataPath + "/gamedate.txt";
        Debug.Log(filePath);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void FindAllPlayer()
    {
        var homeplayers = GameObject.FindGameObjectsWithTag("Home");

        var awayPlayers = GameObject.FindGameObjectsWithTag("Away");

        var players = homeplayers.Concat(awayPlayers).ToArray();

        var scenarios = new ScenarioContainer();
        var scenario = new ScenarioData();
        scenario.ScenarioNumber = 1;
        scenario.addHomePlayers(homeplayers);
        scenarios.scenarios.Add(scenario);
        var scenario2 = new ScenarioData();
        scenario2.ScenarioNumber = 2;
        scenario2.addAwayPlayers(awayPlayers);
        scenarios.scenarios.Add(scenario2);

        writeData(scenarios);
    }
    public void LoadScenario()
    {
        var scenarios = loadData();
        clearnPlayers();
        SetPlayers(scenarios, 1);
        CreatePlayers(scenarios, 2);

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
    private void SetPlayers(ScenarioContainer scenarios, int scenarioNum)
    {

        foreach (var scenario in scenarios.scenarios)
        {
            if (scenario.ScenarioNumber == scenarioNum)
            {
                foreach (var player in scenario.homeplayers)
                {
                    var playerObject = GameObject.Find(player.name);
                    player.initPlayer(playerObject);
                }
                foreach (var player in scenario.awayplayers)
                {
                    var playerObject = GameObject.Find(player.name);
                    player.initPlayer(playerObject);
                }
            }
        }
    }
    private void clearnPlayers()
    {
        var homeplayers = GameObject.FindGameObjectsWithTag("Home");

        var awayPlayers = GameObject.FindGameObjectsWithTag("Away");

        var players = homeplayers.Concat(awayPlayers).ToArray();

        foreach (var player in players)
        {
            Destroy(player);
        }
    }

}
