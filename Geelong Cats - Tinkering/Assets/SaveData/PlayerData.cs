using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    //TODO: pathPoints using Queue<Vector3>
    [XmlElement("position")]
    public Vector3 position;

    [XmlElement("rotation")]
    public Quaternion rotation;


    [XmlAttribute("name")]
    public string name;


    public void getDataFromPlayer(GameObject player)
    {
        position = player.transform.position;
        rotation = player.transform.rotation;
        name = player.name;
    }
    public void initPlayer(GameObject player)
    {
        player.transform.position = position;
        player.transform.rotation = rotation;
        player.name = name;
    }

}
