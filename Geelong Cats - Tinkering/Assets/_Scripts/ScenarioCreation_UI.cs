using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using com.DU.CE.USER;
using System;

public class ScenarioCreation_UI : MonoBehaviour
{
    public GameObject ScenarioEditor = null;
    public GameObject FieldViewMenu;
    public GameObject boardObject = null;
    public GameObject boardContainer;
    public int scenario=0;
    public Button CreateScenario;
    public Button LoadScenario;
    public Button BackButton;
    public Button selectButton;
    public Button FieldViewButton;
    public Button ActivateUIButton;
    public Dropdown ScenarioSelector;

    [SerializeField] private int state;//1 = create nnew scenario, 2 = load
    public GameObject ScenarioMenuObj;
    public GameObject SelectScenarioMenuObj;
    public GameObject ScenarioEditorObj;
    public GameObject Logo;
    //public Button[] ScenarioButton = new Button[6];
   // public USER_UISwitcher UI;
    List<string> scenarios = new List<string>();
   public int TotalScenarioNumber=20;


    // Start is called before the first frame update
    void Start()
    {
        Logo.SetActive(true);
        state = 0;
        CreateScenario.onClick.AddListener(CreateNewScenario);
        LoadScenario.onClick.AddListener(LoadSavedScenario);
        BackButton.onClick.AddListener(BackButtonEvent);
        FieldViewButton.onClick.AddListener(FieldViewEvent);
        selectButton.onClick.AddListener(SelectScenario);
        ActivateUIButton.onClick.AddListener(activateUI);
        //Two methods below is what actually create the scene according to the number user selected

        FieldViewMenu.SetActive(false);
        ScenarioMenuObj.SetActive(true);
        SelectScenarioMenuObj.SetActive(false);
        for (int i = 0; i<TotalScenarioNumber; i++)
        {
            int scenarionum = i + 1;
            scenarios.Add("Scenario " + scenarionum);
        }

        //clear options from the menu
        ScenarioSelector.ClearOptions();
        
            ScenarioSelector.AddOptions(scenarios);
        
    }

    private void activateUI()
    {
        gameObject.GetComponent<Image>().enabled = true;
        //set from beginning again;
        FieldViewMenu.SetActive(false);
        ScenarioMenuObj.SetActive(true);
        SelectScenarioMenuObj.SetActive(false);
        Logo.SetActive(true);
        FieldViewMenu.SetActive(true);
       
    }

    private void SelectScenario()
    {
      
        Logo.SetActive(false);
        //check whawt number of scenario is being selected
        //check state, if it's 1 create, or 2 load
        scenario = ScenarioSelector.value;
        if (state == 1)
        {
            ScenarioEditorObj.SetActive(true);
            SelectScenarioMenuObj.SetActive(false);
            boardObject = GameObject.FindGameObjectWithTag("board");
            boardObject.transform.SetParent(boardContainer.gameObject.transform);//make it current game object child
            boardObject.transform.localPosition = new Vector3(0.0f, 0.0f,-3.0f);
            //create scenario according to the current  index, so need to pass the value of which index it is
            //Also show the menu for scenario creation
        }
        else if (state == 2)
        {
            //loadscenario according to the current  index, so need to pass the value of which index it is
        }


    }

    private void FieldViewEvent()
    {
        FieldViewMenu.SetActive(false);
        Logo.SetActive(false);
        ScenarioMenuObj.SetActive(false);
        SelectScenarioMenuObj.SetActive(false);
        FieldViewMenu.SetActive(true);
        gameObject.GetComponent<Image>().enabled=false;
  
    }

    private void BackButtonEvent()
    {
        Logo.SetActive(true);
        ScenarioMenuObj.SetActive(true);
        SelectScenarioMenuObj.SetActive(false);
        state = 0;
    }

    private void LoadSavedScenario()
    {
        Logo.SetActive(false);
        ScenarioMenuObj.SetActive(false);
        SelectScenarioMenuObj.SetActive(true);
        state = 2;
        //Set trained player
        FieldViewEvent();
    }

    private void CreateNewScenario()
    {
        Logo.SetActive(false);
        ScenarioMenuObj.SetActive(false);
        SelectScenarioMenuObj.SetActive(true);
        state = 1;

    }

   // void ScenarioTask()
   // {
        //ScenarioEditor = GameObject.FindGameObjectWithTag("ScenarioEditor");
        //ScenarioEditor.gameObject.SetActive(true);
        /*Debug.Log("ButtonClick");
        if (ScenarioEditor == null)
        {

            ScenarioEditor = GameObject.FindWithTag("ScenarioEditor");
            ScenarioEditor = ScenarioEditor.transform.GetChild(0).gameObject;

            //Turn off current gameobject
        }
        ScenarioEditor.SetActive(true);
        UI.ToggleUI(false);*/
   // }
    // Update is called once per frame
    void Update()
    {

    }
}
