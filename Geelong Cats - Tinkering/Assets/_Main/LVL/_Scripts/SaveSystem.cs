using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using com.DU.CE.AI;
using com.DU.CE.INT;
using System;
using System.IO;
using System.Linq;
public class SaveSystem : MonoBehaviour
{
 //  public ScenarioManager Savemanager;
    public string UserFilePath;
    // Start is called before the first frame update
    void Start()
    {

        String DateTime = System.DateTime.UtcNow.ToString("HH_mm_ff_dd_MMMMM");
        UserFilePath = Application.persistentDataPath + "/" + DateTime + "_userdata.txt";

        if (!File.Exists(UserFilePath))
        {

            File.WriteAllText(UserFilePath, "Current time: " + DateTime);
            Debug.Log(UserFilePath);
        }
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(SaveCoroutine());
    }
    IEnumerator SaveCoroutine()
    {
        yield return new WaitForSeconds(1f);
        SavePerSecond();
    }


    public void SavePerSecond()
    {
        if (File.Exists(UserFilePath))
        {
            GameObject[] playerObject = GameObject.FindGameObjectsWithTag("UserPlayer");
            Vector3 vector = new Vector3(0, 0, 0);
            for (int i = 0; i < playerObject.Length; i++)
            {
                String DateTime = System.DateTime.UtcNow.ToString("HH_mm_ff_dd_MMMMM");
                File.AppendAllText(UserFilePath, "User:" + i);
                File.AppendAllText(UserFilePath, "Time:" + DateTime);
                vector = playerObject[i].transform.position;
                File.AppendAllText(UserFilePath, "Position:" + vector.ToString("F3"));
                vector = playerObject[i].transform.rotation.eulerAngles;
                File.AppendAllText(UserFilePath, "Rotation:" + vector.ToString("F3"));

            }

        }
    }
}
