using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using com.DU.CE.USER;

public class ScenarioCreation_UI : MonoBehaviour
{
    public GameObject ScenarioEditor = null;
    public Button[] ScenarioButton = new Button[6];
    public USER_UISwitcher UI;


    // Start is called before the first frame update
    void Start()
    {
        // listening the button click event
        foreach (Button btn in ScenarioButton)
        {
            Button SC1 = btn.GetComponent<Button>();
            SC1.onClick.AddListener(ScenarioTask);
        }

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

            //Turn off current gameobject
        }
        ScenarioEditor.SetActive(true);
        UI.ToggleUI(false);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
