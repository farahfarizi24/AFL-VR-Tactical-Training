using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RunScenario : MonoBehaviour
{


    public ScenarioCreation_UI ScenarioCreation;
    public ScenarioCreation_Function ScenarioLoader;



    public Dropdown ScenarioDropdown;
    public Button SelectButton;
    public int scenarioNumber;
    public GameObject PlayScenarioMenu;
    public GameObject ViewFieldMenu;
    public Button QueueScenarios;
    public Button PlayScenarios;
    public Button MainMenu;
    public GameObject LoadScenarioMenu;
    // Start is called before the first frame update
    void Start()
    {
        AddButtonListeners();
        
    }


    private void AddButtonListeners()
    {//add button option
        SelectButton.onClick.AddListener(SelectScenarioToRun);
        QueueScenarios.onClick.AddListener(QueueScenarioMenu);
        PlayScenarios.onClick.AddListener(LoadScenario);
        MainMenu.onClick.AddListener(OpenMainMenu);
        //add dropdown option
        ScenarioDropdown.ClearOptions();
        ScenarioDropdown.AddOptions(ScenarioCreation.scenarios);

    }

    private void OpenMainMenu()
    {
        ViewFieldMenu.SetActive(true);
        GetComponent<Image>().enabled = true;
        PlayScenarioMenu.SetActive(false);
        LoadScenarioMenu.SetActive(false);
        ScenarioCreation.FieldViewMenu.SetActive(false);
        ScenarioCreation.ScenarioMenuObj.SetActive(true);
        ScenarioCreation.SelectScenarioMenuObj.SetActive(false);
    }

    private void QueueScenarioMenu()
    {
        throw new NotImplementedException();
    }

    private void LoadScenario()
    {
        PlayScenarioMenu.SetActive(true);
        LoadScenarioMenu.SetActive(false);
        GetComponent<Image>().enabled = false;
        //hide white bg

        
    }

    private void SelectScenarioToRun()
    {
        scenarioNumber = ScenarioDropdown.value;
        ScenarioLoader.ScenarioNumber = scenarioNumber;
        ScenarioLoader.LoadScenario(ScenarioLoader.ScenarioNumber);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
