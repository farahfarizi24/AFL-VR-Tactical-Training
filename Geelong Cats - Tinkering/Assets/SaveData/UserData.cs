using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

[System.Serializable]
public class UserData
{
    // Start is called before the first frame update

    [XmlElement("UserPosition")]
    public List<Vector3> position = new List<Vector3>();

    [XmlElement("UserRotation")]
    public List<Vector3> rotation = new List<Vector3>();

    [XmlElement("GrabButtonPress_Right")]
    public bool GrabButtonPress_Right;
    [XmlElement("GrabButtonPress_Left")]
    public bool GrabButtonPress_Left;

    [XmlElement("TriggerButtonPress_Left")]
    public bool TriggerButtonPress_Left;

    [XmlElement("TriggerButtonPress_Right")]
    public bool TriggerButtonPress_Right;


    [XmlElement("Teleport")]
    public bool TeleportButton;

    [XmlElement ("MoveButton")]
    public float MoveButton;

    [XmlElement("RotateButton")]
    public float RotateButton;


    public void WriteData(GameObject Userplayer)
    {
  
    }
}
