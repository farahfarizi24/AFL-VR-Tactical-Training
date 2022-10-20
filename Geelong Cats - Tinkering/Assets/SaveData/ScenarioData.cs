using com.DU.CE.AI;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using System;
[XmlRoot("scenarioContainer")]
public class ScenarioContainer
{
    [XmlArray("scenarios"), XmlArrayItem("scenario")]
    public ScenarioData[] scenarios = new ScenarioData[20];

}

public class ScenarioData
{
    public int ScenarioNumber = 0; //denote what saved scenario number is currently running
    public int PositionNumber = 0; //denote how many position is in that scenario ???

    [XmlArray("homeplayers"), XmlArrayItem("player")]
    public List<PlayerData> homeplayers = new List<PlayerData>();
    [XmlArray("awayplayers"), XmlArrayItem("player")]
    public List<PlayerData> awayplayers = new List<PlayerData>();




    public void addHomePlayers(GameObject[] players)
    {
        addPlayers(players, this.homeplayers);
    }
    public void addAwayPlayers(GameObject[] players)
    {
        addPlayers(players, this.awayplayers);
    }


  

    private void addPlayers(GameObject[] players, List<PlayerData> targetList)
    {
        foreach (GameObject player in players)
        {
            if (player.GetComponent<AI_Avatar>().M_NCModel.isActivated == true)
            {
                var dataPlayer = new PlayerData();
                dataPlayer. getDataFromPlayer(player);
                targetList.Add(dataPlayer);
            }

        }
    }
}
