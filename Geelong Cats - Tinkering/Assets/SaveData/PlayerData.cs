using com.DU.CE.AI;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    //TODO: pathPoints using Queue<Vector3>
    [XmlElement("position")]
    public List <Vector3> position = new List<Vector3>();

    [XmlElement("rotation")]
    public List <Vector3 >rotation = new List<Vector3>();

    [XmlElement("BallReceiver")]
    public bool BallReceiver;
    /// <summary>
    /// Haven't been implemented
    /// </summary>
    [XmlAttribute("role")]
    public string role;

    [XmlAttribute("name")]
    public string name;

    [XmlAttribute("number")]
    public int number;


    public void getDataFromPlayer(GameObject player)
    {//Now you want to make sure the player has a list of position and rotation

        BallReceiver = player.GetComponent<AI_Avatar>().BallReceiver;
        for(int i = 0; i < 2; i++)
        {
            position.Add(player.GetComponent<AI_Avatar>().AvatarPosition[i]);
            rotation.Add(player.GetComponent<AI_Avatar>().AvatarRotation[i]);
        }
  
        name = player.name;
        number = player.GetComponent<AI_Avatar>().M_NCModel.number;
    }

 
}
