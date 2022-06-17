using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class ScenarioSave : MonoBehaviour
{
    public int ScenarioNumber = 0; //denote what saved scenario number is currently running
    public int PositionNumber = 0; //denote how many position is in that scenario

    //Create a field of this class for the file

    string saveFile;

    void Awake()
    {
        //update the field once the persistent path exists.
        //

        ///This means it reads file called gamedata
        saveFile = Application.persistentDataPath + "/gamedate.json";
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
