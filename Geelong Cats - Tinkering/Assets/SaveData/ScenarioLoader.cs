using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScenarioLoader : MonoBehaviour
{

    struct CharacterAI
    {

        float[] XPosition;
        float[] YPosition;
        float[] ZPosition;
        float[] XRotation;
        float[] YRotation;
        float[] ZRotation;

    }

    public int ScenarioNumber = 0; //denote what saved scenario number is currently running
    public int PositionNumber = 0; //denote how many position is within one saved scenario

    public void LoadData()
        
        {
        string readFromFilePath = Application.persistentDataPath + "/SaveData/Scenario/" + "scenario_" + ScenarioNumber + ".txt";

        //NOW you want to check if it denote a new character. Maybe everytime it says "character" it'll add new character into struct


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
