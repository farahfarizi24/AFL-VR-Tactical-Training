using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using com.DU.CE.USER;
using System;

public class ScenarioCreation_UI : MonoBehaviour
{

    public GameObject FieldViewMenu;
    public GameObject boardObject = null;
    public GameObject boardContainer;
    public int scenario=0;
    public Button CreateScenario;
    public Button LoadScenario;
    public Button BackButton;
    public Button selectButton;
    //this button is the button not the menu
    public Button FieldViewButton;
    //this one is the one being shown during field view
    public Button ActivateUIButton;
    public Dropdown ScenarioSelector;

    [SerializeField] private int state;//1 = create nnew scenario, 2 = load
    public GameObject ScenarioMenuObj;
    public GameObject SelectScenarioMenuObj;
    public GameObject ScenarioEditorObj;
    public GameObject RunScenarioMenu;
    public GameObject Logo;
    //public Button[] ScenarioButton = new Button[6];
   // public USER_UISwitcher UI;
    public List<string> scenarios = new List<string>();
   public int TotalScenarioNumber=20;
    public List <bool> LastUILayout = new List<bool>();

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

        //load the value from the list

        ScenarioMenuObj.SetActive(LastUILayout[0]);
        SelectScenarioMenuObj.SetActive(LastUILayout[1]);
        ScenarioEditorObj.SetActive(LastUILayout[2]);
        Logo.SetActive(LastUILayout[3]);
        LastUILayout.Clear();

        gameObject.GetComponent<Image>().enabled = true;
        FieldViewMenu.SetActive(false);
        FieldViewButton.gameObject.SetActive(true);
       
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
            //pass scenarionumber
            ScenarioEditorObj.GetComponent<ScenarioCreation_Function>().ScenarioNumber = scenario;
            SelectScenarioMenuObj.SetActive(false);
            boardObject = GameObject.FindGameObjectWithTag("board");
            boardObject.transform.SetParent(boardContainer.gameObject.transform);//make it current game object child
            boardObject.transform.localPosition = new Vector3(0.0f, 0.0f,-3.0f);
            //create scenario according to the current  index, so need to pass the value of which index it is
            //Also show the menu for scenario creation
            state = 0;
        }
        else if (state == 2)
        {

            //loadscenario according to the current  index, so need to pass the value of which index it is
        }


    }

    private void FieldViewEvent()
    {
        FieldViewMenu.SetActive(false);
        //below to save the last screen active
        LastUILayout.Clear();
        LastUILayout.Add(ScenarioMenuObj.activeSelf);
        LastUILayout.Add(SelectScenarioMenuObj.activeSelf);
        LastUILayout.Add(ScenarioEditorObj.activeSelf);
        LastUILayout.Add(Logo.activeSelf);

        //now change everthing into inactive and activate the field view menu

        Logo.SetActive(false);
        ScenarioMenuObj.SetActive(false);
        SelectScenarioMenuObj.SetActive(false);
        ScenarioEditorObj.SetActive(false);
        FieldViewButton.gameObject.SetActive(false);
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
        RunScenarioMenu.SetActive(true);
        gameObject.GetComponent<Image>().enabled = false;
        FieldViewMenu.SetActive(false);
        FieldViewButton.gameObject.SetActive(false);
        //  FieldViewEvent();
        // SelectScenarioMenuObj.SetActive(true);
        //state = 2;
        //Set trained player

    }

    private void CreateNewScenario()
    {
        Logo.SetActive(false);
        ScenarioMenuObj.SetActive(false);
        SelectScenarioMenuObj.SetActive(true);
        state = 1;

    }

}
