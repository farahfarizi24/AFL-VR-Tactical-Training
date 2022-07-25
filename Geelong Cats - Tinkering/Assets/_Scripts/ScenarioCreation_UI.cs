using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ScenarioCreation_UI : MonoBehaviour
{
    public GameObject ScenarioEditor = null;
    public Button[] ScenarioButton = new Button[6];
    

    // Start is called before the first frame update
    void Start()

    {
        Button SC1 = ScenarioButton[0].GetComponent<Button>();
        SC1.onClick.AddListener(ScenarioTask);

        Button SC2 = ScenarioButton[1].GetComponent<Button>();
        SC2.onClick.AddListener(ScenarioTask);

    }

    void ScenarioTask()
    {
        //ScenarioEditor = GameObject.FindGameObjectWithTag("ScenarioEditor");
        //ScenarioEditor.gameObject.SetActive(true);
        Debug.Log("ButtonClick");
        if (ScenarioEditor == null)
        {

            ScenarioEditor = GameObject.FindWithTag("ScenarioEditor");
            ScenarioEditor = ScenarioEditor.transform.GetChild(0).gameObject;
            ScenarioEditor.SetActive(true);
            //Turn off current gameobject
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
